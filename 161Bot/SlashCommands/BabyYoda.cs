using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{
    [ChaoCommand("babyyoda", "See a gif of 'The Child'")]
    public class BabyYoda : ChaoSlashCommand
    {
        private static string lastUrl; // keeps the URLs fresh

        public async Task Run(SocketSlashCommand cmd)
        {
            IList<string> urls = BotConfig.GetCachedConfig().BabyYodaUrls.Where(url => url != lastUrl).ToList<string>();
            string url = urls[new Random().Next(0, urls.Count() - 1)];
            lastUrl = url;
            await cmd.RespondAsync(embed: new EmbedBuilder().WithColor(Color.Green).WithTitle("BABY YODA").WithCurrentTimestamp().WithImageUrl(url).Build());
        }
    }
}
