using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class PrayToChao : ModuleBase<SocketCommandContext>
    {
        [Command("pray")]
        public async Task Run()
        {
            var theEmbed = new Discord.EmbedBuilder();
            theEmbed.WithDescription("As you collapse and tremble on your knees, you do a prayer like a simp worships a girl on OnlyFans. You think of the next lecture when he will just interrupt at any minute and say 'HI PROFESSOR'.");
            theEmbed.WithTitle("You pray to Chao Chen. +5 Praise!");
            theEmbed.WithColor(new Discord.Color(0xff1612));
            theEmbed.WithThumbnailUrl("https://robertserver.s3-us-west-2.amazonaws.com/pray.jpg");
            theEmbed.WithFooter("Praise Count: " + BotConfig.GetCachedConfig().PraiseCount);
            theEmbed.WithCurrentTimestamp();
            await ReplyAsync(embed : theEmbed.Build());
            BotConfig cfg = BotConfig.LoadConfig();
            Console.WriteLine("Praise count is " + cfg.PraiseCount);
            cfg.PraiseCount = (Int64.Parse(cfg.PraiseCount) + 5).ToString();
            Console.WriteLine("Praise count is " + cfg.PraiseCount);
            BotConfig.SaveConfig(cfg);
        }
    }
}
