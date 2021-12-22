using System.Collections.Generic;
using System.Linq;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;

namespace WormsWorld.Services
{
    public class Simulator : IUpdatable
    {
        public static Simulator instance
        {
            get;
            private set;
        }

        private readonly List<GameObject>  _gameObjectsToAdd;
        private readonly List<GameObject>  _gameObjectsToRemove;
        private readonly HashSet<GameObject> _gameObjects;
        public readonly WormBehaviourService WormBehaviourService;
        public readonly FoodGenerator FoodGenerator;
        public readonly Field Field;

        public Simulator(Field field, FoodGenerator foodGenerator, WormBehaviourService wormBehaviourService, Logger? logger)
        {
            instance = this;
            _gameObjects = new();
            _gameObjectsToAdd = new();
            _gameObjectsToRemove = new();
            WormBehaviourService = wormBehaviourService;
            FoodGenerator = foodGenerator;
            GameObject go = new();
            
            go.AddComponent(Field = field);
            if (logger != null)
            {
                go.AddComponent(logger);   
            }
            go.AddComponent(FoodGenerator);
            go.AddComponent(wormBehaviourService);
        }
        
        
        
        public void AddWorm(int x, int y, AIType ai)
        {
            var wormGo = new GameObject();
            var worm = new Worm();
            wormGo.AddComponent(worm);
            worm.Initialize(x, y, ai, WormBehaviourService.NameGenerator.GenerateName());
            Field.AddWorm(worm);
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            _gameObjectsToRemove.Add(gameObject);
        }

        public void AddGameObject(GameObject gameObject)
        {
            _gameObjectsToAdd.Add(gameObject);
        }
        
        public void StartGame()
        {
            AddWorm(0, 0, AIType.Simple);
            Field.Update();
            while (Field.Worms.Any())
            {
                Update();
            }  
        }

        public void Update()
        {
            foreach (var gameObject in _gameObjects)
            {
                gameObject.Update();
            }

            foreach (var gameObject in _gameObjectsToAdd)
            {
                _gameObjects.Add(gameObject);
            }

            foreach (var gameObject in _gameObjectsToRemove)
            {
                _gameObjects.Remove(gameObject);
            }
            _gameObjectsToAdd.Clear();
            _gameObjectsToRemove.Clear();
        }
        
    }
    
    
}