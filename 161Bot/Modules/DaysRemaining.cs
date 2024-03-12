using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class DaysRemaining : ModuleBase<SocketCommandContext>
    {
        [Command("daysremaining")]
        [Summary("Get the numbers of days remaining in this term.")]
        [Alias("days", "getdays")]
        public async Task Run()
        {
            DateTime d1 = new DateTime(year: 2021, month: 3, day: 19, hour: 23, minute: 59, second: 59, millisecond: 0, kind: DateTimeKind.Local);
            DateTime d2 = DateTime.Now;
            TimeSpan diff = d1 - d2;
            await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), embed: new EmbedBuilder().WithTitle("Days Until End of Winter Term").WithDescription(
                "Days: " + diff.Days.ToString() + "\n" +
                "+ Hours: " + diff.Hours.ToString() + "\n" +
                "+ Minutes: " + diff.Minutes.ToString() + "\n" +
                "+ Seconds: " + diff.Seconds.ToString() + "\n"
                ).WithColor(Color.Blue).WithFooter("Winter term ends at: " + d1.ToString() + " • Time on Server: " + d2.ToString()).Build());
        }
    }
}
