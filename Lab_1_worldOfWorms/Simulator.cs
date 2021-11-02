using System;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Text;

namespace Lab_1_worldOfWorms
{
    public class Simulator
    {
        public Field _field;

        private StringBuilder wormsInfo;

        public Simulator()
        {
            _field = new Field();
            wormsInfo = new StringBuilder();
        }

        public void StartGame()
        {
            while (_field.worms.Any())
            {
                _field.Update();
            }
        }

        
    }
    
    
}