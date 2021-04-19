using System.Collections.Generic;

namespace E_Chess.Models
{
    public interface Piece
    {
        public List<Tile> GetNeighbours(Board board);
        public void Move(Tile tile);
        public string GetName();
        public string GetImage();
    }
}