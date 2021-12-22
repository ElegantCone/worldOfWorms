namespace WormsWorld.Model
{
    public class WormAction
    {
        public readonly Decision Decision;
        public readonly Direction Direction;

        public WormAction(Decision decision, Direction direction)
        {
            Decision = decision;
            Direction = direction;
        }
    }
}