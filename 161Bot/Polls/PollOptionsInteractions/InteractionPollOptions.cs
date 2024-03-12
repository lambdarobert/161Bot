using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;

namespace _161Bot.Polls
{
    public class InteractionPollOptions
    {
        public static string _id_interaction_toggle_results = "POLL_OPTIONS_TOGGLE_RESULTS";
        public static string _id_interaction_view_results = "POLL_OPTIONS_VIEW_RESULTS";
        public static string _id_interaction_toggle_lock = "POLL_OPTIONS_TOGGLE_LOCK";
        public static MessageComponent GenerateComps(Poll p)
        {
            var comps = new ComponentBuilder();
            if (p.ShowResults)
            {
                comps.WithButton("Hide Results", _id_interaction_toggle_results + p.Id, ButtonStyle.Danger);
            }
            else
            {
                comps.WithButton("Show Results", _id_interaction_toggle_results + p.Id, ButtonStyle.Primary);
            }
            comps.WithButton("View Results (override)", _id_interaction_view_results + p.Id, ButtonStyle.Primary);
            if (p.PollLocked)
            {
                comps.WithButton("Unlock Poll", _id_interaction_toggle_lock + p.Id, ButtonStyle.Primary);
            }
            else
            {
                comps.WithButton("Lock Poll", _id_interaction_toggle_lock + p.Id, ButtonStyle.Danger);
            }
            var finalComps = comps.Build();
            return finalComps;
        }

        public async Task Run(SocketMessageComponent smc)
        {
            var usr = smc.User as SocketGuildUser;
            if(usr.GuildPermissions.Has(GuildPermission.Administrator))
            {
                var poll = PollManager.Instance.FindPoll(smc.Message.Id);
                if(poll != null)
                {
                    var ketamine = GenerateComps(poll);
                    await smc.RespondAsync(embed: new EmbedBuilder().
                        WithDescription("Options: \n Hide/show results: Toggle the visibility of the poll results \n View results (override): See the results of the poll (use this button if results are hidden, only you and other mods can see them) \n Lock/unlock Poll: Toggle if new votes can be added.")
                        .WithTitle("Options").WithColor(Color.Blue).Build(), ephemeral: true, components: ketamine); 
                }
                else
                {
                    await smc.RespondAsync(embed: QuickEmbeds.Error("Poll no longer exists."), ephemeral: true);
                    return;
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
