using UnityEngine;
using TheKiwiCoder;
using System;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class MoveToTarget : ActionNode
    {
        public NodeProperty<Vector3> target;
        public NodeProperty<Boolean> stopCondition;
        public NodeProperty<Vector3> direction;
        protected override void OnStart()
        {
            context.agent.isStopped = false;
            context.agent.SetDestination(target.Value);
        }

        protected override void OnStop()
        {
            context.agent.isStopped = true;
            context.movementController.Move(Vector2.zero);
        }

        protected override State OnUpdate()
        {
            context.movementController.Move(context.agent.desiredVelocity.normalized);

            if((context.agent.remainingDistance <= context.agent.stoppingDistance && !context.agent.pathPending) || stopCondition.Value)
                return State.Success;

            direction.Value = context.agent.desiredVelocity.normalized;

            return State.Running;
        }
    }
}