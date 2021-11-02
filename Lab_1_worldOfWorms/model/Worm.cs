using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Lab_1_worldOfWorms.ai;

namespace Lab_1_worldOfWorms
{
    public class Worm : IUpdatable
    {
        private Position _position;

        private string name;

        private Field _field;

        private int _life;
        private int life
        {
            get => _life;
            set
            {
                _life = value;
                if (life <= 0)
                {
                    _field.RemoveWorm(this);
                }
            }
        }

        private int radius;

        private IWormAI ai;

        private Dictionary<Direction, Action> ChangePosition;

        public Worm(string name, int x, int y, Field field, AIType type)
        {
            this.name = name;
            _position = new Position(x, y);
            _field = field;
            _field.worms.Add(this);
            life = 100;

            switch (type)
            {
                case AIType.CIRCLE:
                    ai = new WormAIRound(x, y, 3);
                    break;
                //todo: остальные виды ИИ
            }

            ChangePosition = new()
            {
                {Direction.UP, () => _position.y++},
                {Direction.DOWN, () => _position.y--},
                {Direction.LEFT, () => _position.x--},
                {Direction.RIGHT, () => _position.x++}
            };


        }

        public void MakeAction()
        {
            WormAction wAction = ai.GetNextAction(_position);
            //Console.WriteLine($"Life: {life}, Action: {wAction}");
            if (wAction._decision == Decision.MOVE)
            {
                Position pos = _position;
                ChangePosition[wAction._direction]();
                if (!CheckCells(_position))
                {
                    _position = pos;
                }
            }
            life--;
        }
        
        /*//todo: перенести метод в более подходящее место
        private Position GetNextMove()
        {
            Direction dir = (Direction) rnd.Next(0, 3);
            int x = _position.x;
            int y = _position.y;
            switch (dir)
            {
                case (Direction.UP):
                    y++;
                    break;
                case (Direction.DOWN):
                    y--;
                    break;
                case (Direction.LEFT):
                    x--;
                    break;
                case (Direction.RIGHT):
                    x++;
                    break;
            }

            return CheckCells(x, y)? new Position(x, y) :  _position;
        }*/
        private bool CheckCells(Position position)
        {
            
            foreach (var worm in _field.worms) 
            {
                if (worm._position == position)
                {
                    return false;
                }
            }
            return true;
        }

        public string GetInfo()
        {
            StringBuilder sb = new StringBuilder($"{name}, ({_position.x},{_position.y})");
            return sb.ToString();
        }

        public void Update()
        {
            MakeAction();
        }
    }
}