using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace DD2Bot.UI
{
    public class Configuration
    {
        public static readonly string StoragePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        public static readonly string TemplatesPath = StoragePath + "\\templates";
        public static readonly string RecordsPath = StoragePath + "\\records";
        public static readonly string ConfigurationFilePath = StoragePath + "\\dd2bot.json";

        public class ConfigurationSerializer
        {
            public string TemplatesPath { get; set; }
            public string RecordsPath { get; set; }
            public string ConfigurationFilePath { get; set; }

            public ConfigurationSerializer(string templatesPath, string recordsPath, string configurationFilePath)
            {
                TemplatesPath = templatesPath;
                RecordsPath = recordsPath;
                ConfigurationFilePath = configurationFilePath;
            }
        }

        public static void Bootstrap()
        {
            var configurationFileInfo = new FileInfo(ConfigurationFilePath);
            var subdirectories = new string[]
            {
                TemplatesPath,
                RecordsPath,
            };

            if (!configurationFileInfo.Exists)
            {
                CreateConfigurationFile();

                foreach (var dir in subdirectories)
                {
                    var dirInfo = new DirectoryInfo(dir);
                    dirInfo.Create();
                }
            }
        }

        private static void CreateConfigurationFile()
        {
            var stream = new FileStream(ConfigurationFilePath, FileMode.Create);
            var jsonSerializer = new DataContractJsonSerializer(typeof(Configuration));
            jsonSerializer.WriteObject(stream, new ConfigurationSerializer(
                TemplatesPath, RecordsPath, ConfigurationFilePath));
        }
    }
}
