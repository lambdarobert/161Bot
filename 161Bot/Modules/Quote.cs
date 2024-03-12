using Discord;
using Discord.Commands;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class Quote : ModuleBase<SocketCommandContext>
    {

        public static ulong? lastMessage = null;

        [Command("quote", RunMode = RunMode.Async)]
        [Summary("Get a random quote from the server.")]
        public async Task Run() => await Task.Run(RunAsync);

        // this command can take some time, so it's run here instead
        private async Task RunAsync()
        {
            var dis = Context.Channel.EnterTypingState();
            List<RestMessage> pins = new List<RestMessage>();
            foreach (var channel in Context.Guild.TextChannels)
            {
                foreach (var msg in await channel.GetPinnedMessagesAsync() as IReadOnlyList<RestMessage>)
                {
                    // this fixes the problem where the same quote will appear multiple times in the same command run. It's not a big deal, but it can be annoying.
                    if (msg.Id != lastMessage)
                    {
                        pins.Add(msg);
                    }
                }
            }
            if (pins.Count == 0)
            {
                await ReplyAsync("No quotes in this channel :(");
                return;
            }
            var message = pins[new Random().Next(0, pins.Count - 1)];
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
            dis.Dispose();
            await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), embed: theEmbed.Build());
        }
    }
}
