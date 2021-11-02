using System;
using System.IO;
using System.Linq;
using System.Text;

namespace Lab_1_worldOfWorms
{
    public class Simulator
    {
        public Field _field;
        private StreamWriter output;

        public Simulator()
        {
            _field = new Field();
        }

        public void StartGame()
        {
            using (output = new StreamWriter("output.txt", false))
            {
                while (_field.worms.Any())
                {
                    _field.Update();
                    output.Write(_field.GetInfo());
                }    
            }
        }
        
    }
    
    
}