namespace Lab_1_worldOfWorms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Simulator simulator = new Simulator();

            var worm = new Worm("Kek", 0, 0, simulator._field, AIType.CIRCLE);
            var worm2 = new Worm("Kek2", 0, 0, simulator._field, AIType.CIRCLE);
            
            simulator.StartGame();
        }
    }
}