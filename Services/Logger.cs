using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WormsWorld.Engine;

namespace WormsWorld.Services
{
    public class Logger : Component
    {
        private Field _field;
        private StreamWriter _stream;
        public Logger(Field field)
        {
            string outputTxt = "output.txt";
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