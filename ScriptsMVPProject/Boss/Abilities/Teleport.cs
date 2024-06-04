using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TollanWorlds.Abilities.BossAbilities
{
    public class Teleport : Ability
    {
        #region Variables
        [SerializeField] Animator animator;
        [SerializeField] private bool _isTeleporting = false;
        [SerializeField] private Transform toPoint;
        #endregion

        public bool IsTeleporting
        {
            get
            {
                return _isTeleporting;
            }
            set
            {
                _isTeleporting = value;
                animator.SetBool("isTeleporting", value);
            }
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Execute()
        {
            base.Execute();

            //Teleporting();
            TeleportTo(toPoint.localPosition);
        }

        private async UniTask Teleporting()
        {

            await WaitForExecutionAsync();
            IsTeleporting = true;
            await WaitForCooldownAsync();

        }

        /// <summary>
        /// Teleport the boss to some position
        /// </summary>
        /// <param name="to">The position to teleport</param>
        /// <param name="from">The actual position</param>
        /// <returns></returns>
        private async UniTask TeleportTo(Vector3 to)
        {
            Vector3 from = this.transform.localPosition;

            await WaitForExecutionAsync();

            IsTeleporting = true;
            this.transform.parent.parent.localPosition = to;

            Debug.Log(to + "     " + from);
            
            await WaitForCooldownAsync();
            
        }

        /// <summary>
        /// The character is teleported to a position close to the player
        /// A radius from the player
        /// </summary>
        /// <param name="distanceToPlayer">The distance from player position to the char new position</param>
        /// <returns></returns>
        private async UniTask TeleportToPlayer(float distanceToPlayer)
        {

        }
    }
}
