using E_Chess.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace E_Chess.Models
{

    public class Horse : MasterPiece
    {
        public Horse(Color color, PieceData currentPos) : base(currentPos, color)
        {
           
            CurrentPos.PieceName = GetName();
            base.TypeName = CurrentPos.PieceName.FirstOrDefault();
        }

        public int PieceID { get; set; }
  
        public override string GetImage()
        {
            if (Color.White == Color)
            {
                return PieceManager.GetImage("Horse2.jpg", Color);
            }
            return PieceManager.GetImage("Horse.jpg", Color.Black);

        }

        public override string GetName()
        {
            return "horse";
        }

       private void checkTile(Tile q1, List<Tile> list)
        {
            if (q1 != null)
            {
                if (!q1.IsEmpty && q1.Piece.Color != base.Color)
                {
                    list.Add(q1);
                }
                if (q1.IsEmpty)
                {
                    list.Add(q1);
                }

            }
        }
        public override List<Tile> GetNeighbours(Board board)
        {
            int row = CurrentPos.Xpos, col = CurrentPos.Ypos;

            var list = new List<Tile>();

            if(Color.Black == base.Color)
            {
                if(col > 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var q1 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 2));
                    var q2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 2));
                    var q3 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 2));
                    var q4 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 2));
                    var q5 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == (col - 1));
                    var q6 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == (col + 1));
                    var q7 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == (col - 1));
                    var q8 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == (col + 1));

                    checkTile(q1, list);
                    checkTile(q2, list);
                    checkTile(q3, list);
                    checkTile(q4, list);
                    checkTile(q5, list);
                    checkTile(q6, list);
                    checkTile(q7, list);
                    checkTile(q8, list);


                }
                if(col == 0 && row >= 0 && row < board.Rows)
                {
                    var q2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 2));
                    var q6 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == (col + 1));
                    var q8 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == (col + 1));
                    var q4 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 2));

                    checkTile(q2, list);
                    checkTile(q6, list);
                    checkTile(q8, list);
                    checkTile(q4, list);
                }
                if (col == board.Columns - 1)
                {

                }
            }
            else
            {
                if (col > 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var q1 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 2));
                    var q2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 2));
                    var q3 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 2));
                    var q4 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 2));
            
                    var q5 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == (col - 1));
                    var q6 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == (col + 1));
                    var q7 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == (col - 1));
                    var q8 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == (col + 1));
                    checkTile(q1, list);
                    checkTile(q2, list);
                    checkTile(q3, list);
                    checkTile(q4, list);
                    checkTile(q5, list);
                    checkTile(q6, list);
                    checkTile(q7, list);
                    checkTile(q8, list);

                }
                if (col == 0 && row >= 0 && row < board.Rows)
                {
                    var q2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 2));
                    var q6 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == (col + 1));
                    var q8 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == (col + 1));
                    var q4 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 2));

                    checkTile(q2, list);
                    checkTile(q6, list);
                    checkTile(q8, list);
                    checkTile(q4, list);

                }
                if(col == board.Columns - 1)
                {

                }
            }
            return list;
        }

        public override  void Move(Tile tile)
        {
            throw new NotImplementedException();
        }
    }
}
