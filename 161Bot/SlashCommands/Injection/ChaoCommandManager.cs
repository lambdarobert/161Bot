using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands.Injection
{
    public class ChaoCommandManager
    {
        private List<ChaoSlashCommand> cmds = new List<ChaoSlashCommand>();

        private ApplicationCommandOptionType TranslateType(Type t)
        {
            if(t == typeof(string))
            {
                return ApplicationCommandOptionType.String;
            }
            if(t == typeof(long))
            {
                return ApplicationCommandOptionType.Integer;
            }
            if(t == typeof(bool))
            {
                return ApplicationCommandOptionType.Boolean;
            }
            if(t == typeof(IGuildUser))
            {
                return ApplicationCommandOptionType.User;
            }
            if(t == typeof(IGuildChannel))
            {
                return ApplicationCommandOptionType.Channel;
            }
            if(t == typeof(double))
            {
                return ApplicationCommandOptionType.Number;
            }
            return ApplicationCommandOptionType.String;
        }
        
        public async Task Setup(DiscordSocketClient cli)
        {
            //pull each and every Chao Slash Command
            foreach(var type in Assembly.GetEntryAssembly().GetTypes())
            {
                if(type.BaseType == typeof(ChaoSlashCommand) && type.GetConstructor(Type.EmptyTypes) != null)
                {
                    cmds.Add((ChaoSlashCommand) Activator.CreateInstance(type));
                }
            }

            foreach(var cmd in cmds)
            {
                var type = cmd.GetType();
                var builder = new SlashCommandBuilder();
                var attributes = Attribute.GetCustomAttributes(type).Where(a => a is ChaoCommandAttribute);
                var perms = Attribute.GetCustomAttributes(type).Where(a => a is ChaoPermissionsAttribute);
                if(attributes.Count() == 1)
                {
                    var attribute = (ChaoCommandAttribute) attributes.First();
                    builder.WithName(attribute.Name).WithDescription(attribute.Description);
                }
                var method = type.GetMethods().Where(m => m.Name == "Run").First();
                
                foreach (var param in method.GetParameters())
                {
                    if(param.GetCustomAttributes().Count() == 1)
                    {
                        var optionBuilder = new SlashCommandOptionBuilder();
                        optionBuilder.WithType(TranslateType(param.ParameterType));
                        var optionAttribute = param.GetCustomAttributes().Where(a => a is ChaoParameterAttribute).First() as ChaoParameterAttribute;
                        optionBuilder.Required = optionAttribute.Required;
                        optionBuilder.WithName(optionAttribute.Name);
                        optionBuilder.WithDescription(optionAttribute.Description);
                        builder.AddOption(optionBuilder);
                    }
                }
                if(perms.Count() == 1)
                {
                    builder.WithDefaultPermission(false);
                }
                var slashCmd = await cli.Rest.CreateGuildCommand(builder.Build(), BotConfig.GetCachedConfig().ServerGuild);
                if(perms.Count() == 1)
                {
                    var perm = perms.First() as ChaoPermissionsAttribute;
                    Console.WriteLine("adding permissions");
                    await slashCmd.ModifyCommandPermissions(perm.Perms.ToArray());
                }
            }
        }

        public async Task Run(SocketInteraction inter)
        {
            if(inter is SocketSlashCommand cmd)
            {
                var chaoCmd = cmds.Where(c => (c.GetType().GetCustomAttributes().Where(a => a is ChaoCommandAttribute).FirstOrDefault() as ChaoCommandAttribute).Name == cmd.Data.Name).FirstOrDefault();
                var param = new List<object>();
                param.Add(cmd);
                if(cmd.Data.Options != null)
                {
                    foreach (var p in cmd.Data.Options)
                    {
                        param.Add(p.Value);
                    }
                }
                //finally, run the command
                await Task.Run(() =>
                {
                    chaoCmd.GetType().GetMethod("Run").Invoke(chaoCmd, param.ToArray());
                });
                
            }
        }
    }
}
