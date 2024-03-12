using _161Bot.SlashCommands;
using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;


namespace _161Bot.SlashCommands
{

    [ChaoCommand("randomdog", "See a random dog.")]
    public class RandomDog : ChaoSlashCommand
    {
        private class DogResponse
        {
            public string message { get; set; }
            public string status { get; set; }
        }
        public async Task Run(SocketSlashCommand cmd)
        {
            var response = await new ChaoWebRequest(BotConfig.GetCachedConfig().RandomDogUrl).ToClassFromJSON<DogResponse>();
            Console.WriteLine("Var is " + response.IsSuccess);
            if(response.IsSuccess)
            {
                var dogObject = response.Object;
                if(dogObject.status == "success")
                {
                    var embed = new EmbedBuilder();
                    embed.WithTitle("Random Dog");
                    embed.WithColor(Color.Gold);
                    embed.WithImageUrl(dogObject.message);
                    await cmd.RespondAsync(embed: embed.Build());
                }
                else
                {
                    await cmd.RespondAsync("Could not find a dog.", ephemeral: true);
                    return;
                }
            }
            else
            {
                await cmd.RespondAsync("Failed to make web request.", ephemeral: true);
                return;
            }
        }
    }
}
