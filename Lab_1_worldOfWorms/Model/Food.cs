using System;
using System.Runtime.InteropServices;
using Lab_1_worldOfWorms.Engine;

namespace Lab_1_worldOfWorms.Model
{
    public class Food
    {
        public Position _position;
        public Action onSpoiled;
        private int _life = 10;

        public int life
        {
            get => _life;
            set
            {
                _life = value;
                if (life == 0)
                {
                    onSpoiled?.Invoke();
                }
            }
        }

        public Food(Position pos)
        {
            _position = pos;
        }
    }
}