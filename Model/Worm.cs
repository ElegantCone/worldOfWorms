using System.Text;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Services;

namespace WormsWorld.Model
{
    public class Worm : Component
    {
        public string Name;
        public GameObject TargetFood;
        
        private const int Life = 100;
        public int ChildCount;
        public const int ChildHealth = 10;
        
        public IWormAI Ai;
        private WormBehaviourService _wormBehaviourService;

        public void Initialize(int x, int y, AIType type, string name)
        {
            Name = name;
            GameObject.GetComponent<Transform>().Position = new Position(x, y);
            GameObject.AddComponent(new HealthController(Life));

            switch (type)
            {
                case AIType.Circle:
                    Ai = new WormAIRound(x, y, 3);
                    break;
                case AIType.Simple:
                    Ai = new WormAiSimple(this);
                    break;
                //todo: умный ИИ
            }
        }

        public string GetInfo()
        {
            var position = GameObject.GetComponent<Transform>().Position;
            StringBuilder sb = new($"{Name}-{GameObject.GetComponent<HealthController>().health}, {position}");
            return sb.ToString();
        }

        public override void Update()
        {
        }
        
    }
}