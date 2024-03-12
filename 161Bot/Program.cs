using System;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;

using System.Threading.Tasks;
using System.Collections.Generic;
using System.Reflection;
using System.IO;

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
                VCHandlerConfig vcConfig = new VCHandlerConfig();
                vcConfig.ChannelId = 1234;
                vcConfig.RoleId = 1234;
                template.VcConfig = vcConfig;
                BotConfig.SaveConfig(template);
                System.Environment.Exit(0);
            }
            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;

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
            await _client.SetGameAsync("Written in C#!");
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
            if(message.HasStringPrefix("!", ref argPos))
            {
                var result = await _commands.ExecuteAsync(context, argPos, _services);
                if(!result.IsSuccess)
                {
                    Console.WriteLine(result.ErrorReason);

                }
            }
            if(message.ToString().ToLower().Contains("chao"))
            {
                await message.AddReactionAsync(new Emoji("🙏"));
            }

        }
    }
}
