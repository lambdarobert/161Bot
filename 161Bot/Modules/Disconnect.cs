using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class Disconnect : ModuleBase<SocketCommandContext>
    {

        [Command("disconnect")]
        [Summary("Allows the owner of a custom channel to kick a user. Syntax: disconnect <user id>")]
        [Alias("dusr", "dc")]

        public async Task Run(ulong userId)
        {
           if(Context.Guild.GetUser(userId) != null && Context.Guild.GetUser(userId).VoiceChannel != null)
           {
                var usr = Context.Guild.GetUser(userId);
                if(VCChannelManager.ChannelInfo.ContainsKey(usr.VoiceChannel.Id))
                {
                    if(Context.User.Id == VCChannelManager.ChannelInfo[usr.VoiceChannel.Id])
                    {
                        await usr.ModifyAsync(m =>
                        {
                            m.Channel = null;
                        });
                        await ReplyAsync(embed: new EmbedBuilder().WithColor(Color.Green).WithTitle("Disconnected User").Build());
                    }
                    else
                    {
                        await ReplyAsync(embed: QuickEmbeds.PermissionError());
                    }
                }
                else
                {
                    await ReplyAsync(embed: QuickEmbeds.PermissionError());
                }
           }
           else
            {
                await ReplyAsync(embed: QuickEmbeds.Error("Could not find user or they are not currently in a voice channel."));
            }
        }
    }
}
