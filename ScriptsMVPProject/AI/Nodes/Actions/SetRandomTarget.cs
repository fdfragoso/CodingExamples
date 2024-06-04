using UnityEngine;
using TheKiwiCoder;
using TollanWorlds.Utility;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class SetRandomTarget : ActionNode
    {
        public NodeProperty<float> radius;
        public NodeProperty<Vector3> target;
        public NodeProperty<Vector3> firstPos;
        protected override void OnStart()
        {
            target.Value = NavMeshHelper.GetRandomPointInCircle(firstPos.Value,radius.Value);
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