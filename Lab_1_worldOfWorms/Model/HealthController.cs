using System;
using Lab_1_worldOfWorms.Engine;

namespace Lab_1_worldOfWorms.Model
{
    public class HealthController : Component
    {
        public Action OnDeath = () => { };
        
        private int _health = 10;

        public HealthController(int value)
        {
            _health = value;
            OnDeath += () => gameObject.Destroy();
        }

        public int health
        {
            get => _health;
            set
            {
                _health = value;
                if (health == 0)
                {
                    OnDeath?.Invoke();
                }
            }
        }
        
        public override void Update()
        {
            health--;
        }
    }
}