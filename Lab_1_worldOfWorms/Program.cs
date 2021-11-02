namespace Lab_1_worldOfWorms
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            Simulator simulator = new Simulator();

            var worm = new Worm("Worm1", 0, 0, simulator._field, AIType.CIRCLE);

            simulator.StartGame();
        }
    }
}