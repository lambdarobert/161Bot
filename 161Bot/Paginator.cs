using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{

    public static class Paginator
    {
        internal class PaginationData
        {
            public int CurrentPage { get; set; }
            public string[] Data { get; set; }
            public Embed Payload { get; set; }
        }

        private static Dictionary<ulong, PaginationData> paginationData = new Dictionary<ulong, PaginationData>();
        private static readonly string btnForwardId = "PAGINATORFORWARD";
        private static readonly string btnBackId = "PAGINATORBACK";


        /*
         * Responds with pagination, and uses the embed payload as a template and data representing the contents for each page.
         * The contents in the description and footer of the embed payload are ignored, as that is provided in the "data" string.
         * Footer is used to display the current page. 
         */
        public static async Task RespondWithPagination(SocketInteraction inter, Embed payload, string[] data, bool ephemeralResponse = true)
        {
            if (data.Length > 100)
            {
                throw new ArgumentException("Maximum 100 pages.");
            }
           
            var comps = new ComponentBuilder();
            if(data.Length > 1)
            {
                comps.WithButton("Next Page",  customId: btnForwardId + inter.Id, ButtonStyle.Primary);
            }
            var final_embed = payload.ToEmbedBuilder().WithDescription(data[0]).WithFooter("Page 1 of " + data.Length);
            await inter.RespondAsync(embed: final_embed.Build(), ephemeral: ephemeralResponse, component: comps.Build());
            paginationData.Add(inter.Id, new PaginationData
            {
                CurrentPage = 0,
                Data = data,
                Payload = payload
            });
        }

        //handles presses of the buttons
        public static async Task OnInteract(SocketInteraction inter)
        {
            if(inter is SocketMessageComponent smc)
            {
                if(smc.Data.CustomId.StartsWith(btnForwardId) || smc.Data.CustomId.StartsWith(btnBackId))
                {
                    ulong id = Convert.ToUInt64(smc.Data.CustomId.Replace(btnForwardId, "").Replace(btnBackId, ""));
                    if (paginationData.ContainsKey(id))
                    {
                        var dat = paginationData[id];
                        if(smc.Data.CustomId.StartsWith(btnForwardId))
                        {
                            dat.CurrentPage++;
                        }
                        else
                        {
                            dat.CurrentPage--;
                        }
                        // update embed to contain this page.
                        var comps = new ComponentBuilder();
                        if(dat.CurrentPage < dat.Data.Length - 1)
                        {
                            comps.WithButton("Next Page", customId: btnForwardId + id, ButtonStyle.Primary);
                        }
                        if (dat.CurrentPage > 0)
                        {
                            comps.WithButton("Previous Page", customId: btnBackId + id, ButtonStyle.Primary);
                            
                        }
                        await smc.UpdateAsync(m =>
                        {
                            var embed = smc.Message.Embeds.First().ToEmbedBuilder();
                            embed.WithDescription(dat.Data[dat.CurrentPage]);
                            embed.WithFooter("Page " + (dat.CurrentPage + 1) + " of " + dat.Data.Length);
                            m.Embed = Optional.Create(embed.Build());
                            m.Components = Optional.Create(comps.Build());
                        });
                    }
                    else
                    {
                        await smc.RespondAsync(embed: QuickEmbeds.Error("This paginator has expired. Please run the command again."), ephemeral: true);
                        return;
                    }
                }
                
            }
        }

        /*
         * Responds with pagination, automatically splitting the description into different pages
         * 
         */
        public static async Task RespondWithPagination(SocketInteraction inter, Embed payload, int characters, bool ephemeralResponse = true)
        {
            if(characters <= 10 || characters > 1024)
            {
                throw new ArgumentException("Characters cannot be less than ten or greater than 1024.");
            }
            List<string> strings = new List<string>();
            for (int i = 0; i < payload.Description.Length; i += characters)
            {
                strings.Add(payload.Description.Substring(i, Math.Min(characters, payload.Description.Length - 1)));
            }
            await RespondWithPagination(inter, payload, strings.ToArray(), ephemeralResponse);
        }
    }
}
