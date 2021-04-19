namespace E_Chess.Models
{
    public class Controlled
    {
        public int controlID { get; set; }
        public Color Color { get; set; }
        public bool isUnderAttack { get; set; }
        public string ControlledBy { get; set; }
    }
}