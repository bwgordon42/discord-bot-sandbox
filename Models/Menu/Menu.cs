using System;
using System.Collections.Generic;
using System.Text;

namespace DevLifeBot.Models
{
    public class Menu
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public List<Option> Options { get; set; }
        public Menu BackReference { get; set; }

    }
}
