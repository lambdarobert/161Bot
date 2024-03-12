using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class BabyYoda : ModuleBase<SocketCommandContext>
    {
        private static string lastUrl; // keeps the URLs fresh

        [Command("babyyoda")]
        [Summary("See a gif of 'The Child'")]
        [Alias("byoda", "thechild")]

        public async Task Run()
        {
            IList<string> urls = BotConfig.GetCachedConfig().BabyYodaUrls.Where(url => url != lastUrl).ToList<string>();
            string url = urls[new Random().Next(0, urls.Count() - 1)];
            lastUrl = url;
            await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), embed: new EmbedBuilder().WithColor(Color.Green).WithTitle("BABY YODA").WithCurrentTimestamp().WithImageUrl(url).Build());
        }
    }
}
