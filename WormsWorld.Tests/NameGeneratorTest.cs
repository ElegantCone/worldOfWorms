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
    public class NameGeneratorTest
    {
        private readonly AIType _ai = AIType.Simple;

        private Field _field;
        private Simulator _simulator;
        private Random _r;
        
        [SetUp]
        public void SetUp()
        {
            _r = new Random();
            _field = new Field();
            _simulator = new Simulator(_field, new FoodGenerator(_field),
                new WormBehaviourService(new NameGenerator(), _field), null);
            _simulator.Update();
            for (int i = 0; i < 50; i++)
            {
                _simulator.AddWorm(_r.NextNormal(_r.Next(0, 10)),_r.NextNormal(_r.NextNormal() * 5), _ai);
            }
            _field.Update();
        }

        [Test]
        public void UniqueNamesTest()
        {
            var isUnique = true;
            HashSet<string> set = new();
            foreach (var worm in _field.Worms)
            {
                isUnique &= set.Add(worm.Name);
            }
            Assert.True(isUnique);
        }
        
    }
}