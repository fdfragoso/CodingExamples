using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TollanWorlds.Movement;

namespace TollanWorlds.Abilities.BossAbilities
{
    public class Charge : Ability
    {
        #region Variables
        [SerializeField] Animator animator;
        [SerializeField] private bool _isCharging = false;
        #endregion

        public bool IsCharging
        {
            get
            {
                return _isCharging;
            }
            set
            {
                _isCharging = value;
                animator.SetBool("isCharging", value);
            }
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Execute()
        {
            base.Execute();
            
            Charging();
        }

        private async UniTask Charging()    
        {
            
            await WaitForExecutionAsync();
            IsCharging = true;
            await WaitForCooldownAsync();

        }
    }
}
