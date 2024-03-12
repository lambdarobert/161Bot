using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{

    [ChaoCommand("emote", "Allows you to display an emote, regardless of nitro status.")]
    public class EmoteCommand : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("emoteid", "Use /wtfemojis to see the list.")] string emoteId)
        {
            var guild = (cmd.Channel as SocketGuildChannel).Guild;
            if(await guild.GetEmoteAsync(Convert.ToUInt64(emoteId)) != null)
            {
                var emote = await guild.GetEmoteAsync(Convert.ToUInt64(emoteId));
                await cmd.RespondAsync(emote.ToString());
            }
            else
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("Not a valid emote ID. Use !wtfemojis to see the list of valids."), ephemeral: true);
                return;
            }
        }
    }
}
