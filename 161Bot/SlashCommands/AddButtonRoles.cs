using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{
    [ChaoCommand("addbuttonroles", "Add button role selector for the channel")]
    [ChaoPermissions(new ulong[] {}, 796444864177766400)]
    public class AddButtonRoles : ChaoSlashCommand
    {

        //example syntax for payload: Role_1=1234,Role_2=4567
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("payload", "example syntax for payload: Role_1=1234=emoteid,Role_2=4567=emoteid")] string payload, [ChaoParameter("description", "Goes along with the message.")] string description)
        {
            var guild = (cmd.Channel as SocketGuildChannel).Guild;
            var components = new Discord.ComponentBuilder();
            string[] roles = payload.Split(",");
            Console.WriteLine(payload);
            foreach (string role in roles)
            {
                if (role.Split("=").Length != 2 && role.Split("=").Length != 3)
                {
                    await cmd.RespondAsync("Invalid role button, " + role + ".", ephemeral: true);
                    return;
                }
                string key = role.Split("=")[0].Replace("_", " ");
                string value = role.Split("=")[1];
                if (role.Split("=").Length == 3)
                {
                    Emote emotee = null;
                    foreach(Emote e in guild.Emotes)
                    {
                        if (e.Name == role.Split("=")[2]) {
                            emotee = e;
                        }
                    }
                    components.WithButton(key, "BR_" + value, ButtonStyle.Primary, emote: emotee);
                }
                else
                {
                    components.WithButton(key, "BR_" + value); // BR_ identifies this interaction as a button role specifically
                }
            }

            await cmd.Channel.SendMessageAsync(description, components: components.Build());
            await cmd.RespondAsync("Posted!", ephemeral: true);
        }
    }
}
