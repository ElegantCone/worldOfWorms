using System;
using System.IO;
using WormsWorld.Model;
using WormsWorld.Engine;

namespace WormsWorld.Model
{
    public class FoodGenerator : Component
    {
        private readonly Random _r = new Random();
        private readonly Field _field;
        private const int Life = 10;

        public FoodGenerator(Field field)
        {
            _field = field;
        }
        public void GenerateFood()
        {
            Position pos;
            while (!_field.IsCellEmpty(
                pos = new Position(
                    _r.NextNormal(_r.Next(0, 10)), 
                    _r.NextNormal(_r.NextNormal() * 5)), 
                true));
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
            food.GetComponent<Transform>().Position = position;
            food.AddComponent(hc = new HealthController(Life));
            return hc;
        }

        public bool GenerateFoodOnPosition(Position position)
        {
            if (!_field.IsCellEmpty(position, true))
            {
                return false;
            }
            var hc = InstantiateFood(position);
            AddFoodToField(_field, hc);
            return true;
        }

        private static void AddFoodToField(Field field, HealthController hc)
        {
            hc.OnDeath += () => field.RemoveFood(hc.GameObject);
            field.Foods.Add(hc.GameObject);
        }
    }
}