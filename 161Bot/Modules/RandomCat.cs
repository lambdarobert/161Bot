using Discord;
using Discord.Commands;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace _161Bot.Commands
{
    public class RandomCat : ModuleBase<SocketCommandContext>
    {
        private class Category
        {
            public int id { get; set; }
            public string name { get; set; }
        }

        private class CatResponse
        {
            public List<object> breeds { get; set; }
            public List<Category> categories { get; set; }
            public string id { get; set; }
            public string url { get; set; }
            public int width { get; set; }
            public int height { get; set; }
        }

        [Command("randomcat", RunMode = RunMode.Async)]
        [Alias("randcat", "randomcatto")]
        [Summary("See a random cat.")]
        public async Task Run()
        {
            Console.WriteLine("run");
            var response = await new ChaoWebRequest("https://api.thecatapi.com/v1/images/search?limit=1&size=full").ToClassFromJSON<CatResponse>();
            Console.WriteLine("rundi");
            if (response.IsSuccess)
            {
                Console.WriteLine("yes");
                var catObject = response.Object;
                var embed = new EmbedBuilder();
                embed.WithTitle("Random Cat!");
                embed.WithThumbnailUrl(catObject.url);
                await ReplyAsync(embed: embed.Build());
            }
            else
            {
                await ReplyAsync("It didn't work.");
            }
        }
    }
}
