using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace _161Bot.Polls
{
    public class InteractionVotingMenu
    {
        public async Task Run(SocketMessageComponent smc)
        {
            var poll = PollManager.Instance.FindPoll(smc.Message.Id);
            if(poll != null)
            {
                if(poll.PollLocked)
                {
                    await smc.RespondAsync(embed: QuickEmbeds.Error("This poll is currently locked, and the votes cannot be changed."), ephemeral: true);
                }
                poll.VotingData[smc.User.Id] = new List<string>();
                foreach(var option in smc.Data.Values)
                {
                    poll.VotingData[smc.User.Id].Add(option);
                }

                BotConfig.SaveConfig(BotConfig.GetCachedConfig());
                await smc.RespondAsync(embed: new EmbedBuilder()
                  .WithTitle("Vote Cast")
                  .WithDescription("Your vote has been cast.")
                  .WithColor(Color.Green).Build(), ephemeral: true);

                var embeds = new List<Embed>(smc.Message.Embeds);
                var oldEmbed = embeds[0].ToEmbedBuilder();
                await smc.Message.ModifyAsync(m =>
                {
                    oldEmbed.WithFooter("Voted so far: " + poll.VotingData.Count);
                    m.Embed = oldEmbed.Build();
                });
            }
            else
            {
                await smc.RespondAsync(embed: QuickEmbeds.Error("This poll no longer exists."), ephemeral: true);
            }
        }
    }
}
