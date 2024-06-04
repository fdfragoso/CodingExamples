using UnityEngine;
using TheKiwiCoder;
using Cysharp.Threading.Tasks;
using System;

namespace TollanWorlds.AI.Nodes
{
    [System.Serializable]
    public class EnemyStateUpdate : ActionNode
    {
        public NodeProperty<int> enemyState;
        public NodeProperty<int> healingCount;
        public NodeProperty<Boolean> randomWalk;
        public NodeProperty<int> walkingState;
        public NodeProperty<Vector3> firstPos;
        public NodeProperty<float> scanRange;
        public NodeProperty<LayerMask> scanMask;
        public NodeProperty<GameObject> enemyTarget;
        public NodeProperty<Boolean> hasEnemyTarget;
        public int _scanRate;

        protected override void OnStart()
        {
            firstPos.Value = firstPos.Value != Vector3.zero ? firstPos.Value : context.transform.position;
            if(!randomWalk.Value)
                walkingState.Value = 1;

            ScanUpdate();
        }

        protected override void OnStop()
        {
        }
        private async UniTask ScanUpdate()
        {
            while(context.transform)
            {
                var scan = Scan();
                enemyTarget.Value = scan ? scan : enemyTarget.Value;
                hasEnemyTarget.Value = scan;
                await UniTask.WaitForSeconds(1 / _scanRate);
            }
        }

        public GameObject Scan()
        {
            var overlapResult = Physics2D.OverlapCircle(context.transform.position, scanRange.Value, scanMask.Value);

            if(overlapResult != null && overlapResult.tag == "Player")
                return overlapResult.gameObject;
            else
                return null;
        }

        protected override State OnUpdate()
        {
            if(hasEnemyTarget.Value && enemyState.Value!=3)
            {
                if(healingCount.Value > 0 && context.healthSystem.RemainingPercentage <= 50)
                    enemyState.Value = 3;
                else
                    enemyState.Value = 1;
            }
            else if(enemyState.Value == 1)
                enemyState.Value = 2;

            return State.Running;
        }
    }
}