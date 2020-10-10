using System;
using System.Collections.Generic;
using System.Text;

namespace DevLifeBot.Models
{
    public class MenuWrapper
    {
        public ulong UserID { get; set; }
        public ulong MessageID { get; set; }
        public Menu Menu { get; set; }
    }
}
