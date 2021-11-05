using Lab_1_worldOfWorms.Engine;

namespace Lab_1_worldOfWorms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Simulator simulator = new Simulator();
            
            AddWorm("W", 0, 0, AIType.SIMPLE);

            simulator.StartGame();
        }

        private static void AddWorm(string name, int x, int y, AIType ai)
        {
            var wormGo = new GameObject();
            var worm = new Worm();
            wormGo.AddComponent(worm);
            worm.Initialize(name, x, y, Simulator.instance._field, ai);
        }
    }
}