namespace WormsWorld.Engine
{
    public abstract class Component : IUpdatable
    {
        public GameObject GameObject
        {
            get;
            internal set;
        }
        
        public abstract void Update();
    }
}