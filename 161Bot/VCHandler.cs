using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;


namespace _161Bot
{
    public class VCHandler
    {

        private static long antiSpam = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

        public async Task HandleVC(SocketUser user, SocketVoiceState old, SocketVoiceState newState)
        {
            if (old.VoiceChannel == null && newState.VoiceChannel != null)
            {
                var guild = newState.VoiceChannel.Guild;
                await guild.GetUser(user.Id).AddRoleAsync(guild.GetRole(BotConfig.GetCachedConfig().VcConfig.RoleId));

                Console.WriteLine(antiSpam - DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 1800);
                if (DateTimeOffset.UtcNow.ToUnixTimeSeconds() < antiSpam + 1800)
                {
                    Console.WriteLine("anti spam worked, recorded time is " + antiSpam + " and the time now is " + DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                    return;
                }
                Console.WriteLine("joined voice");
                var embed = new EmbedBuilder();
                embed.WithColor(Color.Blue);
                embed.WithTitle("Voice Chat");
                embed.WithDescription(user.Username + " joined a voice chat.");
                embed.WithCurrentTimestamp();
                await newState.VoiceChannel.Guild.GetTextChannel(BotConfig.GetCachedConfig().VcConfig.ChannelId).SendMessageAsync(embed: embed.Build(), text: (newState.VoiceChannel.Users.Count == 1 ? "@here" : ""));
                // prevent spamming the bot
                antiSpam = DateTimeOffset.UtcNow.ToUnixTimeSeconds(); // thanks stackoverflow

            }
            if (old.VoiceChannel != null && newState.VoiceChannel == null)
            {
                await old.VoiceChannel.Guild.GetUser(user.Id).RemoveRoleAsync(old.VoiceChannel.Guild.GetRole(BotConfig.GetCachedConfig().VcConfig.RoleId));
            }
        }

    }
}
