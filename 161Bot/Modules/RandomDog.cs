using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;

namespace _161Bot.Commands
{
    public class RandomDog : ModuleBase<SocketCommandContext>
    {
        private class DogResponse
        {
            public string message { get; set; }
            public string status { get; set; }
        }

        [Command("randomdog", RunMode = RunMode.Async)]
        [Alias("randdog", "randomdoggo")]
        [Summary("See a random dog.")]
        public async Task Run()
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
                    await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), embed: embed.Build());
                }
                else
                {
                    await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), message: "Could not find a dog.");
                    return;
                }
            }
            else
            {
                await ReplyAsync(messageReference: new MessageReference(Context.Message.Id), message: "Failed to make web request.");
                return;
            }
        }
    }
}
