using _161Bot.Modules;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace _161Bot
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                File.ReadAllText("config.yml");
            }
            catch (Exception)
            {
                Console.WriteLine("Welcome to CHAOBOT. It seems you don't have a configuration file yet. I will write a template in config.yml. Please edit it to your liking.");
                BotConfig template = new BotConfig();
                template.BotPrefix = "!";
                template.DiscordToken = "1234";
                template.PraiseCount = "0";
                template.RandomDogUrl = "https://example.com";
                template.ChaoPrayThumbnail = "https://example.com";
                template.ServerGuild = 1234;
                template.VCManagerMessagedUsers = new List<ulong>() { 1234 };
                template.BabyYodaUrls = new List<string>()
                {
                    "https://example.com/url1",
                    "https://example.com/url2",
                    "https://example.com/url3"
                };
                VCHandlerConfig vcConfig = new VCHandlerConfig();
                vcConfig.ChannelId = 1234;
                vcConfig.RoleId = 1234;
                template.VcConfig = vcConfig;
                BotConfig.SaveConfig(template);
                System.Environment.Exit(0);
            }
            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        private static DiscordSocketClient _client;
        private static CommandService _commands;
        private static IServiceProvider _services;

        public static IServiceProvider GetServices()
        {
            return _services;
        }

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _services = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = BotConfig.GetCachedConfig().DiscordToken;

            _client.Log += _client_Log;
            await RegisterCommandsAsync();
            await _client.LoginAsync(TokenType.Bot, botToken);
            await _client.StartAsync();
            await _client.SetGameAsync("::cmds for commands!");
            await Task.Run(async delegate
            {
                await Task.Delay(60000);
                await Quote.GenerateQuotes(_client);
            });
            await Task.Delay(-1);


        }

        private Task _client_Log(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync()
        {
            _client.MessageReceived += HandleCommandsAysnc;
            _client.UserVoiceStateUpdated += new VCHandler().HandleVC;
            _client.UserVoiceStateUpdated += new VCChannelManager().OnChannelJoinLeave;
            _client.ChannelDestroyed += new VCChannelManager().OnVoiceChannelDestroyed;
            _client.ChannelUpdated += new VCChannelManager().OnChannelModified;
            _client.MessageReceived += new Greentext().OnMessage;
            _client.InteractionCreated += HandleInteractions;
  
            _client.Ready += async delegate
            {
                await (_client.GetChannel(831964996287332352) as IMessageChannel).SendMessageAsync("Started.");
                await Task.Run(async delegate
                {
                    await Quote.GenerateQuotes(_client);
                });

              
            };

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleInteractions(SocketInteraction inter)
        {
            if(inter is SocketMessageComponent smc)
            {
                if (smc.Data.CustomId.StartsWith("BR_"))
                {
                    var role = Convert.ToUInt64(smc.Data.CustomId.Replace("BR_", ""));
                    var usr = (SocketGuildUser)smc.User;
                    var guild = usr.Guild;

                    bool hasRole = false;
                    foreach (SocketRole r in usr.Roles)
                    {
                        if (r.Id == Convert.ToUInt64(role))
                        {
                            hasRole = true;
                            break;
                        }
                    }

                    if (guild.GetRole(role) != null)
                    {
                        var guildRole = guild.GetRole(role);
                        if (guildRole.Permissions.Has(GuildPermission.Administrator) || guildRole.Permissions.Has(GuildPermission.ManageGuild) || guildRole.Permissions.Has(GuildPermission.ManageRoles) || guildRole.Permissions.Has(GuildPermission.BanMembers) || guildRole.Permissions.Has(GuildPermission.KickMembers))
                        {
                            await inter.RespondAsync(embed: QuickEmbeds.Error("Sorry, this role cannot be granted."), ephemeral: true);
                        }
                        else
                        {
                            if (hasRole)
                            {
                                await usr.RemoveRoleAsync(guildRole);
                                var embed = new EmbedBuilder();
                                embed.WithTitle("Role Removed");
                                embed.WithDescription("The role '" + guildRole.Name + "' has been removed!");
                                embed.WithColor(Color.Red);
                                await inter.RespondAsync(embed: embed.Build(), ephemeral: true);
                            }
                            else
                            {
                                await usr.AddRoleAsync(guildRole);
                                var embed = new EmbedBuilder();
                                embed.WithTitle("Role Added");
                                embed.WithDescription("The role '" + guildRole.Name + "' has been added!");
                                embed.WithColor(Color.Green);
                                await inter.RespondAsync(embed: embed.Build(), ephemeral: true);
                            }
                        }


                    }
                    else
                    {
                        await inter.RespondAsync(embed: QuickEmbeds.Error("This role does not exist anymore."), ephemeral: true);
                    }
                }
            }
                
        }

        private async Task HandleCommandsAysnc(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;
            int argPos = 0;
            /*
                         if (message.Content.ToLower().Contains(" chat") || message.Content.ToLower().Contains("chat "))
            {
                await message.Channel.SendMessageAsync("NO CHAT");
            }   
             */
            if (message.HasStringPrefix("::", ref argPos))
            {
                if (!(message.Channel is IGuildChannel))
                {
                    await message.Channel.SendMessageAsync(embed: QuickEmbeds.Error("For privacy reasons, this command does not work in this context."));
                    return;
                }
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if (!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);

                }
            }
            if (message.ToString().ToLower().Contains("chao"))
            {
                await message.AddReactionAsync(new Emoji("🙏"));
            }
            if (message.ToString().ToLower().Contains("kishore"))
            {
                await message.AddReactionAsync(Emote.Parse("<:747:805867124593393675>"));
            }
            await new BotSpeakHandler().HandleMessage(arg);

        }
    }
}
