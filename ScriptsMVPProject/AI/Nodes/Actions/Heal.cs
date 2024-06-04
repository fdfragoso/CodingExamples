using TheKiwiCoder;
using Cysharp.Threading.Tasks;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class Heal : ActionNode
    {
        public NodeProperty<int> healingDuration;
        public NodeProperty<float> healingَAmount;
        private State _state;
        public NodeProperty<int> healingCount;
        protected override void OnStart()
        {
            _state = State.Running;
            StartHealing();
        }

        public async UniTask StartHealing()
        {
            await context.healthSystem.Heal(healingَAmount.Value, healingDuration.Value);
            healingCount.Value--;
            _state = State.Success;
        }
        protected override void OnStop()
        {
        }
        protected override State OnUpdate()
        {
            return _state;
        }
    }
}