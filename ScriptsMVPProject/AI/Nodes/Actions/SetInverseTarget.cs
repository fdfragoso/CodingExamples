using UnityEngine;
using TheKiwiCoder;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class SetInverseTarget : ActionNode
    {
        public NodeProperty<Vector3> target;
        public NodeProperty<Vector3> lastDirection;
        public NodeProperty<float> distance;

        protected override void OnStart()
        {
            target.Value = context.transform.position - lastDirection.Value * distance.Value;
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