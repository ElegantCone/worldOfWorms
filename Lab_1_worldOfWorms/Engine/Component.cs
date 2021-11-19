namespace Lab_1_worldOfWorms.Engine
{
    public abstract class Component : IUpdatable
    {
        public GameObject gameObject
        {
            get;
            internal set;
        }
        
        public abstract void Update();
    }
}