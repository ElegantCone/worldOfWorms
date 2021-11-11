using System;
using System.IO;
using Lab_1_worldOfWorms.Engine;
using Lab_1_worldOfWorms.Model;

namespace Lab_1_worldOfWorms.Model
{
    public class FoodGenerator : Component
    {
        private readonly Random _r = new Random();
        private Field _field;
        private const int Life = 10;

        public FoodGenerator(Field field)
        {
            _field = field;
        }
        public void GenerateFood()
        {
            Position pos;
            while (!_field.IsCellEmpty(pos = new Position(_r.NextNormal(_r.Next(0, 10)), _r.NextNormal(_r.NextNormal()*5)), true));
            var hc = InstantiateFood(pos);
            AddFoodToField(_field, hc);
        }

        public override void Update()
        {
            GenerateFood();
        }

        public static HealthController InstantiateFood(Position position)
        {
            var food = new GameObject();
            HealthController hc;
            food.GetComponent<Transform>().position = position;
            food.AddComponent(hc = new HealthController(Life));
            return hc;
        }

        public static void AddFoodToField(Field _field, HealthController hc)
        {
            hc.OnDeath += () => _field.RemoveFood(hc.gameObject);
            _field.foods.Add(hc.gameObject);
        }
    }
}