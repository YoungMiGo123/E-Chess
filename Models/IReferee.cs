using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Chess.Models
{
    public class Referee
    {
        public Board board { get; set; }
        public bool ProcessMove(Tile a, Tile b)
        {
            return false;
        }

    }
}
