using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    public class Greentext
    {
        public async Task OnMessage(SocketMessage msg)
        {
            if(msg.Content.Length != 0)
            {
                if (msg.Content.StartsWith(">"))
                {
                    await (msg as SocketUserMessage).DeleteAsync();
                    var msgembed = new EmbedBuilder();
                    msgembed.WithAuthor(msg.Author.Username + "#" + msg.Author.Discriminator, msg.Author.GetAvatarUrl());
                    msgembed.WithColor(Color.Green);
                    msgembed.WithDescription(msg.Content.Substring(1) + "\n" + msg.Author.Mention);
                    await msg.Channel.SendMessageAsync(embed: msgembed.Build());
                }
            }
        }
    }
}
