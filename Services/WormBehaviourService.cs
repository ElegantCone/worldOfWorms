using System;
using System.Collections.Generic;
using System.Linq;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.Services
{
    public class WormBehaviourService
    {
        public Field Field;
        public NameGenerator NameGenerator;
        private Dictionary<Direction, Action<Position>> ChangePosition;
        
        public WormBehaviourService(NameGenerator nameGenerator, Field field)
        {
            NameGenerator = nameGenerator;
            Field = field;
            ChangePosition = new()
            {
                {Direction.UP, (position) => position.y++},
                {Direction.DOWN, (position) => position.y--},
                {Direction.LEFT, (position) => position.x--},
                {Direction.RIGHT, (position) => position.x++},
                {Direction.NOTHING, (position) => {}}
            };
        }

        public void PerformWormAction(Worm worm)
        {
            var gameObject = worm.gameObject;
            var _position = gameObject.GetComponent<Transform>().position;
            var foods = new List<GameObject>(Field.foods.OrderBy(f => 
                Position.Distance(f.GetComponent<Transform>().position, _position)));
            WormAction wAction;
            if (foods.Count > 0)
            {
                wAction = worm.ai.GetNextAction(_position, foods[0].GetComponent<Transform>());
            }
            else
            {
                wAction = worm.ai.GetNextAction(_position);
            }
            if (wAction._decision == Decision.DO_NOTHING)
            {
                return;   
            }
            Position pos = new(_position.x, _position.y);
            ChangePosition[wAction._direction](pos);
            if (wAction._decision == Decision.MOVE)
            {
                if (Field.IsCellEmpty(pos))
                {
                    gameObject.GetComponent<Transform>().position = pos;
                }
            }
            if (wAction._decision == Decision.REPRODUCE)
            {
                if (Field.IsCellEmpty(pos))
                {
                    Reproduce(worm, pos);
                    gameObject.GetComponent<HealthController>().health -= 10;
                }
            }

            GameObject foodToEat = null;
            foreach (var food in Field.foods)
            {
                if (food.GetComponent<Transform>().position == _position)
                {
                    foodToEat = food;
                    break;
                }
            }

            if (foodToEat != null)
            {
                foodToEat.GetComponent<HealthController>().health = 0;
                gameObject.GetComponent<HealthController>().health += 10;
            }
        }
        
        private void Reproduce(Worm worm, Position pos)
        {
            var wormGo = new GameObject();
            var child = new Worm();
            wormGo.AddComponent(child);
            worm.ChildCount++;
            child.Initialize(pos.x, pos.y, AIType.SIMPLE, this, NameGenerator.GenerateName(worm.Name, worm.ChildCount));
            child.gameObject.GetComponent<HealthController>().health = Worm.ChildHealth;
        }
    }
}