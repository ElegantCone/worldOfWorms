using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Lab_1_worldOfWorms.Engine;

namespace Lab_1_worldOfWorms
{
    public class Field : Component
    {
        
        private ObservableCollection<Worm> _worms = new();
        private ObservableCollection<GameObject> _foods = new();

        private readonly IList<Worm> _wormsToRemove = new List<Worm>(); 
        private readonly IList<GameObject> _foodsToRemove = new List<GameObject>(); 

        public ObservableCollection<Worm> worms => _worms;

        public ObservableCollection<GameObject> foods => _foods;

        public Action<Field> onFieldChanged = field => {};

        private StringBuilder wormsInfo;

        public Field()
        {
            _worms.CollectionChanged += (sender, args) =>
            {
                onFieldChanged?.Invoke(this);
            };
            
            _foods.CollectionChanged += (sender, args) =>
            {
                onFieldChanged?.Invoke(this);
            };
            
            wormsInfo = new StringBuilder();
        }

        public override void Update()
        {
            
            foreach (var worm in _wormsToRemove)
            {
                _worms.Remove(worm);
            }

            foreach (var food in _foodsToRemove)
            {
                _foods.Remove(food);
            }
            _foodsToRemove.Clear();
            _wormsToRemove.Clear();
        }

        public void RemoveWorm(Worm worm)
        {
            if (_worms.Contains(worm))
            {
                _wormsToRemove.Add(worm);
            }
        }
        

        public string GetInfo()
        {
            return wormsInfo.ToString();
        }

        public void RemoveFood(GameObject food)
        {
            if (_foods.Contains(food))
            {
                _foodsToRemove.Add(food);
            }
        }
        
        public bool IsCellEmpty(Position pos, bool isFood = false)
        {

            if (!isFood)
            {
                foreach (var worm in worms)
                {
                    if (worm.gameObject.GetComponent<Transform>().position == pos)
                        return false;
                }
            }

            if (isFood)
            {
                foreach (var food in foods)
                {
                    if (food.GetComponent<Transform>().position == pos) 
                        return false;
                }
            }

            return true;
        }
    }
}