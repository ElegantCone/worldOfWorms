namespace Lab_1_worldOfWorms.ai
{
    public interface IWormAI
    {

        public WormAction GetNextAction(Position position);
    }
}