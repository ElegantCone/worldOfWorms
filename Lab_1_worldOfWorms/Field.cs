using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace Lab_1_worldOfWorms
{
    public class Field : IUpdatable
    {
        
        private ObservableCollection<Worm> _worms = new();
        private Dictionary<Position,Food> _foods = new();

        private readonly IList<Worm> _wormsToRemove = new List<Worm>(); 

        public ObservableCollection<Worm> worms => _worms;

        public Action<Field> onFieldChanged = field => {};

        private StringBuilder wormsInfo;

        public Field()
        {
            _worms.CollectionChanged += (sender, args) =>
            {
                onFieldChanged?.Invoke(this);
            };
            wormsInfo = new StringBuilder();
        }

        public void Update()
        {
            wormsInfo.Clear();
            wormsInfo.Append("Worms: [");
            foreach (var worm in _worms)
            {
                worm.Update();
                wormsInfo.Append(worm.GetInfo());
            }
            wormsInfo.Append("]\n");
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

        public string GetInfo()
        {
            return wormsInfo.ToString();
        }
    }
}