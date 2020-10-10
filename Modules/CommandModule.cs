using DevLifeBot.Handlers;
using DevLifeBot.Models;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DevLifeBot.Modules
{
    class CommandModule : ModuleBase<SocketCommandContext>
    {
        [Command("xkcd")]
        public async Task RandomComic()
        {
            try
            {
                using(var client = new HttpClient())
                {
                    var newestJson = client.GetAsync("https://xkcd.com/info.0.json");
                    var newestData = JsonSerializer.Deserialize<ComicData>(await newestJson.Result.Content.ReadAsByteArrayAsync());
                    Random rand = new Random();
                    var r = rand.Next(1, newestData.num);
                    var randomJson = client.GetAsync($"https://xkcd.com/{r}/info.0.json");
                    var randomData = JsonSerializer.Deserialize<ComicData>(await randomJson.Result.Content.ReadAsByteArrayAsync());

                    var fieldBuilder = new EmbedFieldBuilder();
                    fieldBuilder.WithName("Camping!");
                    fieldBuilder.WithValue("How about no");

                    var embedBuilder = new EmbedBuilder();
                    embedBuilder.WithImageUrl(randomData.img);
                    embedBuilder.WithTitle($"{randomData.title} #{randomData.num}");
                    embedBuilder.WithFooter(randomData.alt);
                    embedBuilder.WithUrl($"https://xkcd.com/{randomData.num}/");
                    embedBuilder.WithCurrentTimestamp();
                    embedBuilder.WithAuthor("Author", Context.User.GetAvatarUrl(), "http://example.com");
                    embedBuilder.WithColor(Color.Green);
                    embedBuilder.WithDescription("Hey look a description");
                    embedBuilder.WithFields(fieldBuilder, fieldBuilder);
                    
                    Console.WriteLine(randomData.alt);

                    await Context.Message.Channel.SendMessageAsync(embed: embedBuilder.Build());

                }

            }
            catch (Exception e)
            { Console.WriteLine(e.Message); await Context.Message.Channel.SendMessageAsync("Sorry, something went wrong. :( "); }
        }

        [Command("kill")]
        public async Task Kill()
        {
            await Context.Client.LogoutAsync();
            Environment.Exit(0);
        }


        [Command("test-menu")]
        public async Task TestMenu()
        {
            try
            {
                var menu = TestData.BuildTestMenu();
                var embed = MenuHandler.ConvertMenuToEmbed(menu);
                var message = await Context.Message.Channel.SendMessageAsync(embed: embed);
                BotContext.Menus.Add(new MenuWrapper
                {
                    Menu = menu,
                    MessageID = message.Id,
                    UserID = Context.User.Id
                });
                int i = 0;
                foreach (var option in menu.Options)
                {
                    await message.AddReactionAsync(new Emoji((new Rune(127462 + i++).ToString())), new RequestOptions { RetryMode = RetryMode.RetryRatelimit });
                }
            }
            catch (Exception e)
            { Console.WriteLine(e.Message); await Context.Message.Channel.SendMessageAsync("Sorry, something went wrong. :( "); }
        }
    }
}
