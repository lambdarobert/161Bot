using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    /*
     * Allow users to create their own voice channels dynamically.
     */

    public class VCChannelManager
    {

        private static ulong channelId = 817051019619467315;
        private static ulong messageChannel = 803012802612690944;
        private static ulong modRole = 796444864177766400;
        private static ulong catId = 795714784150159431;

        /*
         * Key represents the channel ID, and value is channel owner.
         */
        private static IDictionary<ulong, ulong> channelInfo = new Dictionary<ulong, ulong>();

        public async Task OnChannelJoinLeave(SocketUser usr, SocketVoiceState old, SocketVoiceState newState) {
            if(newState.VoiceChannel != null && usr is SocketGuildUser guildUsr && !(usr.IsBot))
            {
                if(newState.VoiceChannel.Id == channelId)
                {
                    if(channelInfo.Count < 5)
                    {
                        var channel = await guildUsr.Guild.CreateVoiceChannelAsync(guildUsr.Username + "'s Channel");
                        channelInfo.Add(channel.Id, guildUsr.Id);
                        await channel.ModifyAsync((m) =>
                        {
                            m.UserLimit = 99;
                        m.CategoryId = newState.VoiceChannel.CategoryId;
                            var list = new List<Overwrite>();
                            list.Add(new Overwrite(guildUsr.Id, PermissionTarget.User, new OverwritePermissions(
                                createInstantInvite: PermValue.Deny,
                                manageRoles: PermValue.Allow,
                                manageChannel: PermValue.Allow,
                                muteMembers: PermValue.Allow,
                                prioritySpeaker: PermValue.Allow,
                                deafenMembers: PermValue.Allow,
                                moveMembers: PermValue.Deny
                                ))); // owner permissions
                            list.Add(new Overwrite(modRole, PermissionTarget.Role, OverwritePermissions.AllowAll(channel)));
                            m.PermissionOverwrites = list;

                        }); // place channel in correct category
                        await guildUsr.ModifyAsync(m =>
                        {
                            m.Channel = channel;
                        });
                    }
                    else
                    {
                        await newState.VoiceChannel.Guild.GetTextChannel(messageChannel).SendMessageAsync(usr.Mention + " No more channels can be created.");
                        await guildUsr.ModifyAsync((m) =>
                        {
                            m.Channel = null;
                        }); // kick user from channel
                    }

                }
            }
            if(usr is SocketGuildUser gUsr)
            {
                foreach(var channel in gUsr.Guild.VoiceChannels)
                {
                    if(channelInfo.ContainsKey(channel.Id))
                    {
                        if(channel.Users.Count == 0)
                        {
                            var id = channel.Id;
                            await channel.DeleteAsync();
                            channelInfo.Remove(id);
                        }
                    }
                }
            }
        }

        public async Task OnChannelModified(SocketChannel chan1, SocketChannel chan2)
        {
            if(channelInfo.ContainsKey(chan2.Id))
            {
               if(chan2 is SocketVoiceChannel svc && chan1 is SocketVoiceChannel)
                {
                    if (svc.CategoryId != catId)
                    {
                        await svc.ModifyAsync(m =>
                        {
                            m.CategoryId = catId;
                        });
                    }
                }
            }
        }

        public async Task OnVoiceChannelDestroyed(SocketChannel chan)
        {
            if(channelInfo.ContainsKey(chan.Id))
            {
                Console.WriteLine("Sanitizing channel from VC manager list" + chan.Id);
                channelInfo.Remove(chan.Id);
            }
        }
    }
}
