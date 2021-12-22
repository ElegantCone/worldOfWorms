using NUnit.Framework;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;
using WormsWorld.Services;

namespace WormsWorld.WormsWorld.Tests
{
    [TestFixture]
    public class WormReproducementTest
    {
        private readonly Position _position = new (0, 0);
        private readonly AIType _ai = AIType.Simple;
        private readonly Position _reproducePositionEmpty = new(1, 0);
        private readonly Position _reproducePositionFood = new(0, 1);
        private readonly Position _reproducePositionWorm = new(0, -1);
        private Worm _worm;
        private Worm _worm2;

        private Field _field;
        private Simulator _simulator;

        [SetUp]
        public void SetUp()
        {
            _field = new Field();
            _simulator = new Simulator(_field, new FoodGenerator(_field),
                new WormBehaviourService(new NameGenerator(), _field), null);
            _simulator.AddWorm(_position.x, _position.y, _ai);
            _simulator.AddWorm(_reproducePositionWorm.x, _reproducePositionWorm.y, _ai);
            _simulator.Update();
            _field.Update();
            _worm = _simulator.Field.Worms[0];
            _worm2 = _simulator.Field.Worms[1];
            _field.Foods.Clear();
            _simulator.FoodGenerator.GenerateFoodOnPosition(_reproducePositionFood);
        }
        
        [Test]
        public void ReproduceOnEmptyCell()
        {
            var health = _worm.GameObject.GetComponent<HealthController>().health;
            var childCount = _worm.ChildCount;
            var isReproducementSuccessfull = _simulator.WormBehaviourService.Reproduce(_worm, _reproducePositionEmpty);
            Assert.True(isReproducementSuccessfull);
            Assert.That(_worm.GameObject.GetComponent<HealthController>().health, Is.LessThan(health));
            Assert.That(_worm.ChildCount, Is.GreaterThan(childCount));
        }
        
        [Test]
        public void ReproduceOnFood()
        {
            _simulator.FoodGenerator.GenerateFoodOnPosition(_reproducePositionFood);
            _field.Update();
            var isReproducementSuccessfull = _simulator.WormBehaviourService.Reproduce(_worm, _reproducePositionFood);
            Assert.False(isReproducementSuccessfull);
        }
        
        [Test]
        public void ReproduceOnWorm()
        {
            bool isReproducementSuccessfull = _simulator.WormBehaviourService.Reproduce(_worm, _reproducePositionWorm);
            Assert.False(isReproducementSuccessfull);
        }
        
    }
}