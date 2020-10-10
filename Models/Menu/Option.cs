using System;
using System.Collections.Generic;
using System.Text;

namespace DevLifeBot.Models
{
    public class Option
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Func<object> Action { get; set; }
    }
}
