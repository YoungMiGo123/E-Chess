using E_Chess.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace E_Chess.Models
{

    public class Pawn : MasterPiece
    {
        public Pawn()
        {
        }

        public Pawn(Color color, PieceData data) : base(data, color)
        {  
            data.PieceName = GetName();
            base.TypeName = data.PieceName.FirstOrDefault();
        }

        public int PieceID { get; set; }

        public bool isFirstMove { get; set; } 

        public override string GetImage()
        {
            if (Color.White == Color)
            {
                return PieceManager.GetImage("Pawn2.jpg", Color.White);
            }
            return PieceManager.GetImage("Pawn.jpg", Color.Black);

        }
       
        public void isFirstMoveTest(bool flag)
        {
            isFirstMove = flag;
        }
        public  override string GetName()
        {
            return "pawn";
        }

        public override List<Tile> GetNeighbours(Board board)
        {

            int row = CurrentPos.Xpos, col = CurrentPos.Ypos;
            var list = new List<Tile>();
            if ((row > 0 && row < board.Rows-1) && (col > 0 && col < board.Columns-1))
            {
                if(Color.Black == Color)
                {
                    var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row-1) && x.Ypos == col);
                    
                    var rightup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
                    var leftup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));
                    if (base.MoveCount == 0)
                    {
                        var up2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == col);
                        if (up2 != null && up2.IsEmpty)
                        {
                            list.Add(up2);
                        }
                  
                        var tile = board.Tiles.Values.FirstOrDefault(x => x.Description == up2.Description);
                        
                    }
                    if (up != null && up.IsEmpty)
                    {
                        list.Add(up);
                    }
                    list.Add(rightup);
                    list.Add(leftup);
               
                }
                else
                {
                    var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                    var leftdown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
                    var rightDown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
                    if (base.MoveCount == 0)
                    {
                        var down2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == col);
                        if (down2 != null && down2.IsEmpty)
                        {
                            list.Add(down2);
                       
                        }
                    }
                    if (down != null && down.IsEmpty)
                    {
                        list.Add(down);
                    }
                    list.Add(leftdown);
                    list.Add(rightDown);
                }
            }
            else if(col == 0)
            {
                if (Color.Black == Color)
                {

                    var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col);
                    var rightup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col + 1));
                    if (base.MoveCount == 0)
                    {
                        var up2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == col);
                        if (up2 != null && up2.IsEmpty)
                        {
                            list.Add(up2);
                         
                        }
                    }
                    if (up != null &&  up.IsEmpty) { list.Add(up);  }
                    list.Add(rightup);
                }
                else
                {
                    var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                    var rightDown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col + 1));
                    if (base.MoveCount == 0)
                    {
                        var down2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == col);
                        if (down2 != null && down2.IsEmpty)
                        {
                            list.Add(down2);
                           
                        }
                    }
                    if (down != null && down.IsEmpty)
                    {
                        list.Add(down);
                    }
                    list.Add(rightDown);
                }
            }
            else if (col == board.Columns - 1)
            {
                if (Color.Black == Color)
                {

                    var up = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == col );
                    var leftup = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 1) && x.Ypos == (col - 1));
                    if (base.MoveCount == 0)
                    {
                        var up2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row - 2) && x.Ypos == col);
                        if (up2 != null && up2.IsEmpty)
                        {
                            list.Add(up2);
                       
                        }
                    }
                    if (up != null && up.IsEmpty) { list.Add(up); }
                    list.Add(leftup);
                }
                else
                {
                    var down = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == col);
                    var leftdown = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 1) && x.Ypos == (col - 1));
                    if (base.MoveCount == 0)
                    {
                        var down2 = board.Tiles.Values.FirstOrDefault(x => x.Xpos == (row + 2) && x.Ypos == col);
                        if (down2 != null && down2.IsEmpty)
                        {
                            list.Add(down2);
                          
                        }
                    }
                    if (down != null &&  down.IsEmpty)
                    {
                        list.Add(down);
                    }
                    list.Add(leftdown);

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
