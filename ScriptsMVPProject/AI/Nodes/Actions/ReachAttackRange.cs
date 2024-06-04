using UnityEngine;
using TheKiwiCoder;
using System;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class ReachAttackRange : ActionNode
    {
        public NodeProperty<float> attackRange;
        public NodeProperty<GameObject> target;
        public NodeProperty<Boolean> hasTarget;
        public NodeProperty<Vector3> direction;

        protected override void OnStart()
        {
            context.agent.isStopped = false;
            context.agent.SetDestination(target.Value.transform.position);
        }

        protected override void OnStop()
        {
            context.agent.isStopped = true;
            context.movementController.Move(Vector2.zero);
        }

        protected override State OnUpdate()
        {
            context.agent.SetDestination(target.Value.transform.position);

            context.movementController.Move(context.agent.desiredVelocity.normalized);

            if(Vector3.Distance(context.transform.position, target.Value.transform.position) <= attackRange.Value || !hasTarget.Value)
                return State.Success;

            direction.Value = context.agent.desiredVelocity.normalized;

            return State.Running;
        }
    }
}