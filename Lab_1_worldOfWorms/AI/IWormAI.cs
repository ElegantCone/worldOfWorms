using Lab_1_worldOfWorms.Engine;
using Lab_1_worldOfWorms.Model;

namespace Lab_1_worldOfWorms.AI
{
    public interface IWormAI
    {

        public WormAction GetNextAction(Position position, Transform food = null);
    }
}