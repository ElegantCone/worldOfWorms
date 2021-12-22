using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using WormsWorld.Engine;

namespace WormsWorld.Model
{
    public class Field : Component
    {
        private readonly ObservableCollection<Worm> _worms = new();
        private readonly ObservableCollection<GameObject> _foods = new();

        private readonly IList<Worm> _wormsToRemove = new List<Worm>(); 
        private readonly IList<GameObject> _foodsToRemove = new List<GameObject>();
        private readonly IList<Worm> _wormsToAdd = new List<Worm>();

        public ObservableCollection<Worm> Worms => _worms;

        public ObservableCollection<GameObject> Foods => _foods;

        private readonly Action<Field> _onFieldChanged = field => {};
        

        public Field()
        {
            _worms.CollectionChanged += (sender, args) =>
            {
                _onFieldChanged?.Invoke(this);
            };
            
            _foods.CollectionChanged += (sender, args) =>
            {
                _onFieldChanged?.Invoke(this);
            };
        }

        public override void Update()
        {

            foreach (var worm in _wormsToRemove)
            {
                _worms.Remove(worm);
            }

            foreach (var worm in _wormsToAdd)
            {
                _worms.Add(worm);
            }

            foreach (var food in _foodsToRemove)
            {
                _foods.Remove(food);
            }
            _foodsToRemove.Clear();
            _wormsToRemove.Clear();
            _wormsToAdd.Clear();
        }

        public void AddWorm(Worm worm)
        {
            worm.GameObject.GetComponent<HealthController>().OnDeath += () => RemoveWorm(worm);
            _wormsToAdd.Add(worm);
        }


        private void RemoveWorm(Worm worm)
        {
            if (_worms.Contains(worm))
            {
                _wormsToRemove.Add(worm);
            }
        }

        public void RemoveFood(GameObject food)
        {
            if (_foods.Contains(food))
            {
                _foodsToRemove.Add(food);
            }
        }
        
        public bool IsCellEmpty(Position pos, bool isFood = false, bool isReproduce = false)
        {
            if (!isFood){
                foreach (var worm in Worms)
                {
                    if (worm.GameObject.GetComponent<Transform>().Position == pos)
                         return false;
                }
                
            }
            
            if (isReproduce)
            {
                isFood = true;
            }

            if (isFood)
            {
                foreach (var food in Foods)
                {
                    if (food.GetComponent<Transform>().Position == pos) 
                        return false;
                }
            }

            return true;
        }
    }
}