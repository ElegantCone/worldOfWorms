using NUnit.Framework;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;
using WormsWorld.Services;

namespace WormsWorld.WormsWorld.Tests
{
    [TestFixture]
    public class WormAITest
    {
        private readonly Position _position = new (0, 0);
        private readonly Position _nearestFoodPosition = new(-1, -1);
        private readonly Position _foodPosition = new(10, 10);

        private const AIType _ai = AIType.Simple;

        private Worm _worm;

        private Simulator _simulator;
        private Field _field;
        
        [SetUp]
        public void SetUp()
        {
            _field = new Field();
            _simulator = new Simulator(_field, new FoodGenerator(_field),
                new WormBehaviourService(new NameGenerator(), _field), null);
            _simulator.AddWorm(_position.x, _position.y, _ai);
            _simulator.Update();
            _field.Update();
            _field.Foods.Clear();
            _worm = _simulator.Field.Worms[0];
            _simulator.FoodGenerator.GenerateFoodOnPosition(_nearestFoodPosition);
            _simulator.FoodGenerator.GenerateFoodOnPosition(_foodPosition);
        }
        
        [Test]
        public void MoveWormToNearestFoodTest()
        {
            _simulator.Update();
            var target = _worm.TargetFood.GetComponent<Transform>().Position;
            Assert.That(target, Is.EqualTo(_nearestFoodPosition));
        }
    }
}