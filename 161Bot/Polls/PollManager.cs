using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using _161Bot.Polls.PollOptionsInteractions;
using Discord;
using Discord.WebSocket;

namespace _161Bot.Polls
{
    public class PollManager
    {
        private static readonly PollManager _instance = new PollManager();
        private readonly ulong _guild = 795714783801245706;
        public readonly string _id_prefix = "POLLDDM_";
        public readonly string _id_poll_options_ddm_option = "POLLDDMPOLL_OPTIONS";
        public readonly string _id_poll_results_ddm = "POLLDDMPOLL_RESULTS";

        private PollManager()
        {

        }

        public static PollManager Instance
        {
            get
            {
                return _instance;
            }
        }

        public Poll? FindPoll(ulong msgId)
        {
            var list = BotConfig.GetCachedConfig().Poll;
            foreach(var poll in list)
            {
                if(poll.MessageId == msgId)
                {
                    return poll;
                }
            }
            return null;
        }

        public Poll? FindPollWithId(string pollId)
        {
            var list = BotConfig.GetCachedConfig().Poll;
            foreach (var poll in list)
            {
                if (poll.Id == pollId)
                {
                    return poll;
                }
            }
            return null;
        }

        private async Task OnInteraction(SocketInteraction inter)
        {
            if(inter is SocketSlashCommand cmd)
            {
                if(cmd.Data.Name == "newpoll")
                {

                    List<SocketSlashCommandDataOption> ops = new List<SocketSlashCommandDataOption>(cmd.Data.Options);
                    await new CommandNewPoll().Run(cmd, (string)ops[0].Value, (string)ops[1].Value, (string)ops[2].Value, (bool)ops[3].Value, (IGuildChannel)ops[4].Value, (int)ops[5].Value, (int) ops[6].Value);
                }
            }
            else
            {
                if(inter is SocketMessageComponent smc)
                {
                    if(smc.Data.CustomId.StartsWith(_id_prefix))
                    {
                        // drop down menu was clicked
                        await new InteractionVotingMenu().Run(smc);
                    }
                    if(smc.Data.CustomId == _id_poll_results_ddm)
                    {
                        await new InteractionVoteResults().Run(smc);
                    }
                    if(smc.Data.CustomId == _id_poll_options_ddm_option)
                    {
                        await new InteractionPollOptions().Run(smc);
                    }
                    if(smc.Data.CustomId.StartsWith(InteractionPollOptions._id_interaction_toggle_results))
                    {
                        await new InteractionToggleResults().Run(smc);
                    }
                    if(smc.Data.CustomId.StartsWith(InteractionPollOptions._id_interaction_view_results))
                    {
                        await new InteractionViewResultsOverride().Run(smc);
                    }
                    if (smc.Data.CustomId.StartsWith(InteractionPollOptions._id_interaction_toggle_lock))
                    {
                        await new InteractionToggleLock().Run(smc);
                    }
                }
            }
        }

        public Embed GenerateEmbed(Poll p)
        {
            var embed = new EmbedBuilder();
            embed.WithTitle(p.Title);
            var desc = new StringBuilder();
            desc.Append(p.Description)
                .Append("\n\n **Options:** ");
            foreach(string opt in p.OptionData)
            {
                desc.Append("\n").Append(opt);
            }
            embed.WithDescription(desc.ToString());
            embed.WithColor(Color.Blue);
            return embed.Build();
        }

        public async Task Setup(DiscordSocketClient client)
        {

            if(BotConfig.GetCachedConfig().Poll == null) // if poll list doesn't already exist, make one!
            {
                var cfg = BotConfig.GetCachedConfig();
                cfg.Poll = new List<Poll>();
                BotConfig.SaveConfig(cfg);
            }
            var addCommand = new SlashCommandBuilder();
            addCommand.WithName("newpoll");
            addCommand.WithDescription("Create a new poll");
            addCommand.AddOption("title", ApplicationCommandOptionType.String, "Title of poll");
            addCommand.AddOption("description", ApplicationCommandOptionType.String, "Description of what poll is about");
            addCommand.AddOption("option-data", ApplicationCommandOptionType.String, "Poll options, must be comma seperated values");
            addCommand.AddOption("show-results", ApplicationCommandOptionType.Boolean, "Show the results of this poll by default?");
            addCommand.AddOption("channel", ApplicationCommandOptionType.Channel, "Which channel to post this poll?");
            addCommand.AddOption("min-options", ApplicationCommandOptionType.Integer, "Set != 1 to allow user to pick multiple options.");
            addCommand.AddOption("max-options", ApplicationCommandOptionType.Integer, "Set != 1 to allow user to pick multiple options.");

            await client.Rest.CreateGuildCommand(addCommand.Build(), _guild);

            client.InteractionCreated += OnInteraction;
        }
 
    }
}
