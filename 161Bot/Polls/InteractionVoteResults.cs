using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;

namespace _161Bot.Polls
{
    public class InteractionVoteResults
    {
        public async Task Run(SocketMessageComponent smc, bool overrideRestrictions = false)
        {
            Poll poll;
            if(overrideRestrictions)
            {
                var id = smc.Data.CustomId.Replace(InteractionPollOptions._id_interaction_view_results, "");
                poll = PollManager.Instance.FindPollWithId(id);
            }
            else
            {
                poll = PollManager.Instance.FindPoll(smc.Message.Id);
            }
            
            if (poll != null)
            {
                if (poll.ShowResults || overrideRestrictions)
                {
                    // KEY: poll option
                    // VALUE: Frequency
                    StringBuilder userData = new StringBuilder();
                    Dictionary<int, int> frequency = new Dictionary<int, int>();
                    var rawData = poll.VotingData;
                    for(int i = 0; i < poll.OptionData.Count; i++)
                    {
                        frequency[i] = 0; // initialize everything
                    }
                    foreach (KeyValuePair<ulong, List<string>> votes in poll.VotingData)
                    {
                        foreach(var option in votes.Value)
                        {
                            frequency[Convert.ToInt32(option)]++;
                        }
                    }

                    foreach (KeyValuePair<int, int> kv in frequency)
                    {
                        userData.Append("**" + poll.OptionData[kv.Key] + "**: " + kv.Value + "\n");
                    }

                    await smc.RespondAsync(embed: new EmbedBuilder()
                        .WithTitle("Poll Results")
                        .WithColor(Color.Blue)
                        .WithDescription(userData.ToString())
                        .Build()
                        , ephemeral: true); 
               
                }
                else
                {
                    await smc.RespondAsync(embed: QuickEmbeds.Error("The poll creator hasn't opened the results yet. Check back later."), ephemeral: true);
                    return;
                }
            }
            else
            {
                await smc.RespondAsync(embed: QuickEmbeds.Error("Poll no longer exists."), ephemeral: true);
                return;
            }
        }
    }
}
