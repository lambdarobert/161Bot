using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;

namespace _161Bot.Polls.PollOptionsInteractions
{
    public class InteractionViewResultsOverride
    {
        public async Task Run(SocketMessageComponent smc)
        {
            var user = smc.User as SocketGuildUser;
            if (user.GuildPermissions.Has(GuildPermission.Administrator))
            {
                var poll = PollManager.Instance.FindPollWithId(smc.Data.CustomId.Replace(InteractionPollOptions._id_interaction_view_results, ""));
                if (poll != null)
                {
                    await new InteractionVoteResults().Run(smc, overrideRestrictions: true);
                }
                else
                {
                    await smc.RespondAsync(embed: QuickEmbeds.Error("Poll no longer exists"), ephemeral: true);
                }
            }
            else
            {
                await smc.RespondAsync(embed: QuickEmbeds.PermissionError(), ephemeral: true);
                return;
            }
        }
    }
}
