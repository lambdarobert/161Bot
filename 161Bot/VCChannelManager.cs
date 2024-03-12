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
        public static IDictionary<ulong, ulong> ChannelInfo = new Dictionary<ulong, ulong>();

        public async Task OnChannelJoinLeave(SocketUser usr, SocketVoiceState old, SocketVoiceState newState) {
            if(newState.VoiceChannel != null && usr is SocketGuildUser guildUsr && !(usr.IsBot))
            {
                if(newState.VoiceChannel.Id == channelId)
                {
                    if(ChannelInfo.Count < 5)
                    {
                        var channel = await guildUsr.Guild.CreateVoiceChannelAsync(guildUsr.Username + "'s Channel");
                        ChannelInfo.Add(channel.Id, guildUsr.Id);
                        await channel.ModifyAsync((m) =>
                        {
                            m.UserLimit = 99;
                            if(guildUsr.Guild.GetChannel(channelId) != null)
                            {
                                m.Position = guildUsr.Guild.GetChannel(channelId).Position - 1;
                            }
                        m.CategoryId = newState.VoiceChannel.CategoryId;
                            var list = new List<Overwrite>();
                            list.Add(new Overwrite(guildUsr.Id, PermissionTarget.User, new OverwritePermissions(
                                createInstantInvite: PermValue.Deny,
                                manageChannel: PermValue.Allow,
                                muteMembers: PermValue.Allow,
                                connect: PermValue.Allow,
                                prioritySpeaker: PermValue.Allow
                                ))); // owner permissions
                            list.Add(new Overwrite(829525044539031592, PermissionTarget.Role, new OverwritePermissions(
    speak: PermValue.Deny,
    stream: PermValue.Deny


    )));
                            list.Add(new Overwrite(modRole, PermissionTarget.Role, OverwritePermissions.AllowAll(channel)));

                            m.PermissionOverwrites = list;

                        }); // place channel in correct category
                            await guildUsr.ModifyAsync(m =>
                            {
                                if(guildUsr.VoiceChannel != null)
                                {
                                    m.Channel = channel;
                                }
                            });
                        // handle if the user joins and then leaves 
                            var list = BotConfig.GetCachedConfig().VCManagerMessagedUsers;
                            if(!list.Contains(guildUsr.Id))
                            {
                            await guildUsr.SendMessageAsync(embed: new EmbedBuilder()
                                .WithColor(Color.Green)
                                .WithTitle("Custom Channels")
                                .WithDescription("You've created a custom channel. You can customize this channel in the channel settings, but don't change the name. If a user is being a turd, you can disconnect them with the !dusr <user id> command. Enjoy!" +
                                "\n\n If you need to get a User ID, first make sure you have developer mode enabled. Then right click on the user and hit 'Copy ID'. You can't use an @mention for the bot command.")
                                .WithFooter("This message will only show once.")
                                .Build()
                                );
                            var config = BotConfig.GetCachedConfig();
                            var ls = config.VCManagerMessagedUsers;
                            ls.Add(guildUsr.Id);
                            config.VCManagerMessagedUsers = ls;
                            BotConfig.SaveConfig(config);
                        }

                        
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
                    if(ChannelInfo.ContainsKey(channel.Id))
                    {
                        if(channel.Users.Count == 0)
                        {
                            var id = channel.Id;
                            await channel.DeleteAsync();
                            ChannelInfo.Remove(id);
                        }

                    }
                }
            }
        }

        public async Task OnChannelModified(SocketChannel chan1, SocketChannel chan2)
        {
            if(ChannelInfo.ContainsKey(chan2.Id))
            {
               if(chan2 is SocketVoiceChannel svc2 && chan1 is SocketVoiceChannel svc1)
                {
                    if (svc2.CategoryId != catId)
                    {
                        await svc2.ModifyAsync(m =>
                        {
                            m.CategoryId = catId;
                        });
                    }
                    if(svc2.Name != svc1.Name)
                    {
                        var usr = svc2.Guild.GetUser(ChannelInfo[svc2.Id]);
                        if (usr != null) {
                            if(usr.GuildPermissions.Administrator || usr.GuildPermissions.ManageChannels)
                            {
                                Console.WriteLine("Permission bypass for rename channel");
                                return;
                            }
                        }
                       if(svc2.Guild.GetTextChannel(messageChannel)  != null)
                        {
                            await svc2.Guild.GetTextChannel(messageChannel).SendMessageAsync(embed: QuickEmbeds.Error("Renaming of custom channels is currently not allowed to prevent abuse of the system."));
                        }
                        await svc2.DeleteAsync();
                    }
                }
            }
        }

        public async Task OnVoiceChannelDestroyed(SocketChannel chan)
        {
            if(ChannelInfo.ContainsKey(chan.Id))
            {
                Console.WriteLine("Sanitizing channel from VC manager list" + chan.Id);
                ChannelInfo.Remove(chan.Id);
            }
        }
    }
}
