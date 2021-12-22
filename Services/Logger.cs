using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.Services
{
    public class Logger : Component
    {
        private readonly Field _field;
        private readonly StreamWriter _stream;
        public Logger(Field field)
        {
            string outputTxt = "output.txt";
            _stream = new StreamWriter(outputTxt);
            _field = field;
        }

        public override void Update()
        {
            var logWorms = "";
            if (_field.Worms.Count > 0)
            {
                logWorms = _field.Worms.Select(worm => worm.GetInfo()).Aggregate((acc, info) => $"{acc}, {info}");
            }
            var logFood = "";
            if (_field.Foods.Count > 0)
            {
                logFood = _field.Foods.Select(food => food.GetComponent<Transform>().Position.ToString())
                    .Aggregate((acc, info) => $"{acc}, {info}");
            }

            _stream.WriteLine($"Worms: [{logWorms}], Food: [{logFood}]");
            //Console.WriteLine($"Worms: [{logWorms}], Food: [{logFood}]");
        }
    }
}