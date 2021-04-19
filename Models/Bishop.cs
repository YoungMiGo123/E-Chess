using E_Chess.Models;
using E_Chess.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace E_Chess.Pages
{

    public class Bishop : MasterPiece    
    {
        public int PieceID { get; set; }

        public Bishop()
        {
        }

        public Bishop(Color white, PieceData tile) : base(tile, white)
        {
        
            tile.PieceName = GetName();
            base.TypeName = tile.PieceName.FirstOrDefault();
        }

        public override string GetImage()
        {
            if (Color.White == base.Color)
            {
                return PieceManager.GetImage("Bishop2.jpg", base.Color);
            }
            return PieceManager.GetImage("Bishop.jpg", Color.Black);
        }

        public override string GetName()
        {
            return "bishop";
        }

   
        public override List<Tile> GetNeighbours(Board board)
        {
            int row = CurrentPos.Xpos, col = CurrentPos.Ypos;

            var list = new List<Tile>();

            if(Color.Black == base.Color)
            {
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col-1));
                  
                    if (query != null) 
                    { 
                        if(!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1) );
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1) );
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1) );
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col -= 1;
                }
            }
            else
            {
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1) );
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1) );
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1) );
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1) );
                    if (query != null)
                    {
                        if (!query.IsEmpty && query.Piece.Color != base.Color)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                    col -= 1;
                }
            }
            return list;
        }

        public override  void Move(Tile tile)
        {
           
        }
    }
}