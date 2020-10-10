using DevLifeBot.Models;
using Discord;
using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace DevLifeBot.Handlers
{
    public static class MenuHandler
    {
        public static Embed ConvertMenuToEmbed(Menu menu)
        {
            List<EmbedFieldBuilder> options = new List<EmbedFieldBuilder>();
            int i = 0;
            foreach(var option in menu.Options)
            {
                var fieldBuilder = new EmbedFieldBuilder();
                fieldBuilder.WithName($"{new Rune(127462 + i++)} {option.Name}");
                fieldBuilder.WithValue(option.Details ?? "");
                options.Add(fieldBuilder);
            }

            var embedBuilder = new EmbedBuilder();
            embedBuilder.WithTitle(menu.Title);
            if(!string.IsNullOrWhiteSpace(menu.Instructions)) embedBuilder.WithDescription(menu.Instructions);
            embedBuilder.WithFields(options);
            return embedBuilder.Build();
        }
    }
}
