using Discord;
using System;
using System.Collections.Generic;
using System.Text;

namespace _161Bot
{
    public class QuickEmbeds
    {
        public static Embed Error(string errorDescription)
        {
            var e = new EmbedBuilder();
            e.WithTitle("Error");
            e.WithDescription(errorDescription);
            e.WithColor(Color.Red);
            e.WithCurrentTimestamp();
            return e.Build();
        }

        public static Embed PermissionError()
        {
            var e = new EmbedBuilder();
            e.WithTitle("Permission Error");
            e.WithDescription("You don't have permission to do this.");
            e.WithColor(Color.Red);
            return e.Build();
        }
    }
}
