using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{

    public class GivePraise : ModuleBase<SocketCommandContext>
    {
        [Command("givepraise")]
        public async Task Run()
        {
            var theEmbed = new Discord.EmbedBuilder();
            theEmbed.WithTitle("Praise be to Chao!");
            theEmbed.WithColor(Color.Gold);
            theEmbed.WithDescription("You gave praise to Chao Chen.");
            theEmbed.WithFooter("Praise Count: " + BotConfig.GetCachedConfig().PraiseCount + "");
            theEmbed.WithCurrentTimestamp();
            await ReplyAsync(embed: theEmbed.Build());
            BotConfig cfg = BotConfig.LoadConfig();
            cfg.PraiseCount = (Int64.Parse(cfg.PraiseCount) + 1).ToString();
            BotConfig.SaveConfig(cfg);
        }
    }
}
