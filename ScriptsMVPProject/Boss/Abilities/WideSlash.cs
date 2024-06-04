using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TollanWorlds.Abilities.BossAbilities
{
    public class WideSlash : Ability
    {
        #region Variables
        [SerializeField] Animator animator;
        [SerializeField] private bool _isSlashing = false;
        #endregion

        public bool IsSlashing
        {
            get
            {
                return _isSlashing;
            }
            set
            {
                _isSlashing = value;
                animator.SetBool("isSlashing", value);
            }
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Execute()
        {
            base.Execute();

            Slashing();
        }

        private async UniTask Slashing()
        {
            await WaitForExecutionAsync();
            IsSlashing = true;
            await WaitForCooldownAsync();
        }
    }
}
