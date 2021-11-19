using System;
using System.IO;
using System.Linq;
using Lab_1_worldOfWorms.Engine;

namespace Lab_1_worldOfWorms
{
    public class Logger : Component
    {
        private Field _field;
        private StreamWriter _stream;
        public Logger(string outputTxt, Field field)
        {
            _stream = new StreamWriter(outputTxt);
            _field = field;
        }

        public override void Update()
        {
            String logWorms = "";
            if (_field.worms.Count > 0)
            {
                logWorms = _field.worms.Select(worm => worm.GetInfo()).Aggregate((acc, info) => $"{acc}, {info}");
            }
            String logFood = "";
            if (_field.foods.Count > 0)
            {
                logFood = _field.foods.Select(food => food.GetComponent<Transform>().position.ToString())
                    .Aggregate((acc, info) => $"{acc}, {info}");
            }

            _stream.WriteLine($"Worms: [{logWorms}], Food: [{logFood}]");
            Console.WriteLine($"Worms: [{logWorms}], Food: [{logFood}]");
        }
    }
}