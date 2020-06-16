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

function runTests {
    local platform=$1
    local category=$2
    local burst=$3
    local apiProfile=$4

    local scriptingBackend="mono"
    local args=()

    if [[ "${platform}" == "editmode" ]]; then
        args+=("-runEditorTests")
    else
        scriptingBackend=$5
        args+=("-runTests -testPlatform ${platform}")
        args+=("-testSettingsFile ${TEST_SETTINGS_DIR}/${scriptingBackend}-${apiProfile}.json")
    fi

    if [[ "${burst}" == "burst-disabled" ]]; then
        args+=("--burst-disable-compilation")
    fi

    pushd "workers/unity"
        dotnet run -p "${PROJECT_DIR}/.shared-ci/tools/RunUnity/RunUnity.csproj" -- \
            -batchmode \
            -projectPath "${PROJECT_DIR}/workers/unity" \
            "${ACCELERATOR_ARGS}" \
            -logfile "${PROJECT_DIR}/logs/${platform}-${burst}-${scriptingBackend}-${apiProfile}-perftest-run.log" \
            -testResults "${TEST_RESULTS_DIR}/${platform}-${burst}-${scriptingBackend}-${apiProfile}-perftest-results.xml" \
            -testCategory "${category}" \
            "${args[@]}"
    popd
}

traceStart "Performance Testing: Editmode :writing_hand:"
    for burst in burst-default burst-disabled
    do
        for apiProfile in dotnet-std-2 dotnet-4
        do
            traceStart "${burst} ${apiProfile}"
                runTests "editmode" "Performance" ${burst} ${apiProfile}
            traceEnd
        done
    done
traceEnd

traceStart "Performance Testing: Playmode :joystick:"
    for burst in burst-default burst-disabled
    do
        for apiProfile in dotnet-std-2 dotnet-4
        do
            for scriptingBackend in mono il2cpp winrt
            do
                local platform="StandaloneWindows64"

                traceStart "${platform} ${burst} ${scriptingBackend} ${apiProfile}"
                    runTests ${platform} "Performance" ${burst} ${apiProfile} ${scriptingBackend}
                traceEnd
            done
        done
    done
traceEnd

cleanUnity "$(pwd)/workers/unity"
