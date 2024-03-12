using System.Collections.Generic;
using System.IO;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace _161Bot
{

    public class VCHandlerConfig
    {
        public ulong ChannelId { get; set; }
        public ulong RoleId { get; set; }
    }

    public class BotConfig
    {
        public string PraiseCount { get; set; }
        public string DiscordToken { get; set; }

        public ulong ServerGuild { get; set; }
        public string BotPrefix { get; set; }

        public string RandomDogUrl { get; set; }
        public string ChaoPrayThumbnail { get; set; }

        public IList<string> BabyYodaUrls { get; set; }
        public VCHandlerConfig VcConfig { get; set; }

        private static BotConfig? configCache;

        // so we don't have to load the config every time a value needs to be read from it
        public static BotConfig GetCachedConfig()
        {
            if (configCache == null)
            {
                LoadConfig();
            }
            return configCache;
        }



        //loads config, forcing the cache to update
        public static BotConfig LoadConfig()
        {
            string fileContents = File.ReadAllText("config.yml");
            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var p = deserializer.Deserialize<BotConfig>(fileContents);
            configCache = p;
            return p;
        }

        public static void SaveConfig(BotConfig cfg)
        {
            var serializer = new SerializerBuilder()
                .WithNamingConvention(CamelCaseNamingConvention.Instance)
                .Build();
            var yml = serializer.Serialize(cfg);
            File.WriteAllText("config.yml", yml);
            LoadConfig(); // update the cache
        }

    }

}
