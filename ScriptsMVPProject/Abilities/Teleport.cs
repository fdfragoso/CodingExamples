using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
namespace TollanWorlds.Abilities
{
    /// <summary>
    /// Teleport Ability
    /// </summary>
    public class Teleport : Ability
    {
        #region Variables
        [SerializeField] private Transform _transform;
        [SerializeField] private float _startingDuration;
        [SerializeField] private float _absenceDuration;
        [SerializeField] private float _finishingDuration;
        [SerializeField] private UnityEvent _onAbsenceEnded;
        private Vector3 _target;
        #endregion

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();

        }
        #endregion

        #region Functions
        public void SetTarget(Vector3 target)
        {
            _target = target;
        }
        public override void Execute()
        {
            base.Execute();

            Teleporting();
        }
        public async UniTask TeleportAsync()
        {
            await UniTask.WaitForSeconds(_startingDuration);
            _transform.position = _target;
            await UniTask.WaitForSeconds(_absenceDuration);
            _onAbsenceEnded?.Invoke();
            await UniTask.WaitForSeconds(_finishingDuration);
        }

        private async UniTask Teleporting()
        {
            await TeleportAsync();

            StopExecution();
            await WaitForCooldownAsync();
        }
        #endregion
    }
}