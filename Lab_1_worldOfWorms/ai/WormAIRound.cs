using System;
using System.Collections.Generic;

namespace Lab_1_worldOfWorms.ai
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

        private Position center;
        private int radius;

        private WormAction previousAction = new WormAction(Decision.MOVE, Direction.UP);
        
        public WormAIRound(int x, int y, int r)
        {
            center = new Position(x, y);
            radius = r;
        }
        
        public WormAction GetNextAction(Position position)
        {
            var dir = previousAction._direction;

            if (ChangeDirectionConditions[dir](position, center, radius))
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