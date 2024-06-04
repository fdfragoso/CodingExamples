using Cysharp.Threading.Tasks;
using System.Collections;
using TollanWorlds.Movement;
using UnityEngine;

namespace TollanWorlds.Abilities
{
    /// <summary>
    /// Dash Ability
    /// </summary>
    public class Dash : Ability
    {
        #region Variables
        [SerializeField] private float _dashSpeed;
        [SerializeField] private MovementController _movement;
        [SerializeField] private TrailRenderer _trail;
        #endregion

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();

            _trail.emitting = false;
        }
        #endregion

        #region Functions
        public override void Execute()
        {
            base.Execute();

            Dashing();
        }
        private async UniTask Dashing()
        {
            _trail.emitting = true;
            var baseSpeed = _movement.Speed;
            _movement.SetSpeed(baseSpeed * _dashSpeed);

            await WaitForExecutionAsync();

            _movement.SetSpeed(baseSpeed);
            StopExecution();
            _trail.emitting = false;

            await WaitForCooldownAsync();
        }
        #endregion
    }
}
