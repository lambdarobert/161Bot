using _161Bot.SlashCommands.Injection;
using _161Bot.SlashCommands.Injection.Attributes;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot.SlashCommands
{

    [ChaoCommand("bingo", "Create a new bingo.")]
    public class Bingo : ChaoSlashCommand
    {
        public async Task Run(SocketSlashCommand cmd, [ChaoParameter("title", "The title of this bingo set")] string title,
            [ChaoParameter("row1", "Enter FIVE comma seperated values. Ex: option1,option2,option3")] string row1,
            [ChaoParameter("row2", "Enter FIVE comma seperated values.")] string row2,
            [ChaoParameter("row3", "Enter FOUR comma seperated values.")] string row3,
            [ChaoParameter("row4", "Enter FOUR comma seperated values.")] string row4,
            [ChaoParameter("row5", "Enter FIVE comma seperated values.")] string row5
            )
        {
            var row1list = new List<string>(row1.Split(","));
            var row2list = new List<string>(row2.Split(","));
            var row3list = new List<string>(row3.Split(","));
            var row4list = new List<string>(row4.Split(","));
            var row5list = new List<string>(row5.Split(","));

            row3list.Insert(2, "Free Space");

            if (row1list.Count != 5 || row2list.Count != 5 || row3list.Count != 5 || row4list.Count != 5 || row5list.Count != 5)
            {
                await cmd.RespondAsync("You need to have five elements per row, except for the third row where you need four (to include free space). Row sizes are: Row1: " + row1list.Count() +  " | Row2: " + row2list.Count() + " | Row3: " + row3list.Count() + " | Row4 (including free space): " + row4list.Count() + " | Row 5: " + row5list.Count(), ephemeral: true);
                return;
            }
            if(title.Length >= 75)
            {
                await cmd.RespondAsync("Title needs to be 75 characters or less and alphanumeric.", ephemeral: true);
                return;
            }
            var comps = new Discord.ComponentBuilder();


            int i = 0;
            foreach (var str in row1list) {
                if(str.Length >= 75 )
                {
                   
                    await cmd.RespondAsync("All buttons needs to be 75 alphanumeric characters or less. Please check '" + str + "'", ephemeral: true);
                    return;
                }
                else
                {
                    i++;
                    comps.WithButton(label: str, customId: "BINGOBUTTON" + i, style: Discord.ButtonStyle.Secondary, emote: null, url: null, disabled: false, row: 0);
                }
            }

            foreach (var str in row2list)
            {
                if (str.Length >= 75 )
                {
                   
                    await cmd.RespondAsync("All buttons needs to be 75 characters or less. Please check '" + str + "'", ephemeral: true);
                    return;
                }
                else
                {
                    i++;
                    comps.WithButton(label: str, customId: "BINGOBUTTON" + i, style: Discord.ButtonStyle.Secondary, emote: null, url: null, disabled: false, row: 1);
                }
            }

            foreach (var str in row3list)
            {
                if (str.Length >= 75 )
                {
           
                    await cmd.RespondAsync("All buttons needs to be 75 characters or less. Please check '" + str + "'", ephemeral: true);
                    return;
                }
                else
                {
                    i++;
                    comps.WithButton(label: str, customId: "BINGOBUTTON" + i, style: Discord.ButtonStyle.Secondary, emote: null, url: null, disabled: false, row: 2);
                }
            }

            foreach (var str in row4list)
            {
                if (str.Length >= 75)
                {
               
                    await cmd.RespondAsync("All buttons needs to be 75 characters or less. Please check '" + str + "'", ephemeral: true);
                    return;
                }
                else
                {
                    i++;
                    comps.WithButton(label: str, customId: "BINGOBUTTON" + i, style: Discord.ButtonStyle.Secondary, emote: null, url: null, disabled: false, row: 3);
                }
            }

            foreach (var str in row5list)
            {
                if (str.Length >= 75 )
                {
               
                    await cmd.RespondAsync("All buttons needs to be 75 characters or less. Please check '" + str + "'", ephemeral: true);
                    return;
                }
                else
                {
                    i++;

                    comps.WithButton(label: str, customId: "BINGOBUTTON" + i, style: Discord.ButtonStyle.Secondary, emote: null, url: null, disabled: false, row: 4);
                }
            }



            try
            {
                await cmd.Channel.SendMessageAsync("**BINGO:** " + title + " by " + cmd.User.Mention, components: comps.Build());
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }



            await cmd.RespondAsync("Your bingo has been created.", ephemeral: true);

            


        }
    }
}
