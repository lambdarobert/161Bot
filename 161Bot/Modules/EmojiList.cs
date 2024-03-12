using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class EmojiList : ModuleBase<SocketCommandContext>
    {
        [Command("wtfemojis")]
        public async Task Run()
        {
            StringBuilder sb = new StringBuilder("**Emojis in this Guild**: \n");
            var guild = Context.Guild;
            foreach(var emote in guild.Emotes)
            {
                sb.Append(emote.Name + " with ID " + emote.Id + "\n");
            }
            await ReplyAsync(sb.ToString());
        }
    }
}
