using E_Chess.Models;
using E_Chess.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace E_Chess.Pages
{

    public class Rook : MasterPiece
    {
   

        public Rook(Color white, PieceData tile) : base(tile,white)
        {
         
            tile.PieceName = GetName();
            base.TypeName = CurrentPos.PieceName.FirstOrDefault();
        }

        public override  string GetImage()
        {
            if (Color.White == base.Color)
            {
                return PieceManager.GetImage("Rook2.jpg", Color.White);
            }
            return PieceManager.GetImage("Rook.jpg", Color.Black);

        }

        public override string GetName()
        {
            return "rook";
        }

        public override List<Tile> GetNeighbours(Board board)
        {
            int row = CurrentPos.Xpos, col = CurrentPos.Ypos;
         
            var list = new List<Tile>();
            if (Color.Black == base.Color)
            {
                while(row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col);
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
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (col - 1) && x.Xpos == row);
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
                    col -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col );

                    if (query != null)
                    {
                        if (!query.IsEmpty )
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (col + 1) && x.Xpos == row);

                    if (query != null)
                    {
                        if (!query.IsEmpty )
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
            }
            else
            {
                while (row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col );
                    if (query != null)
                    {
                        if (!query.IsEmpty )
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (col + 1) && x.Xpos == row );

                    if (query != null)
                    {
                        if (!query.IsEmpty)
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while(row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col);
                    if (query != null)
                    {
                        if (!query.IsEmpty )
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    row -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (col - 1) && x.Xpos == row );

                    if (query != null)
                    {
                        if (!query.IsEmpty )
                        {
                            list.Add(query);
                        }
                        if (query.IsEmpty)
                        {
                            list.Add(query);
                        }
                    }
                    col -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
            }
            return list;
        }

        public override void Move(Tile tile)
        {
            throw new System.NotImplementedException();
        }
    }
}