using UnityEngine;
using TheKiwiCoder;
using System;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class Attack : ActionNode
    {
        public NodeProperty<GameObject> target;
        public NodeProperty<Boolean> hasTarget;
        public NodeProperty<float> attackRange;
        protected override void OnStart()
        {
        }

        protected override void OnStop()
        {
        }

        protected override State OnUpdate()
        {
            if(Vector3.Distance(context.transform.position, target.Value.transform.position) > attackRange.Value || !hasTarget.Value)
                return State.Success;

            context.attackController.PrimaryAttack(target.Value.transform.position);
            return State.Running;
        }
    }
}