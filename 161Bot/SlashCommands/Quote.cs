using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{

    [ChaoCommand("quote", "Get a random quote from the server.")]
    public class Quote : ChaoSlashCommand
    {

        public static ulong? lastMessage = null;
        private static readonly ulong guildId = 795714783801245706;
        private static List<RestMessage> messages;


        // this command can take some time, so it's run here instead

        public static async Task GenerateQuotes(DiscordSocketClient client)
        {
            Console.WriteLine("updating quote cache");
            messages = new List<RestMessage>();
            var guild = client.GetGuild(guildId);
            foreach(var channel in guild.TextChannels)
            {
                foreach (var msg in await channel.GetPinnedMessagesAsync() as IReadOnlyList<RestMessage>)
                {
                    if(msg.Content != null && msg.Content != "")
                    {
                        messages.Add(msg);
                    }
                }
            }
        }

        public async Task Run(SocketSlashCommand cmd)
        {
            if(messages.Count == 0)
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("This command is not ready yet. Please try again later."), ephemeral: true);
                return;
            }
            Random r = new Random();

            var message = messages[r.Next(messages.Count)];

            var theEmbed = new EmbedBuilder();
            theEmbed.WithAuthor(message.Author.Username + "#" + message.Author.Discriminator);
            theEmbed.WithAuthor(new EmbedAuthorBuilder().
                WithIconUrl(message.Author.GetAvatarUrl()).
                WithName(message.Author.Username + "#" + message.Author.Discriminator).
                WithUrl(message.GetJumpUrl()))
                .Build();
            theEmbed.WithCurrentTimestamp();
            theEmbed.WithFooter(message.Author.Id.ToString());
            theEmbed.WithDescription(message.Content);
            theEmbed.WithColor(Color.DarkPurple);
            foreach (var a in message.Attachments)
            {
                theEmbed.ThumbnailUrl = a.ProxyUrl;
            }
            lastMessage = message.Id;
 
            await cmd.RespondAsync(embed: theEmbed.Build());
        }
    }
}
