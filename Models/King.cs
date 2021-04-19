using E_Chess.Models;
using E_Chess.Utilities;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace E_Chess.Pages
{

    public class King : MasterPiece
    {
      

        public King(Color white, PieceData tile) : base(tile, white)
        {
       
            tile.PieceName = GetName();
            base.TypeName = tile.PieceName.FirstOrDefault();
        }

        public  override string GetImage()
        {
            if (Color.White == base.Color)
            {
               return PieceManager.GetImage("King2.jpg", base.Color);
            }
            return PieceManager.GetImage("King.jpg", Color.Black);
        }

        public override string GetName()
        {
            return "king";
        }


        private void ChannelTest(Tile query, List<Tile> list)
        {
          
            if (query != null)
            {
                if (query.IsEmpty)
                {
              
                    list.Add(query);
                    
                   
                }

                if (query.Piece != null)
                {
                    
                    if (query.Piece.Color != base.Color)
                    {
                        if(query.Piece.Color == Color.White && !query.isUnderAttackBlack)
                        {
                            list.Add(query);
                        }
                        if (query.Piece.Color == Color.Black && !query.isUnderAttackWhite)
                        {
                            list.Add(query);
                        }
                    }

                }
               
            }
        }
        public override  List<Tile> GetNeighbours(Board board)
        {
            int row = CurrentPos.Xpos, col = CurrentPos.Ypos;
            var list = new List<Tile>();
            if ((row > 0 && row < board.Rows-1) && (col > 0 && col < board.Columns-1))
            {
                var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col );
                var rightup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
                var leftup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));
                var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                var leftdown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
                var rightDown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
                var right = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col + 1));
                var left = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col - 1));

                ChannelTest(up, list);
                ChannelTest(rightup, list);
                ChannelTest(leftup, list);
                ChannelTest(down, list);
                ChannelTest(leftdown, list);
                ChannelTest(rightDown, list);
                ChannelTest(left, list);
                ChannelTest(right, list);
             
            }
            else if (row == 0 && col >= 0)
            {
                var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col);
                var rightup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
                var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                var rightDown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
                var right = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col + 1) );
                var left = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col - 1) );
                var leftup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row+1) && x.Ypos == (col - 1));
                ChannelTest(up, list);
                ChannelTest(rightup, list);
                ChannelTest(leftup, list);
                ChannelTest(down, list);
            
                ChannelTest(rightDown, list);
                ChannelTest(left, list);
                ChannelTest(right, list);


            }
            else if (row == board.Rows - 1 && col >= 0)
            {


                var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col);
               
                var leftdown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));
                var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                var rightdown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
                var right = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col + 1));
                var left = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col - 1));
                var leftup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));

                ChannelTest(up, list);
           
                ChannelTest(leftup, list);
                ChannelTest(down, list);
                ChannelTest(leftdown, list);
                ChannelTest(right, list);
                ChannelTest(left, list);
                ChannelTest(rightdown, list);


            }
            else if (col == 0 && row >= 0)
            {


                var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col);

                var rightup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
                var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                var rightDown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
                var right = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col + 1));
              


                ChannelTest(up, list);

                ChannelTest(rightup, list);
                ChannelTest(down, list);
                ChannelTest(rightDown, list);
                ChannelTest(right, list);
              


            }
            else if (col == board.Columns - 1 && row >= 0)
            {


                var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col);

                var leftup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));
                var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                var leftdown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
               
                var left = board.Tiles.Values.FirstOrDefault(x => x.Xpos == row && x.Ypos == (col - 1));


                ChannelTest(up, list);

                ChannelTest(leftup, list);
                ChannelTest(down, list);
                ChannelTest(leftdown, list);
            
                ChannelTest(left, list);


            }
            return list;
        }

        public override  void Move(Tile tile)
        {
            throw new System.NotImplementedException();
        }
    }
}