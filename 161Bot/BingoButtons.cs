using Discord;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace _161Bot
{
    public class BingoButtons
    {
        public async Task HandleInteractions(SocketInteraction inter)
        {
            if(inter is SocketMessageComponent smc)
            {
                if(smc.Data.CustomId.StartsWith("BINGOBUTTON"))
                {
                    await smc.UpdateAsync(m =>
                    {
                        ComponentBuilder builder = ComponentBuilder.FromMessage(smc.Message);
                        foreach(var row in builder.ActionRows)
                        {
                        for (int i = 0; i < row.Components.Count; i++)
                            {
                                var button = row.Components[i] as ButtonComponent;
                                if(smc.Data.CustomId == button.CustomId)
                                {
                                   row.Components[i] = button.ToBuilder().WithStyle(button.Style == ButtonStyle.Primary ? ButtonStyle.Secondary : ButtonStyle.Primary).Build();
                                }
                            }
                        }

                        m.Components = builder.Build();
                        
                    });
                }
            }
        }
    }
}
