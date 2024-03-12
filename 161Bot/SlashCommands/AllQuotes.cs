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
    [ChaoCommand("allquotes", "See each and every quote")]
    public class AllQuotes : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd)
        {
            try
            {
                List<string> quotes = new List<string>();
                foreach (var quote in Quote.messages)
                {
                    quotes.Add("Quote by: " + quote.Author.Mention + "\n\n" + quote.Content);
                }
                var embed = new EmbedBuilder().WithTitle("All Quotes").WithColor(Color.Blue).Build();
                await Paginator.RespondWithPagination(cmd, embed, quotes.ToArray(), false);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }

        }
    }
}
