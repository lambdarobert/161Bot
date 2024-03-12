using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
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
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("text", "The text you want kishoreified")] string text, [ChaoParameter("kishoreification", "1 = Maximum Kishoredrive, 5 = Minimal")] long kishoreification)
        {
            if(kishoreification < 1 || kishoreification > 5)
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("Kishoreification must be in range 1 to 5."), ephemeral: true);
                return;
            }
            string newStr = "";
            var rnd = new Random();
            foreach (char c in text)
            {
                bool b = rnd.Next((int) kishoreification + 1) == 1;
                if (b)
                {
                    newStr += Char.ToUpper(c);
                }
                else
                {
                    newStr += c;
                }
            }
            await cmd.RespondAsync(newStr, ephemeral: true);
        }
    }
}
