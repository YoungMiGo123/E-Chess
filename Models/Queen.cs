using E_Chess.Models;
using E_Chess.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace E_Chess.Pages
{

    public class Queen : MasterPiece
    {
     

        public Queen(Color white, PieceData tile) : base(tile,white)
        {
          
            tile.PieceName = GetName();
            base.TypeName = CurrentPos.PieceName.FirstOrDefault();
        }

        public override  string GetImage()
        {
            if(Color.White == base.Color)
            {
                return PieceManager.GetImage("Queen2.jpg", Color.White);
            }
            return PieceManager.GetImage("Queen.jpg", Color.Black);
        }

        public override string GetName()
        {
            return "queen";
        }
        public char TypeName { get; set; } = 'Q';


        public override List<Tile> GetNeighbours(Board board)
        {
            int row = CurrentPos.Xpos, col = CurrentPos.Ypos;

            var list = new List<Tile>();

            #region Functionality
            //Rook functionality
            if (Color.Black == base.Color)
            {
                while (row >= 0 && row < board.Rows)
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
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (col - 1) && x.Xpos == row);
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
                while (row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);

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
            }
            else
            {
                while (row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
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
                while (row >= 0 && row < board.Rows)
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
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Ypos == (col - 1) && x.Xpos == row);

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

            //Bishop Functionality
            if (Color.Black == base.Color)
            {
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));

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
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
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
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
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
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
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
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));
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
                    row -= 1;
                    col -= 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
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
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
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
                    col += 1;
                }
                row = CurrentPos.Xpos; col = CurrentPos.Ypos;
                while (col >= 0 && col < board.Columns && row >= 0 && row < board.Rows)
                {
                    var query = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
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
                    col -= 1;
                }
            }
            #endregion

            return list;
        }

        public override void Move(Tile tile)
        {
            throw new System.NotImplementedException();
        }
    }
}