using System;
using System.Collections.Generic;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.AI
{
    public class WormAISimple : IWormAI
    {
        private static readonly Dictionary<Direction, Func<Position, Position, bool>> ChangeDirectionConditions =
            new() {
                { Direction.UP, (wPosition, fPosition) => wPosition.y == fPosition.y },
                { Direction.DOWN, (wPosition, fPosition) => wPosition.y == fPosition.y},
                { Direction.LEFT, (wPosition, fPosition) => wPosition.x == fPosition.x},
                { Direction.RIGHT, (wPosition, fPosition) => wPosition.x == fPosition.x},
                { Direction.NOTHING, (wPosition, fPosition) => true}
            };
        
        private Transform fTransform;
        private Worm _worm;
        private Vector2d desiredPosition;

        private WormAction previousMove = new WormAction(Decision.MOVE, Direction.UP);
        private WormAction previousReproduce = null;

        public WormAISimple(Worm worm)
        {
            _worm = worm;
            desiredPosition = (Vector2d) worm.gameObject.GetComponent<Transform>().position;
        }


        public WormAction GetNextAction(Position wPosition, Transform food)
        {
            if (food == null) return new WormAction(Decision.DO_NOTHING, Direction.NOTHING);
            if (food != fTransform)
            {
                fTransform = food;
                desiredPosition = (Vector2d) _worm.gameObject.GetComponent<Transform>().position;
            }
            WormAction wormAction;
            Random r = new Random();
            float num = (float) r.NextDouble();
            if (num < 0.5)
            {
                return GetNextMove();
            }
            if (num < 0.99)
            {
                return _worm.gameObject.GetComponent<HealthController>().health <= 10 ? GetNextMove() : GetNextReproduce();
            }
            return new WormAction(Decision.DO_NOTHING, Direction.NOTHING);
        }

        private WormAction GetNextReproduce()
        {
            Direction dir;
            if (previousReproduce == null)
            {
                dir = GetRandomDirection();
            }
            else
            {
                var reprDir = previousReproduce._direction;
                while (reprDir == (dir = GetRandomDirection()));
            }

            return new WormAction(Decision.REPRODUCE, dir);

        }
        
        
        private Direction GetRandomDirection()
        {
            Random r = new Random();
            Array actions = Enum.GetValues(typeof(Direction));
            Direction dir;
            while ((dir = (Direction) actions.GetValue(r.Next(actions.Length))) == Direction.NOTHING) ;
            return dir;
        }
        
        private WormAction GetNextMove()
        {
            previousReproduce = null;
            return previousMove = new WormAction(Decision.MOVE, GetNextDirMove());
        }

        private Direction GetNextDirMove()
        {
            desiredPosition += ((Vector2d)fTransform.position - desiredPosition).Normalize();
            Position newPosition = (Position) (desiredPosition - (Vector2d)_worm.gameObject.GetComponent<Transform>().position);
            if (newPosition.x != 0)
            {
                return newPosition.x > 0 ? Direction.RIGHT : Direction.LEFT;
            }

            if (newPosition.y != 0)
            {
                return newPosition.y > 0 ? Direction.UP : Direction.DOWN;
            }

            return Direction.NOTHING;
        }
    }
}