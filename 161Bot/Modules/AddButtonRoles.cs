using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.Modules
{
    public class AddButtonRoles : ModuleBase<SocketCommandContext>
    {

        [Command("addbuttonroles")]
        [Summary("Admin command")]
        [RequireUserPermission(Discord.GuildPermission.Administrator)]
        //example syntax for payload: Role_1=1234,Role_2=4567
        public async Task Run(string payload, [Remainder] string description)
        {
            var components = new Discord.ComponentBuilder();
            string[] roles = payload.Split(",");
            Console.WriteLine(payload);
            foreach (string role in roles)
            {
                if (role.Split("=").Length != 2 && role.Split("=").Length != 3)
                {
                    await Context.Channel.SendMessageAsync("Invalid role button, " + role + ".");
                    return;
                }
                string key = role.Split("=")[0].Replace("_", " ");
                string value = role.Split("=")[1];
                if (role.Split("=").Length == 3)
                {
                    Emote emotee = null;
                    foreach(Emote e in Context.Guild.Emotes)
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

            await Context.Channel.SendMessageAsync(description, component: components.Build());
        }
    }
}
