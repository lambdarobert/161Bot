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
    [ChaoCommand("dayssurvived", "How many days have we survived K747?")]
    public class DaysSurvived : ChaoSlashCommand
    {
        private readonly DateTime D2 = new DateTime(2021, 3, 19);
        public async Task Run(SocketSlashCommand cmd)
        {
            DateTime d1 = DateTime.Now;
            await cmd.RespondAsync(embed: new EmbedBuilder()
                .WithTitle("Days Survived")
                .WithColor(Color.Blue)
                .WithDescription((d1 - D2).Days.ToString())
                .Build()
                );
        }
    }
}
