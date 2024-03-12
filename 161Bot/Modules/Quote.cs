using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class Quote : ModuleBase<SocketCommandContext>
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

        [Command("quote", RunMode = RunMode.Async)]
        [Summary("Get a random quote from the server.")]
        public async Task Run()
        {
            if(messages.Count == 0)
            {
                await ReplyAsync(embed: QuickEmbeds.Error("This command is not ready yet. Please try again later."));
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
 
            await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), embed: theEmbed.Build());
        }
    }
}
