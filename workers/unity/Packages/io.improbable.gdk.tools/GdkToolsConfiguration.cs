using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Improbable.Gdk.Tools
{
    [Serializable]
    public class GdkToolsConfiguration
    {
        public List<string> SchemaSourceDirs = new List<string>();
        public bool VerboseLogging;
        public string CodegenLogOutputDir;
        public string CodegenOutputDir;
        public string DescriptorOutputDir;
        public List<string> SerializationOverrides = new List<string>();

        public string DevAuthTokenDir;
        public int DevAuthTokenLifetimeDays;
        public bool SaveDevAuthTokenToFile;

        public string EnvironmentPlatform;
        public string RuntimeVersionOverride;

        public string DevAuthTokenFullDir => Path.Combine(Application.dataPath, DevAuthTokenDir);
        public string DevAuthTokenFilepath => Path.Combine(DevAuthTokenFullDir, "DevAuthToken.txt");
        public int DevAuthTokenLifetimeHours => TimeSpan.FromDays(DevAuthTokenLifetimeDays).Hours;

        public string FullCodegenOutputPath => Path.GetFullPath(Path.Combine(Application.dataPath, "..", CodegenOutputDir));
        public string DefaultCodegenLogPath => Path.GetFullPath(Path.Combine(CodegenLogOutputDir, "codegen-output.log"));

        public string RuntimeVersion => string.IsNullOrEmpty(RuntimeVersionOverride)
            ? DefaultValues.RuntimeVersion.Value
            : RuntimeVersionOverride;

        private const string CustomSnapshotPathPrefKey = "CustomSnapshotPath";

        public string CustomSnapshotPath
        {
            get => PlayerPrefs.GetString(CustomSnapshotPathPrefKey, Path.GetFullPath("../../snapshots/default.snapshot"));
            set => PlayerPrefs.SetString(CustomSnapshotPathPrefKey, value);
        }

        private static readonly string JsonFilePath = Path.GetFullPath("Assets/Config/GdkToolsConfiguration.json");

        private GdkToolsConfiguration()
        {
            ResetToDefault();
        }

        public void Save()
        {
            var json = JsonUtility.ToJson(this, true);
            File.WriteAllText(JsonFilePath, json);

            GenerateCode.GenerateCodegenRunConfigs();
        }

        internal List<string> Validate()
        {
            var errors = new List<string>();

            if (string.IsNullOrEmpty(CodegenOutputDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.CodegenLogOutputDirLabel} cannot be empty.");
            }

            if (string.IsNullOrEmpty(CodegenOutputDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.CodegenOutputDirLabel} cannot be empty.");
            }

            if (string.IsNullOrEmpty(DescriptorOutputDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.DescriptorOutputDirLabel} cannot be empty.");
            }

            for (var i = 0; i < SchemaSourceDirs.Count; i++)
            {
                var schemaSourceDir = SchemaSourceDirs[i];

                if (string.IsNullOrEmpty(schemaSourceDir))
                {
                    errors.Add($"Schema path [{i}] is empty. You must provide a valid path.");
                    continue;
                }

                try
                {
                    var fullSchemaSourceDirPath = Path.Combine(Application.dataPath, "..", schemaSourceDir);
                    if (!Directory.Exists(fullSchemaSourceDirPath))
                    {
                        errors.Add($"{fullSchemaSourceDirPath} cannot be found.");
                    }
                }
                catch (ArgumentException)
                {
                    errors.Add($"Schema path [{i}] contains one or more invalid characters.");
                }
            }

            if (!SaveDevAuthTokenToFile)
            {
                return errors;
            }

            if (string.IsNullOrEmpty(DevAuthTokenDir))
            {
                errors.Add($"{GdkToolsConfigurationWindow.DevAuthTokenDirLabel} cannot be empty.");
            }
            else if (!DevAuthTokenDir.Equals("Resources") && !DevAuthTokenDir.EndsWith("/Resources"))
            {
                errors.Add(
                    $"{GdkToolsConfigurationWindow.DevAuthTokenDirLabel} must be at root of a Resources folder.");
            }

            return errors;
        }

        internal void ResetToDefault()
        {
            SchemaSourceDirs.Clear();
            SchemaSourceDirs.Add(DefaultValues.SchemaSourceDir);
            VerboseLogging = DefaultValues.VerboseLogging;
            CodegenLogOutputDir = DefaultValues.CodegenLogOutputDir;
            CodegenOutputDir = DefaultValues.CodegenOutputDir;
            DescriptorOutputDir = DefaultValues.DescriptorOutputDir;
            SerializationOverrides.Clear();

            DevAuthTokenDir = DefaultValues.DevAuthTokenDir;
            DevAuthTokenLifetimeDays = DefaultValues.DevAuthTokenLifetimeDays;
            SaveDevAuthTokenToFile = false;

            EnvironmentPlatform = string.Empty;
            RuntimeVersionOverride = string.Empty;
        }

        public static GdkToolsConfiguration GetOrCreateInstance()
        {
            return File.Exists(JsonFilePath) ? LoadFromFile() : CreateInstance();
        }

        private static GdkToolsConfiguration LoadFromFile()
        {
            return JsonUtility.FromJson<GdkToolsConfiguration>(File.ReadAllText(JsonFilePath));
        }

        private static GdkToolsConfiguration CreateInstance()
        {
            var config = new GdkToolsConfiguration();

            File.WriteAllText(JsonFilePath, JsonUtility.ToJson(config, true));

            return config;
        }

        private static class DefaultValues
        {
            public const bool VerboseLogging = false;
            public const string CodegenLogOutputDir = "logs/";
            public const string CodegenOutputDir = "Assets/Generated/Source";
            public const string DescriptorOutputDir = "../../build/assembly/schema";
            public const string SchemaSourceDir = "../../schema";
            public const string DevAuthTokenDir = "Resources";
            public const int DevAuthTokenLifetimeDays = 30;

            // NOTE: Cannot use regular static initialization as we cannot read package metadata when it is
            // initialized. We also don't want to hit the filesystem every single time we access this. So we use
            // Lazy<T>!
            public static readonly Lazy<string> RuntimeVersion = new Lazy<string>(() => File.ReadAllText(PinnedRuntimeFilePath).Trim());
            private static string PinnedRuntimeFilePath => Path.Combine(Common.GetThisPackagePath(), "runtime.pinned");
        }
    }
}
