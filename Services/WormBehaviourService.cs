using System;
using System.Collections.Generic;
using System.Linq;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.Services
{
    public class WormBehaviourService : Component
    {
        private readonly Field _field;
        public readonly NameGenerator NameGenerator;
        private readonly Dictionary<Direction, Action<Position>> _changePosition;

        public WormBehaviourService(NameGenerator nameGenerator, Field field)
        {
            NameGenerator = nameGenerator;
            _field = field;
            _changePosition = new()
            {
                {Direction.Up, (position) => position.y++},
                {Direction.Down, (position) => position.y--},
                {Direction.Left, (position) => position.x--},
                {Direction.Right, (position) => position.x++},
                {Direction.Nothing, (position) => {}}
            };
        }

        private void PerformWormAction(Worm worm)
        {
            var go = worm.GameObject;
            var position = go.GetComponent<Transform>().Position;
            var foods = new List<GameObject>(_field.Foods.OrderBy(f => 
                Position.Distance(f.GetComponent<Transform>().Position, position)));
            WormAction wAction;
            if (foods.Count > 0)
            {
                worm.TargetFood = foods[0];
                wAction = worm.Ai.GetNextAction(position, foods[0].GetComponent<Transform>());
            }
            else
            {
                wAction = worm.Ai.GetNextAction(position);
            }
            if (wAction.Decision == Decision.DoNothing)
            {
                return;   
            }
            Position pos = new(position.x, position.y);
            _changePosition[wAction.Direction](pos);
            if (wAction.Decision == Decision.Move)
            {
                Move(pos, worm);
            }
            if (wAction.Decision == Decision.Reproduce)
            {
                Reproduce(worm, pos);
            }
        }

        public bool Move(Position position, Worm worm)
        {
            if (_field.IsCellEmpty(position))
            {
                worm.GameObject.GetComponent<Transform>().Position = position;
                EatFood(position, worm);
                return true;
            }
            return false;
        }

        private GameObject EatFood(Position position, Worm worm)
        {
            GameObject foodToEat = null;
            foreach (var food in _field.Foods)
            {
                if (food.GetComponent<Transform>().Position == position)
                {
                    foodToEat = food;
                    break;
                }
            }

            if (foodToEat != null)
            {
                foodToEat.GetComponent<HealthController>().health = 0;
                worm.GameObject.GetComponent<HealthController>().health += 10;
            }

            return foodToEat;
        }

        public bool Reproduce(Worm worm, Position pos)
        {
            if (_field.IsCellEmpty(pos, false, true) && worm.GameObject.GetComponent<HealthController>().health > 10)
            {
                var wormGo = new GameObject();
                var child = new Worm();
                wormGo.AddComponent(child);
                worm.ChildCount++;
                child.Initialize(pos.x, pos.y, AIType.Simple, NameGenerator.GenerateName(worm.Name, worm.ChildCount));
                child.GameObject.GetComponent<HealthController>().health = Worm.ChildHealth;
                _field.AddWorm(child); 
                EatFood(pos, worm);
                worm.GameObject.GetComponent<HealthController>().health -= 10;
                return true;
            }
            EatFood(pos, worm);
            return false;
        }

        public override void Update()
        {
            foreach (var worm in _field.Worms)
            {
                GameObject food = EatFood(worm.GameObject.GetComponent<Transform>().Position, worm);
                _field.Foods.Remove(food);
                PerformWormAction(worm);
            }
        }
    }
}