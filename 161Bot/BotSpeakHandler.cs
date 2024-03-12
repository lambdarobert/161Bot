using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    public class BotSpeakHandler
    {
        public async Task HandleMessage(SocketMessage msg)
        {
            if(msg.Channel.Id == 814235529910419456)
            {
                if(msg.Channel is SocketGuildChannel gc && msg.Channel is SocketTextChannel sc && Convert.ToUInt64(sc.Topic) != 0) {
                    if(gc.Guild.GetChannel(Convert.ToUInt64(sc.Topic)) != null)
                    {
                        var msgChan = gc.Guild.GetChannel(Convert.ToUInt64(sc.Topic)) as SocketTextChannel;
                        await msgChan.SendMessageAsync(msg.Content, msg.IsTTS, messageReference: msg.Reference);
                    }
                }
            }
        }
    }
}
