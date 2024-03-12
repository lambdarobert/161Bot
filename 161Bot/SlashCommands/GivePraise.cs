using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{

    [ChaoCommand("givepraise", "Add one to the praise count")]
    [ChaoPermissions(new ulong[] {}, 805875347044302868)]
    public class GivePraise : ChaoSlashCommand
    {

        public async Task Run(SocketSlashCommand cmd)
        {
            var theEmbed = new Discord.EmbedBuilder();
            theEmbed.WithTitle("Praise be to Chao!");
            theEmbed.WithColor(Color.Gold);
            theEmbed.WithDescription("You gave praise to Chao Chen.");
            theEmbed.WithFooter("Praise Count: " + BotConfig.GetCachedConfig().PraiseCount + "");
            theEmbed.WithCurrentTimestamp();
            await cmd.RespondAsync(embed: theEmbed.Build());
            BotConfig cfg = BotConfig.LoadConfig();
            cfg.PraiseCount = (Int64.Parse(cfg.PraiseCount) + 1).ToString();
            BotConfig.SaveConfig(cfg);
        }
    }
}
