using System;
using System.Collections.Generic;
using NUnit.Framework;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;
using WormsWorld.Services;

namespace WormsWorld.WormsWorld.Tests
{
    [TestFixture]
    public class FoodGeneratorTest
    {
        private Field _field;
        private Simulator _simulator;
        private Random _r;
        private const int FoodCount = 50;
        private readonly Position _position = new(0, 0);
        
        [SetUp]
        public void SetUp()
        {
            _r = new Random();
            _field = new Field();
            _simulator = new Simulator(_field, new FoodGenerator(_field),
                new WormBehaviourService(new NameGenerator(), _field), null);
            _field.Foods.Clear();
            _simulator.Update();
        }

        [TestCase(FoodCount)]
        public void UniqueFoodPositionTest(int count)
        {
            for (int i = 0; i < count; i++)
            {
                _simulator.FoodGenerator.GenerateFood();
            }
            _field.Update();
            HashSet<Position> positions = new();
            bool isUnique = true;
            foreach (var food in _field.Foods)
            {
                isUnique &= positions.Add(food.GetComponent<Transform>().Position);
            }

            Assert.That(_field.Foods.Count, Is.EqualTo(count));
            Assert.True(isUnique);
            
            
        }

        [Test]
        public void GenerateFoodOnWormCell()
        {
            _simulator.AddWorm(_position.x, _position.y, AIType.Simple);
            _field.Update();
            Worm worm = _simulator.Field.Worms[0];
            _field.Update();
            _field.Foods.Clear();
            var isSuccessGeneration = _simulator.FoodGenerator.GenerateFoodOnPosition(_position);
            var health = worm.GameObject.GetComponent<HealthController>().health;
            var foodsCount = _field.Foods.Count;
            _simulator.WormBehaviourService.Update();
            
            Assert.That(health, Is.LessThan(worm.GameObject.GetComponent<HealthController>().health));
            Assert.That(foodsCount, Is.GreaterThan(_field.Foods.Count));
            Assert.True(isSuccessGeneration);
        }
    }
}