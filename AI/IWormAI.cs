using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.AI
{
    public interface IWormAI
    {

        public WormAction GetNextAction(Position position, Transform food = null);
    }
}