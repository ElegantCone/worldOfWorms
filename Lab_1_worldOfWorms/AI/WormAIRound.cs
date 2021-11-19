using System;
using System.Collections.Generic;
using Lab_1_worldOfWorms.Engine;
using Lab_1_worldOfWorms.Model;

namespace Lab_1_worldOfWorms.AI
{
    public class WormAIRound : IWormAI
    {
        private static readonly Dictionary<Direction, Func<Position, Position, int, bool>> ChangeDirectionConditions =
            new()
            {
                { Direction.UP, (position, center, radius) => position.y >= center.y + radius },
                { Direction.DOWN, (position, center, radius) => position.y <= center.y - radius },
                { Direction.LEFT, (position, center, radius) => position.x <= center.x - radius },
                { Direction.RIGHT, (position, center, radius) => position.x >= center.x + radius }
            };

        private Position _center;
        private int _radius;

        private WormAction previousAction = new WormAction(Decision.MOVE, Direction.UP);
        
        public WormAIRound(int x, int y, int r)
        {
            _center = new Position(x, y);
            _radius = r;
        }
        
        public WormAction GetNextAction(Position position, Transform food = null)
        {
            var dir = previousAction._direction;

            if (ChangeDirectionConditions[dir](position, _center, _radius))
            {
                return previousAction = new WormAction(Decision.MOVE, GetNextDirection(dir));
            }
            
            return previousAction = new WormAction(Decision.MOVE, dir);
        }

        private Direction GetNextDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.UP: return Direction.RIGHT;
                case Direction.RIGHT: return Direction.DOWN;
                case Direction.DOWN: return Direction.LEFT;
                case Direction.LEFT: return Direction.UP;
            }

            return Direction.UP;
        }
        
    }
}