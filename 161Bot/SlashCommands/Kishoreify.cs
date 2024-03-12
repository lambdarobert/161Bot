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
    [ChaoCommand("kishoreify", "Prints a kishoreified text back to you")]
    public class Kishoreify : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd)
        {
            var modal = new ModalBuilder();
            modal.WithTitle("Kishoreify");
            modal.WithCustomId("KISHOREIFY_MODAL");
            modal.AddTextInput("Content", "content", TextInputStyle.Paragraph, placeholder: "The text you want to Kishoreify.");
            modal.AddTextInput("Kishorification", "level", TextInputStyle.Short, placeholder: "From 1 to 5");
            await cmd.RespondWithModalAsync(modal.Build());
        }
    }
}
