namespace WormsWorld.Model
{
    public class WormAction
    {
        public Decision _decision;
        public Direction _direction;

        public WormAction(Decision decision, Direction direction)
        {
            _decision = decision;
            _direction = direction;
        }
    }
}