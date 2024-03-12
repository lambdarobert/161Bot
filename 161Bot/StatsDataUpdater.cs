using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    public class StatsDataUpdater
    {
        public async Task Handle(SocketMessage msg)
        {
            bool containsBased = msg.Content.ToLower().Contains("based");
            bool containsCringe = msg.Content.ToLower().Contains("cringe");
            bool containsGif = msg.Content.ToLower().Contains(".gif") || msg.Content.ToLower().Contains("https://tenor.com/view/");
            foreach(var attachment in msg.Attachments)
            {
                if(attachment.Filename.ToLower().EndsWith("gif"))
                {
                    containsGif = true;
                }
            }
            if(containsBased || containsCringe || containsGif)
            {
                if(BotConfig.GetCachedConfig().UserStats == null)
                {
                    BotConfig.GetCachedConfig().UserStats = new Dictionary<ulong, Stats>();
                }
                if(!BotConfig.GetCachedConfig().UserStats.ContainsKey(msg.Author.Id))
                {
                    BotConfig.GetCachedConfig().UserStats[msg.Author.Id] = new Stats()
                    {
                        Based = 0,
                        Cringe = 0,
                        Gifs = 0
                    };
                }
                if(containsBased)
                {
                    BotConfig.GetCachedConfig().UserStats[msg.Author.Id].Based++;
                }
                if (containsCringe)
                {
                    BotConfig.GetCachedConfig().UserStats[msg.Author.Id].Cringe++;
                }
                if (containsGif)
                {
                    BotConfig.GetCachedConfig().UserStats[msg.Author.Id].Gifs++;
                }
                BotConfig.SaveConfig(BotConfig.GetCachedConfig());
                BotConfig.LoadConfig();
            }
            
        }
    }
}
