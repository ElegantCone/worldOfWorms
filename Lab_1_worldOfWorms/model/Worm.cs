using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Lab_1_worldOfWorms.ai;
using Lab_1_worldOfWorms.Engine;
using Lab_1_worldOfWorms.model;

namespace Lab_1_worldOfWorms
{
    public class Worm : Component
    {
        private string _name;

        private Field _field;

        private const int Life = 100;
        private int _childCount = 0;
        private const int ChildHealth = 10;
        private int radius;
        

        private IWormAI ai;

        private Dictionary<Direction, Action<Position>> ChangePosition;
        
        public void Initialize(string name, int x, int y, Field field, AIType type)
        {
            _name = name;
            gameObject.GetComponent<Transform>().position = new Position(x, y);
            _field = field;
            _field.worms.Add(this);
            HealthController hc;
            gameObject.AddComponent(hc = new HealthController(Life));
            hc.OnDeath += () => _field.RemoveWorm(this);

            switch (type)
            {
                case AIType.CIRCLE:
                    ai = new WormAIRound(x, y, 3);
                    break;
                case AIType.SIMPLE:
                    ai = new WormAISimple(this);
                    break;
                //todo: умный ИИ
            }

            ChangePosition = new()
            {
                {Direction.UP, (position) => position.y++},
                {Direction.DOWN, (position) => position.y--},
                {Direction.LEFT, (position) => position.x--},
                {Direction.RIGHT, (position) => position.x++},
                {Direction.NOTHING, (position) => {}}
            };


        }

        private void MakeAction()
        {
            var _position = gameObject.GetComponent<Transform>().position;
            var foods = new List<GameObject>(_field.foods.OrderBy(f => 
                Position.Distance(f.GetComponent<Transform>().position, _position)));
            WormAction wAction;
            if (foods.Count > 0)
            {
                wAction = ai.GetNextAction(_position, foods[0].GetComponent<Transform>());
            }
            else
            {
                wAction = ai.GetNextAction(_position);
            }
            if (wAction._decision == Decision.DO_NOTHING)
            {
                return;   
            }
            Position pos = new Position(_position.x, _position.y);
            ChangePosition[wAction._direction](pos);
            if (wAction._decision == Decision.MOVE)
            {
                if (_field.IsCellEmpty(pos))
                {
                    gameObject.GetComponent<Transform>().position = pos;
                }
            }
            if (wAction._decision == Decision.REPRODUCE)
            {
                if (_field.IsCellEmpty(pos))
                {
                    Reproduce(pos);
                    gameObject.GetComponent<HealthController>().health -= 10;
                }
            }

            GameObject foodToEat = null;
            foreach (var food in _field.foods)
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

        private void Reproduce(Position pos)
        {
            var wormGo = new GameObject();
            var worm = new Worm();
            wormGo.AddComponent(worm);
            worm.Initialize(GenerateName(), pos.x, pos.y, Simulator.instance._field, AIType.SIMPLE);
            worm.gameObject.GetComponent<HealthController>().health = ChildHealth;
            
        }
        

        private string GenerateName()
        {
            return $"{_name}#{_childCount++}";
        }


        public string GetInfo()
        {
            var _position = gameObject.GetComponent<Transform>().position;
            StringBuilder sb = new StringBuilder($"{_name}-{gameObject.GetComponent<HealthController>().health}, {_position}");
            return sb.ToString();
        }

        public override void Update()
        {
            MakeAction();
        }
        
    }
}