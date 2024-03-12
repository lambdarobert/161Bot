using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using System.Threading.Tasks;

namespace _161Bot.Polls.PollOptionsInteractions
{
    public class InteractionToggleLock
    {
        public async Task Run(SocketMessageComponent smc)
        {
            var user = smc.User as SocketGuildUser;
            if (user.GuildPermissions.Has(GuildPermission.Administrator))
            {
                var poll = PollManager.Instance.FindPollWithId(smc.Data.CustomId.Replace(InteractionPollOptions._id_interaction_toggle_lock, ""));
                if (poll != null)
                {
                    poll.PollLocked = !poll.PollLocked;
                    BotConfig.SaveConfig(BotConfig.GetCachedConfig());
                    var newcomps = InteractionPollOptions.GenerateComps(poll);
                    await smc.UpdateAsync(m =>
                    {
                        m.Components = newcomps;
                    });
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
