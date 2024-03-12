using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace _161Bot.Polls
{
    public class CommandNewPoll
    {
        public async Task Run(SocketSlashCommand cmd, string title, string description, string optionData, bool showResults, IGuildChannel chan, int minOptions, int maxOptions)
        {
            //begin sanity checking
            if(!(cmd.User as SocketGuildUser).GuildPermissions.Has(GuildPermission.Administrator))
            {
                await cmd.RespondAsync(embed: QuickEmbeds.PermissionError());
            }
            if(optionData.Split(",").Length == 0)
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("Not a comma seperated list."));
                return;
            }
            if(minOptions < 1 || minOptions > 10)
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("You can only have up to ten options."));
                return;
            }
            if (maxOptions < 1 || maxOptions > 10)
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("You can only have up to ten options."));
                return;
            }

            Poll p = new Poll();
            p.Id = Guid.NewGuid().ToString();
            p.Title = title;
            p.Description = description;
            p.OptionData = new List<string>(optionData.Split(","));
            p.ShowResults = showResults;
            p.MinOptions = minOptions;
            p.MaxOptions = maxOptions;
            p.PollLocked = false;
            p.VotingData = new Dictionary<ulong, List<string>>();
        
            if(chan is SocketTextChannel gchan)
            {
                var comp = new ComponentBuilder();
                List<SelectMenuOptionBuilder> selectOptions = new List<SelectMenuOptionBuilder>();
                for(int i = 0; i < p.OptionData.Count; i++)
                {
                    selectOptions.Add(new SelectMenuOptionBuilder().WithValue(i.ToString()).WithLabel(p.OptionData[i]));
                }
                comp.WithSelectMenu("Select option", PollManager.Instance._id_prefix + p.Id, selectOptions, minValues: p.MinOptions, maxValues: p.MaxOptions);

                comp.WithButton("View Results", PollManager.Instance._id_poll_results_ddm, ButtonStyle.Primary, new Emoji("✅"));
                comp.WithButton("Poll Options", PollManager.Instance._id_poll_options_ddm_option, ButtonStyle.Secondary, new Emoji("⚙️"));

                try
                {
                    var msg = await gchan.SendMessageAsync(embed: PollManager.Instance.GenerateEmbed(p), component: comp.Build());
                    p.MessageId = msg.Id;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    await cmd.RespondAsync(embed: QuickEmbeds.Error("Something didn't work."));
                    return;
                }
                
                await cmd.RespondAsync("Poll posted!");
                var cfg = BotConfig.GetCachedConfig();
                cfg.Poll.Add(p);
                BotConfig.SaveConfig(cfg);
            }
            else
            {
                await cmd.RespondAsync(embed: QuickEmbeds.Error("Channel needs to be a guild/server text channel."));
                return;
            }
        }
    }
}
