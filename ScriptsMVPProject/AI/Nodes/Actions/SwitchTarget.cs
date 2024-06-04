using UnityEngine;
using TheKiwiCoder;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class SwitchTarget : ActionNode
    {
        public NodeProperty<Vector3> firstPos;
        public NodeProperty<Vector3> secondPos;
        public NodeProperty<GameObject> secondPosTarget;
        public NodeProperty<Vector3> target;


        protected override void OnStart()
        {
            secondPos.Value = secondPosTarget.Value ? secondPosTarget.Value.transform.position : secondPos.Value;
            target.Value = target.Value == secondPos.Value ? firstPos.Value : secondPos.Value;
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