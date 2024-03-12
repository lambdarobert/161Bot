using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    public class DropdownRoles
    {
        public async Task Setup(DiscordSocketClient client)
        {
            var cmdNewMenu = new SlashCommandBuilder();
            cmdNewMenu.WithName("newroledropdown");
            cmdNewMenu.WithDescription("Creates a new role drop down menu");
            cmdNewMenu.AddOption("role-list", ApplicationCommandOptionType.String, "Comma seperated values of role ids ex: 1234,5678");
            cmdNewMenu.AddOption("title", ApplicationCommandOptionType.String, "Title for the menu.");
            cmdNewMenu.AddOption("description", ApplicationCommandOptionType.String, "Description for the menu");
            cmdNewMenu.AddOption("channel", ApplicationCommandOptionType.Channel, "Channel to post in");
            cmdNewMenu.AddOption("min", ApplicationCommandOptionType.Integer, "Minimum number of roles they have to select.");
            cmdNewMenu.AddOption("max", ApplicationCommandOptionType.Integer, "Maximum number of roles they can select.");

            await client.Rest.CreateGuildCommand(cmdNewMenu.Build(), BotConfig.GetCachedConfig().ServerGuild);

        }

        public async Task OnNewDropdownRole(SocketSlashCommand cmd, string roleList, string title, string description, IGuildChannel chan, long min, long max)
        {
            var usr = cmd.User as SocketGuildUser;
            if (usr.GuildPermissions.Has(GuildPermission.Administrator))
            {
                if(roleList.Split(",").Length < 1)
                {
                    await cmd.RespondAsync(embed: QuickEmbeds.Error("Not a valid role list."));
                    return;
                }
                List<IRole> roles = new List<IRole>();
                foreach(var str in roleList.Split(","))
                {
                    if(chan.Guild.GetRole(Convert.ToUInt64(str)) != null) {
                        roles.Add(chan.Guild.GetRole(Convert.ToUInt64(str)));
                    }
                    else
                    {
                        await cmd.RespondAsync(embed: QuickEmbeds.Error("The role " + str + " does not map to a valid role in this server."));
                        return;
                    }
                }
                var comps = new ComponentBuilder();
                var options = new List<SelectMenuOption>();
                foreach(var role in roles)
                {
                    options.Add(new SelectMenuOptionBuilder().WithLabel(role.Name).WithValue(role.Id.ToString()).Build());
                }
            }
            else
            {
                await cmd.RespondAsync(embed: QuickEmbeds.PermissionError(), ephemeral: true);
                return;
            }
        }

        public async Task OnInteract(SocketInteraction inter)
        {
            if(inter is SocketSlashCommand cmd)
            {
                if(cmd.Data.Name == "newroledropdown")
                {
                    var options = new List<SocketSlashCommandDataOption>(cmd.Data.Options);
                    await OnNewDropdownRole(cmd, (string)options[0].Value, (string)options[1].Value, (string)options[2].Value, (IGuildChannel)options[3].Value, (long) options[4].Value, (long) options[5].Value);
                }
            }
        }
    }
}
