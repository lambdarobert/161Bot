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
        public async Task Run()
        {
            DateTime d1 = new DateTime(2021, 3, 19);
            DateTime d2 = DateTime.Now;
            TimeSpan diff = d1 - d2;
            await ReplyAsync(embed: new EmbedBuilder().WithTitle("Days Until End of Winter Term").WithDescription(diff.Days.ToString()).WithColor(Color.Blue).Build());
        }
    }
}
