using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_Chess.Models
{
    public class Board
    {
        public Dictionary<int, Tile> Tiles { get; set; }
        public int Rows = 8;
        public int Columns = 8;
        public int BoardSize;

        public Board()
        {
            Tiles = new Dictionary<int, Tile>();
            BoardSize = Rows * Columns;
        }
        public Tile GetTile(string Description)
        {
            return Tiles.Values.FirstOrDefault(x => x.Description == Description);
        }
        public Tile GetTile(int Id)
        {
            return Tiles[Id];
        }
        public void SetTile(Tile tile)
        {
            if(tile.Piece != null)
            {
                tile.IsEmpty = false;
            }
            else
            {
                tile.IsEmpty = true;
            }
           var item = Tiles.Where(x => x.Value.Description == tile.Description).FirstOrDefault();
           Tiles[item.Key] = tile;
        }
    }
}
