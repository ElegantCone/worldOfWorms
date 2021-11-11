
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Lab_1_worldOfWorms.Engine;
using Lab_1_worldOfWorms.Model;

namespace Lab_1_worldOfWorms
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


        public Field _field;
        private StreamWriter output;

        public Simulator()
        {
            instance = this;
            _gameObjects = new();
            _gameObjectsToAdd = new();
            _gameObjectsToRemove = new();
            GameObject go = new GameObject();
            go.AddComponent(_field = new Field());
            go.AddComponent(new FoodGenerator(_field));
            go.AddComponent(new Logger("output.txt", _field));
            //var food = FoodGenerator.InstantiateFood(new Position(3, 5));
            //FoodGenerator.AddFoodToField(_field, food);
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