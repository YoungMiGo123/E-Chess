using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Chess.Models
{
    public class Check
    {
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public int CheckID { get; set; }
        public bool isChecked { get; set; }

        public string ColorInCheck { get; set; }
        public bool isActive { get; set; }
    }
}
