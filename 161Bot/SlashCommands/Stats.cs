using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{
    [ChaoCommand("stats", "See some interesting stats about a user")]
    public class Stats : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("victim", "Who do you want to run this command on?")] IGuildUser usr)
        {
            if(BotConfig.GetCachedConfig().UserStats != null && BotConfig.GetCachedConfig().UserStats.ContainsKey(usr.Id))
            {
                var info = BotConfig.GetCachedConfig().UserStats[usr.Id];
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Said based: " + info.Based);
                sb.AppendLine("Said cringe: " + info.Cringe);
                sb.AppendLine("Posted a GIF: " + info.Gifs);

                var embed = new EmbedBuilder();
                embed.WithTitle("Statistics for " + usr.Username + "#" + usr.Discriminator);
                embed.WithDescription(sb.ToString());
                embed.WithColor(Color.DarkGreen);

                await cmd.RespondAsync(embed: embed.Build());
            }
            else
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("There are no statistics for this user yet."));
            }
        }
    }
}
