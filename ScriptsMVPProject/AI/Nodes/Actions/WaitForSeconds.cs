using TheKiwiCoder;
using Cysharp.Threading.Tasks;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class WaitForSeconds : ActionNode
    {
        public NodeProperty<float> duration;

        private State _currentState;
        protected override void OnStart()
        {
            _currentState = State.Running;
            Wait();
        }

        private async UniTask Wait()
        {
            await UniTask.WaitForSeconds(duration.Value);
            _currentState = State.Success;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return _currentState;
        }
    }
}