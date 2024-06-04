using TheKiwiCoder;
using Cysharp.Threading.Tasks;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class WaitForFrames : ActionNode
    {
        public int _frameCount;
        private State _state;
        protected override void OnStart()
        {
            _state = State.Running;
            Wait();
        }

        private async UniTask Wait()
        {
            for(int i = 0; i < _frameCount; i++)
                await UniTask.WaitForEndOfFrame();
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