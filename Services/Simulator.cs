
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Model;
using WormsWorld.Services;

namespace WormsWorld
{
    public class Simulator : IUpdatable
    {
        public static Simulator instance
        {
            get;
            private set;
        }

        private List<GameObject>  _gameObjectsToAdd, _gameObjectsToRemove;
        private HashSet<GameObject> _gameObjects;
        private WormBehaviourService _wormBehaviourService;

        public Field _field;

        public Simulator(Field field, FoodGenerator foodGenerator, Logger logger, WormBehaviourService wormBehaviourService)
        {
            instance = this;
            _gameObjects = new();
            _gameObjectsToAdd = new();
            _gameObjectsToRemove = new();
            _wormBehaviourService = wormBehaviourService;
            GameObject go = new GameObject();
            
            go.AddComponent(_field = field);
            go.AddComponent(foodGenerator);
            go.AddComponent(logger);
        }
        
        
        private void AddWorm(int x, int y, AIType ai)
        {
            var wormGo = new GameObject();
            var worm = new Worm();
            wormGo.AddComponent(worm);
            worm.Initialize(x, y, ai, _wormBehaviourService);
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
            AddWorm(0, 0, AIType.SIMPLE);
            AddWorm(1, 1, AIType.SIMPLE);
            
            while (_field.worms.Any())
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