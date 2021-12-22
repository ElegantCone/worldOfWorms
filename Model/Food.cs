using System;
using System.Runtime.InteropServices;
using WormsWorld.Engine;

namespace WormsWorld.Model
{
    public class Food
    {
        private Position _position;
        private Action _onSpoiled;
        private int _life = 10;

        public int life
        {
            get => _life;
            set
            {
                _life = value;
                if (life == 0)
                {
                    _onSpoiled?.Invoke();
                }
            }
        }

        public Food(Position pos)
        {
            _position = pos;
        }
    }
}