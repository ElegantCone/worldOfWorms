using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Internal.Commands;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;
using WormsWorld.Services;

namespace WormsWorld.WormsWorld.Tests
{
    [TestFixture]
    public class WormMovementTest
    {
        private readonly Position _position = new (0, 0);
        private readonly Position _position2 = new (0, -1);
        private readonly AIType _ai = AIType.Simple;

        private Worm _worm;
        private Worm _worm2;

        private const int EmptyX = 0;
        private const int EmptyY = 1;

        private const int FoodX = 1;
        private const int FoodY = 0;


        private Simulator _simulator;
        private Field _field;
        
        
        [SetUp]
        public void SetUp()
        {
            _field = new Field();

            _simulator = new Simulator(_field, new FoodGenerator(_field),
                new WormBehaviourService(new NameGenerator(), _field), null);
            _simulator.AddWorm(_position.x, _position.y, _ai);
            _simulator.AddWorm(_position2.x, _position2.y, _ai);
            _simulator.Update();
            _field.Update();
            _field.Foods.Clear();
            _worm = _simulator.Field.Worms[0];
            _worm2 = _simulator.Field.Worms[1];
        }

        
        [TestCase(EmptyX, EmptyY)]
        public void MoveWormOnEmptyCellTest(int x, int y)
        {
            _simulator.WormBehaviourService.Move(new Position(x, y), _worm);
            Position newPosition = _simulator.Field.Worms[0].GameObject.GetComponent<Transform>().Position;
            Assert.That(x, Is.EqualTo(newPosition.x));
            Assert.That(y, Is.EqualTo(newPosition.y));
        }

        [TestCase(FoodX, FoodY)]
        public void MoveWormOnFoodTest(int x, int y)
        {
            _simulator.FoodGenerator.GenerateFoodOnPosition(new Position(x, y));
            var health = _worm.GameObject.GetComponent<HealthController>().health + 1;
            var foodCount = _field.Foods.Count + 1;
            _simulator.WormBehaviourService.Move(new Position(x, y), _worm);
            Position newPosition = _simulator.Field.Worms[0].GameObject.GetComponent<Transform>().Position;
            Assert.That(x, Is.EqualTo(newPosition.x));
            Assert.That(y, Is.EqualTo(newPosition.y));
            
            Assert.That(health, Is.LessThan(_worm.GameObject.GetComponent<HealthController>().health));
            Assert.That(_simulator.Field.Foods.Count, Is.LessThan(foodCount));
        }

        [Test]
        public void MoveWormOnAnotherWormTest()
        {
            Position position = _worm2.GameObject.GetComponent<Transform>().Position;
            var isMovementSuccessfull = _simulator.WormBehaviourService.Move(position, _worm);
            Assert.False(isMovementSuccessfull);
        }
        
    }
}