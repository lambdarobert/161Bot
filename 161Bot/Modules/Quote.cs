using Discord;
using Discord.Commands;
using Discord.Rest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class Quote : ModuleBase<SocketCommandContext>
    {

        public static ulong? lastMessage = null;

        [Command("quote", RunMode = RunMode.Async)]
        public async Task Run()
        {
            List<RestMessage> pins = new List<RestMessage>();
            foreach(var channel in Context.Guild.TextChannels)
            {
                foreach(var msg in await channel.GetPinnedMessagesAsync() as IReadOnlyList<RestMessage>)
                {
                    if(msg.Id != lastMessage)
                    {
                        pins.Add(msg);
                    }
                }
            }
            if(pins.Count == 0)
            {
                await ReplyAsync("No quotes in this channel :(");
                return;
            }
            var message = pins[new Random().Next(0, pins.Count - 1)];
            var theEmbed = new EmbedBuilder();
            theEmbed.WithAuthor(message.Author.Username + "#" +  message.Author.Discriminator);
            theEmbed.WithAuthor(new EmbedAuthorBuilder().
                WithIconUrl(message.Author.GetAvatarUrl()).
                WithName(message.Author.Username + "#" + message.Author.Discriminator).
                WithUrl(message.GetJumpUrl()))
                .Build();
            theEmbed.WithCurrentTimestamp();
            theEmbed.WithFooter(message.Author.Id.ToString());
            theEmbed.WithDescription(message.Content);
            theEmbed.WithColor(Color.DarkPurple);
            foreach(var a in message.Attachments)
            {
                theEmbed.ThumbnailUrl = a.ProxyUrl;
            }
            lastMessage = message.Id;
            await ReplyAsync(embed: theEmbed.Build());
        }
    }
}
