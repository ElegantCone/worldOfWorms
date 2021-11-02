using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Lab_1_worldOfWorms
{
    public class Field : IUpdatable
    {
        private ObservableCollection<Worm> _worms = new();
        private Dictionary<Position,Food> _foods = new();

        private readonly IList<Worm> _wormsToRemove = new List<Worm>(); 

        public ObservableCollection<Worm> worms => _worms;

        public Action<Field> onFieldChanged = field => {};

        public Field()
        {
            _worms.CollectionChanged += (sender, args) =>
            {
                onFieldChanged?.Invoke(this);
            };
        }

        public void Update()
        {
            Console.Write("Worms: [");
            foreach (var worm in _worms)
            {
                worm.Update();
                Console.Write(worm.GetInfo());
            }
            Console.Write("]\n");
            foreach (var worm in _wormsToRemove)
            {
                _worms.Remove(worm);
            }
            
            _wormsToRemove.Clear();
        }

        public void RemoveWorm(Worm worm)
        {
            if (_worms.Contains(worm))
            {
                _wormsToRemove.Add(worm);
            }
        }
    }
}