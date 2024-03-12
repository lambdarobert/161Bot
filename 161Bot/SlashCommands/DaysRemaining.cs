using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{

    [ChaoCommand("daysremaining", "Get the numbers of days remaining in this term.")]
    public class DaysRemaining : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd)
        {
                var l = (long)BotConfig.GetCachedConfig().CurrentTermEnds;
                DateTime d1 = DateTimeOffset.FromUnixTimeSeconds(l).DateTime;
                DateTime d2 = DateTime.Now;
                TimeSpan diff = d1 - d2;
                await cmd.RespondAsync(embed: new EmbedBuilder().WithTitle("Days Until End of " + BotConfig.GetCachedConfig().CurrentTerm).WithDescription(
                    "Days: " + diff.Days.ToString() + "\n" +
                    "+ Hours: " + diff.Hours.ToString() + "\n" +
                    "+ Minutes: " + diff.Minutes.ToString() + "\n" +
                    "+ Seconds: " + diff.Seconds.ToString() + "\n"
                    ).WithColor(Color.Blue).WithFooter(BotConfig.GetCachedConfig().CurrentTerm + " ends at: " + d1.ToString() + " • Time on Server: " + d2.ToString()).Build());


        }
    }
}
