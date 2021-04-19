using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Chess.Models
{
    public class MasterPiece
    {
        public PieceData CurrentPos { get; set;  }
        public Color Color { get; set; }
        public int MoveCount { get; set; }
        public char TypeName { get; set; }
        public MasterPiece()
        {
        }
        public MasterPiece(PieceData data, Color color)
        {
            CurrentPos = data;
            Color = color;
            var t = GetImage();
            if (!string.IsNullOrEmpty(t))
            {
                CurrentPos.Image = GetImage();
            }

         
        }

        public virtual List<Tile> GetNeighbours(Board board)
        {
            return new List<Tile>();
        }
        public virtual void Move(Tile tile)
        {
            
        }

        public virtual string GetName()
        {
            return "";
        }
        public virtual string GetImage()
        {
            return "";
        }
        public void SetImage(string img)
        {
            CurrentPos.Image = img;
        }
        public void SetMoveCount(int move)
        {
            MoveCount = move;
        }
        public override string ToString()
        {
            var data = new { Name = GetName(), Image = GetImage(), CurrentPos.Xpos, CurrentPos.Ypos, CurrentPos.Description, Color, MoveCount, TypeName };
            return JsonConvert.SerializeObject(data);
        } 
    }
}
