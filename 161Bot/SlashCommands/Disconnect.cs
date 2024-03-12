using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{
    [ChaoCommand("kickvc", "Allows the owner of a custom channel to kick a user.")]
    public class Disconnect : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("user", "The user to boot")] IGuildUser usr)
        {
           if(usr.VoiceChannel != null)
           {
                if(VCChannelManager.ChannelInfo.ContainsKey(usr.VoiceChannel.Id))
                {
                    if(usr.Id == VCChannelManager.ChannelInfo[usr.VoiceChannel.Id])
                    {
                        await usr.ModifyAsync(m =>
                        {
                            m.Channel = null;
                        });
                        await cmd.RespondAsync(embed: new EmbedBuilder().WithColor(Color.Green).WithTitle("Disconnected User").Build());
                    }
                    else
                    {
                        await cmd.RespondAsync(embed: QuickEmbeds.PermissionError(), ephemeral: true);
                    }
                }
                else
                {
                    await cmd.RespondAsync(embed: QuickEmbeds.PermissionError(), ephemeral: true);
                }
           }
           else
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("Could not find user or they are not currently in a voice channel."), ephemeral: true);
            }
        }
    }
}
