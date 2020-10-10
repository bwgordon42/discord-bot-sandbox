using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevLifeBot.Models;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace DevLifeBot.Handlers
{
    class ReactionAddedHandler
    {
        private readonly DiscordSocketClient _client;

        public const int ENCLOSED_A_INT = 127462;

        public ReactionAddedHandler(DiscordSocketClient client)
        {            
            _client = client;
        }

        public void InstallReactionAddedHandler()
        {
            _client.ReactionAdded += HandleReactionAddedAsync;
        }

        private async Task HandleReactionAddedAsync(Cacheable<IUserMessage, ulong> reactionMessage, ISocketMessageChannel reactionChannel, SocketReaction reaction)
        {
            if (reaction.User.IsSpecified && reaction.User.Value.IsBot) return;
            var rune = Rune.GetRuneAt(reaction.Emote.Name,0);

            var message = await reactionMessage.GetOrDownloadAsync();
            var menu = BotContext.Menus.FirstOrDefault(m => m.MessageID == message.Id );
            if(menu != null)
            {
                await message.RemoveReactionAsync(reaction.Emote, reaction.User.Value);
            }
            if (menu?.UserID == reaction.UserId && rune.Value >= ENCLOSED_A_INT && rune.Value < (ENCLOSED_A_INT+26))
            {
                int i = rune.Value - ENCLOSED_A_INT;
                if (i < menu.Menu.Options.Count)
                {
                    var option = menu.Menu.Options[i];
                    var result = option.Action();
                    switch (result)
                    {
                        case string response:
                            await message.DeleteAsync();
                            await reactionChannel.SendMessageAsync("Menu Result: " + response);
                            BotContext.Menus.Remove(menu);
                            break;
                        case Menu submenu:
                            await message.RemoveAllReactionsAsync();
                            await message.ModifyAsync(m => m.Embed = MenuHandler.ConvertMenuToEmbed(submenu));
                            foreach (var o in submenu.Options)
                            {
                                await message.AddReactionAsync(new Emoji((new Rune(127462 + i++).ToString())), new RequestOptions { RetryMode = RetryMode.RetryRatelimit });
                            }
                            submenu.BackReference = menu.Menu;
                            menu.Menu = submenu;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }
    }
}
