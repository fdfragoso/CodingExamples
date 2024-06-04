using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TollanWorlds.Abilities.BossAbilities
{
    public class Apocalypse : Ability
    {
        #region Variables
        [SerializeField] Animator animator;
        [SerializeField] private bool _isApocalypse = false;
        #endregion

        public bool IsApocalypse
        {
            get
            {
                return _isApocalypse;
            }
            set
            {
                _isApocalypse = value;
                animator.SetBool("isApocalypse", value);
            }
        }

        public override void Awake()
        {
            base.Awake();
        }

        public override void Execute()
        {
            base.Execute();

            ApocalypseStart();
        }

        private async UniTask ApocalypseStart()
        {

            await WaitForExecutionAsync();
            IsApocalypse = true;
            await WaitForCooldownAsync();

        }
    }
}