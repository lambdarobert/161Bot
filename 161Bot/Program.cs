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
                Console.WriteLine("Welcome to ChaoBot. It seems you don't have a configuration file yet. I will write a template in config.yml. Please edit it to your liking.");
                BotConfig template = new BotConfig();
                template.BotPrefix = "!";
                template.DiscordToken = "1234";
                template.PraiseCount = "0";
                template.RandomDogUrl = "https://example.com";
                template.ChaoPrayThumbnail = "https://example.com";
                template.ServerGuild = 1234;
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
            await _client.SetGameAsync("!chaocmds for commands!");
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

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
        }

        private async Task HandleCommandsAysnc(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            var context = new SocketCommandContext(_client, message);
            if (message.Author.IsBot) return;
            int argPos = 0;
            if (message.Content.ToLower().Contains(" chat") || message.Content.ToLower().Contains("chat "))
            {
                await message.Channel.SendMessageAsync("NO CHAT");
            }
            if (message.HasStringPrefix("!", ref argPos))
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
