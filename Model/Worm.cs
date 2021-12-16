using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Hosting;
using WormsWorld.AI;
using WormsWorld.Engine;
using WormsWorld.Services;

namespace WormsWorld.Model
{
    public class Worm : Component
    {
        public string Name;
        
        private const int Life = 100;
        public int ChildCount;
        public const int ChildHealth = 10;
        
        public IWormAI ai;
        private WormBehaviourService _wormBehaviourService;

        public void Initialize(int x, int y, AIType type, WormBehaviourService wbs)
        {
            Initialize(x, y, type, wbs, wbs.NameGenerator.GenerateName());
        }
        
        public void Initialize(int x, int y, AIType type, WormBehaviourService wbs, string name)
        {
            _wormBehaviourService = wbs;
            Name = name;
            gameObject.GetComponent<Transform>().position = new Position(x, y);
            wbs.Field.worms.Add(this);
            HealthController hc;
            gameObject.AddComponent(hc = new HealthController(Life));
            hc.OnDeath += () => wbs.Field.RemoveWorm(this);

            switch (type)
            {
                case AIType.CIRCLE:
                    ai = new WormAIRound(x, y, 3);
                    break;
                case AIType.SIMPLE:
                    ai = new WormAISimple(this);
                    break;
                //todo: умный ИИ
            }
        }

        private void MakeAction()
        {
            _wormBehaviourService.PerformWormAction(this);
        }

        public string GetInfo()
        {
            var _position = gameObject.GetComponent<Transform>().position;
            StringBuilder sb = new StringBuilder($"{Name}-{gameObject.GetComponent<HealthController>().health}, {_position}");
            return sb.ToString();
        }

        public override void Update()
        {
            MakeAction();
        }
        
    }
}