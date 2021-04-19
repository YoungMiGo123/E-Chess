using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Chess.Models
{
    public class TrackMove
    {
        public TrackMove( Color color, char pieceType, string loc)
        {
            Color = color;
            PieceType = pieceType;
            Loc = loc;
            MoveMade = $"{PieceType}{Loc.ToLower()}";
        }

        public int Id { get; set; }
        public Color Color { get; set; }

        public char PieceType { get; set; }
        public string Loc { get; set; }
        public string MoveMade { get; set; } 


    }
}
