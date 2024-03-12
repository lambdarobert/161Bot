using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    public class ModalHandler
    {
        public async Task Handle(SocketModal m)
        {
            var components = m.Data.Components.ToList();

            string kishoreification = components.First(x => x.CustomId == "level").Value;
            string text = components.First(x => x.CustomId == "content").Value;

            UInt16 level = 0;

            try
            {
                level = UInt16.Parse(kishoreification);
                if(level < 1 || level > 5)
                {
                    throw new Exception();
                }
            }
            catch (Exception ex)
            {
                await m.RespondAsync(embed: QuickEmbeds.Error("Not a valid number."), ephemeral: true);
            }

            string newStr = "";
            var rnd = new Random();
            foreach (char c in text)
            {
                bool b = rnd.Next(level + 1) == 1;
                if (b)
                {
                    newStr += Char.ToUpper(c);
                }
                else
                {
                    newStr += c;
                }
            }
            await m.RespondAsync(newStr, ephemeral: true);

        }
    }
}
