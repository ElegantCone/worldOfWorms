using System;
using System.Collections.Generic;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.AI
{
    public class WormAiSimple : IWormAI
    {
        private static readonly Dictionary<Direction, Func<Position, Position, bool>> ChangeDirectionConditions =
            new() {
                { Direction.Up, (wPosition, fPosition) => wPosition.y == fPosition.y },
                { Direction.Down, (wPosition, fPosition) => wPosition.y == fPosition.y},
                { Direction.Left, (wPosition, fPosition) => wPosition.x == fPosition.x},
                { Direction.Right, (wPosition, fPosition) => wPosition.x == fPosition.x},
                { Direction.Nothing, (wPosition, fPosition) => true}
            };
        
        private Transform _fTransform;
        private readonly Worm _worm;
        private Vector2d _desiredPosition;
        public AIType Type = AIType.Simple;

        private WormAction _previousMove = new WormAction(Decision.Move, Direction.Up);
        private WormAction _previousReproduce = null;

        public WormAiSimple(Worm worm)
        {
            _worm = worm;
            _desiredPosition = (Vector2d) worm.GameObject.GetComponent<Transform>().Position;
        }
        

        public WormAction GetNextAction(Position wPosition, Transform food)
        {
            if (food == null) return new WormAction(Decision.DoNothing, Direction.Nothing);
            if (food != _fTransform)
            {
                _fTransform = food;
                _desiredPosition = (Vector2d) _worm.GameObject.GetComponent<Transform>().Position;
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
                return _worm.GameObject.GetComponent<HealthController>().health <= 10 ? GetNextMove() : GetNextReproduce();
            }
            return new WormAction(Decision.DoNothing, Direction.Nothing);
        }

        private WormAction GetNextReproduce()
        {
            Direction dir;
            if (_previousReproduce == null)
            {
                dir = GetRandomDirection();
            }
            else
            {
                var reprDir = _previousReproduce.Direction;
                while (reprDir == (dir = GetRandomDirection()));
            }

            return new WormAction(Decision.Reproduce, dir);

        }
        
        
        private Direction GetRandomDirection()
        {
            Random r = new Random();
            Array actions = Enum.GetValues(typeof(Direction));
            Direction dir;
            while ((dir = (Direction) actions.GetValue(r.Next(actions.Length))) == Direction.Nothing) ;
            return dir;
        }
        
        private WormAction GetNextMove()
        {
            _previousReproduce = null;
            return _previousMove = new WormAction(Decision.Move, GetNextDirMove());
        }

        private Direction GetNextDirMove()
        {
            _desiredPosition += ((Vector2d)_fTransform.Position - _desiredPosition).Normalize();
            Position newPosition = (Position) (_desiredPosition - (Vector2d)_worm.GameObject.GetComponent<Transform>().Position);
            if (newPosition.x != 0)
            {
                return newPosition.x > 0 ? Direction.Right : Direction.Left;
            }

            if (newPosition.y != 0)
            {
                return newPosition.y > 0 ? Direction.Up : Direction.Down;
            }

            return Direction.Nothing;
        }
    }
}