using System;
using System.Collections.Generic;
using System.Text;

namespace DevLifeBot.Models
{
    public static class TestData
    {
        public static Menu BuildTestMenu()
        {
            var menu = new Menu();
            menu.Title = "This is a Test Menu";
            menu.Instructions = "Just respond to some things...";
            menu.Options = new List<Option>();
            menu.Options.Add(new Option
            {
                Name = "To a submenu!",
                Details = "That's all",
                Action = () => new Menu
                {
                    Title = "This is a Submenu",
                    Instructions = "Cool eh?",
                    Options = new List<Option>
                    {
                        new Option
                        {
                            Name = "Yes",
                            Details = "I agree",
                            Action = () => "Yay!"
                        },
                        new Option
                        {
                            Name = "No",
                            Details = "I disagree",
                            Action = () => "Well okay then..."
                        }
                    }
                }
            });
            menu.Options.Add(new Option
            {
                Name = "Want a joke?",
                Details = "I can't promise to be funny.",
                Action = () => "I don't know any jokes..."
            });
            return menu;
        }
    }
}
