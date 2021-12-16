namespace WormsWorld.Engine
{
    public class Position
    {
        public int x { get; set; }
        public int y { get; set; }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static bool operator ==(Position p1, Position p2)
        {
            return p1.x == p2.x && p1.y == p2.y;
        }

        public static bool operator !=(Position p1, Position p2)
        {
            return !(p1 == p2);
        }
        
        public static explicit operator Vector2d(Position p)
        {
            return new Vector2d(p.x, p.y);
        }

        public static float Distance(Position p1, Position p2)
        {
            return ((Vector2d) p2 - (Vector2d) p1).Length();
        }

        public override string ToString()
        {
            return $"({x}, {y})";
        }
    }
}