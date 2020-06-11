#!/usr/bin/env bash

set -e -u -o pipefail

if [[ -n "${DEBUG-}" ]]; then
    set -x
fi

cd "$(dirname "$0")/../"

source .shared-ci/scripts/pinned-tools.sh

ACCELERATOR_ARGS=$(getAcceleratorArgs)

PROJECT_DIR="$(pwd)"
TEST_RESULTS_DIR="${PROJECT_DIR}/logs/nunit"
mkdir -p "${TEST_RESULTS_DIR}"

TEST_SETTINGS_DIR="${PROJECT_DIR}/workers/unity/Packages/io.improbable.gdk.testutils/TestSettings"
MONO_BACKEND="${TEST_SETTINGS_DIR}/Mono.json"
IL2CPP_BACKEND="${TEST_SETTINGS_DIR}/IL2CPP.json"

traceStart "Testing Unity: Editmode :writing_hand:"
    pushd "workers/unity"
        traceStart "Editmode - Burst on"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runEditorTests \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/editmode-burst-perftest-run.log" \
                -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-burst-perftest-results.xml" \
                "${ACCELERATOR_ARGS}"
        traceEnd

        traceStart "Editmode - Burst off"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runEditorTests \
                -testCategory "Performance,BurstOff" \
                -logfile "${PROJECT_DIR}/logs/editmode-burstoff-perftest-run.log" \
                -editorTestsResultFile "${TEST_RESULTS_DIR}/editmode-burstoff-perftest-results.xml" \
                "${ACCELERATOR_ARGS}"
        traceEnd
    popd
traceEnd

traceStart "Testing Unity: Playmode :joystick:"
    pushd "workers/unity"
        traceStart "Playmode - Burst on - Mono"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance" \
                -logfile "${PROJECT_DIR}/logs/playmode-burst-mono-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burst-mono-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $MONO_BACKEND
        traceEnd

        traceStart "Playmode - Burst on - IL2CPP"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance,Il2CppBackend" \
                -logfile "${PROJECT_DIR}/logs/playmode-burst-il2cpp-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burst-il2cpp-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $IL2CPP_BACKEND
        traceEnd

        traceStart "Playmode - Burst off - Mono"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance,BurstOff" \
                -logfile "${PROJECT_DIR}/logs/playmode-burstoff-mono-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burstoff-mono-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $MONO_BACKEND \
                --burst-disable-compilation
        traceEnd

        traceStart "Playmode - Burst off - IL2CPP"
            dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
                -batchmode \
                -projectPath "${PROJECT_DIR}/workers/unity" \
                -runTests \
                -testPlatform playmode \
                -testCategory "Performance,BurstOff,Il2CppBackend" \
                -logfile "${PROJECT_DIR}/logs/playmode-burstoff-il2cpp-perftest-run.log" \
                -testResults "${TEST_RESULTS_DIR}/playmode-burstoff-il2cpp-perftest-results.xml" \
                "${ACCELERATOR_ARGS}" \
                -testSettingsFile $IL2CPP_BACKEND \
                --burst-disable-compilation
        traceEnd
    popd
traceEnd

cleanUnity "$(pwd)/workers/unity"
