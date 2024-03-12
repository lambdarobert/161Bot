
using _161Bot.Polls;
using _161Bot.SlashCommands;
using _161Bot.SlashCommands.Injection;
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

        public static DiscordSocketClient _client;
        private static CommandService _commands;
        private static IServiceProvider _services;
        private static ChaoCommandManager _slash_commands = new ChaoCommandManager();

        public static IServiceProvider GetServices()
        {
            return _services;
        }

        public async Task RunBotAsync()
        {
            _client = new DiscordSocketClient(new DiscordSocketConfig());
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
            await _client.SetGameAsync("with Winnie the Pooh");
            
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
            _client.MessageReceived += new StatsDataUpdater().Handle;
            _client.UserVoiceStateUpdated += new VCHandler().HandleVC;
            _client.UserVoiceStateUpdated += new VCChannelManager().OnChannelJoinLeave;
            _client.ChannelDestroyed += new VCChannelManager().OnVoiceChannelDestroyed;
            _client.ChannelUpdated += new VCChannelManager().OnChannelModified;
            _client.InteractionCreated += HandleInteractions;
            _client.InteractionCreated += Quote.OnInteraction;
            _client.InteractionCreated += Paginator.OnInteract;
            _client.InteractionCreated += new BingoButtons().HandleInteractions;
            _client.InteractionCreated += async (inter) =>
            {
                try
                {
                    await _slash_commands.Run(inter);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            };


            _client.Ready += async delegate
            {
                Console.WriteLine("running ready");

                /*
                var chan = _client.GetChannel(795714783801245709) as IMessageChannel;
                var embed = new EmbedBuilder();
                embed.WithTitle("K747 Survivors Rules");
                string rulesDesc = File.ReadAllText("rules.txt");
                embed.WithDescription(rulesDesc);
                embed.WithColor(Color.Blue);
                embed.WithFooter("If you see something being broken, please talk to a mod about it. Don't publicly accuse people in the chats.");

                await chan.SendMessageAsync(embed: embed.Build());
                */

                Task.Run(async delegate
                {
                    await Quote.GenerateQuotes(_client);
                    await _slash_commands.Setup(_client);
                });
                await (_client.GetChannel(831964996287332352) as IMessageChannel).SendMessageAsync("Started.");
                await PollManager.Instance.Setup(_client);


            };

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleInteractions(SocketInteraction inter)
        {

            if (inter is SocketMessageComponent smc)
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
            if(arg == null || message == null)
            {
                return;
            }
            if (message.Author.IsBot) return;

            /*
            if (message.ToString().ToLower().Contains("chao"))
            {
                await message.AddReactionAsync(new Emoji("🙏"));
            }
            */
            if (message.ToString().ToLower().Contains("kishore"))
            {
                await message.AddReactionAsync(Emote.Parse("<:747:805867124593393675>"));
            }
            if(message.ToString().ToLower().Contains("memory leak"))
            {
                await message.Channel.SendMessageAsync("https://tenor.com/view/minion-memory-alert-gif-9313925", messageReference: new MessageReference(message.Id));
            }
            await new BotSpeakHandler().HandleMessage(arg);

        }
    }
}
