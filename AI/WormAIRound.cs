using System;
using System.Collections.Generic;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.AI
{
    public class WormAIRound : IWormAI
    {
        private static readonly Dictionary<Direction, Func<Position, Position, int, bool>> ChangeDirectionConditions =
            new()
            {
                { Direction.Up, (position, center, radius) => position.y >= center.y + radius },
                { Direction.Down, (position, center, radius) => position.y <= center.y - radius },
                { Direction.Left, (position, center, radius) => position.x <= center.x - radius },
                { Direction.Right, (position, center, radius) => position.x >= center.x + radius }
            };

        private Position _center;
        private int _radius;

        private WormAction previousAction = new WormAction(Decision.Move, Direction.Up);
        
        public WormAIRound(int x, int y, int r)
        {
            _center = new Position(x, y);
            _radius = r;
        }
        
        public WormAction GetNextAction(Position position, Transform food = null)
        {
            var dir = previousAction.Direction;

            if (ChangeDirectionConditions[dir](position, _center, _radius))
            {
                return previousAction = new WormAction(Decision.Move, GetNextDirection(dir));
            }
            
            return previousAction = new WormAction(Decision.Move, dir);
        }

        private Direction GetNextDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.Up: return Direction.Right;
                case Direction.Right: return Direction.Down;
                case Direction.Down: return Direction.Left;
                case Direction.Left: return Direction.Up;
            }

            return Direction.Up;
        }
        
    }
}