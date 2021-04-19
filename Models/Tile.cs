namespace E_Chess.Models
{
    public class Tile
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public MasterPiece Piece { get; set; }
        public bool IsEmpty { get; set; }
        public int Xpos { get; set; }
        public int Ypos { get; set; }
        public bool isUnderAttackBlack { get; set; }
        public bool isUnderAttackWhite { get; set; }
       
    }
}