using TheKiwiCoder;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class SetState : ActionNode
    {
        public NodeProperty<int> stateTarget;
        public int value;
        protected override void OnStart()
        {
            stateTarget.Value = value;
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            return State.Success;
        }
    }
}