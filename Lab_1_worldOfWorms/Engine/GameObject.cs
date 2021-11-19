using System;
using System.Collections.Generic;

namespace Lab_1_worldOfWorms.Engine
{
    public class GameObject : IUpdatable
    {
        private Dictionary<Type, Component> _components = new();

        private List<Component> _componentsToRemove = new();

        private List<Component> _componentsToAdd = new();

        public GameObject()
        {
            AddComponent(new Transform());
            Update();
            Simulator.instance.AddGameObject(this);
        }

        public bool AddComponent<T>(T component) where T : Component
        {
            if (_components.ContainsKey(typeof(T))) return false;
            //_componentsToAdd.Add(component);
            _components.Add(component.GetType(), component);
            component.gameObject = this;
            return true;
        }

        public T GetComponent<T>() where T : Component
        {
            if (_components.TryGetValue(typeof(T), out var component)) return component as T;
            return null;
        }

        public bool RemoveComponent<T>() where T : Component
        {
            if (!_components.ContainsKey(typeof(T))) return false;
            //_componentsToRemove.Add(_components[typeof(T)]);
            _components.Remove(typeof(T));  
            return true;
        }

        public void Destroy()
        {
            Simulator.instance.RemoveGameObject(this);
        }
        
        public void Update()
        {
            var cache = new List<Component>(_components.Values);
            foreach (var component in cache)
            {
                component.Update();
            }

            /*foreach (var component in _componentsToRemove)
            {
                _components.Remove(component.GetType());   
            }

            foreach (var component in _componentsToAdd)
            {
                _components.Add(component.GetType(), component);
            }
            _componentsToRemove.Clear();
            _componentsToAdd.Clear();*/
        }
        
    }
}