using Cysharp.Threading.Tasks;
using TollanWorlds.Combat;
using TollanWorlds.Utility;
using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Abilities
{
    /// <summary>
    /// Charge Ability
    /// </summary>

    public class Charge : Ability
    {
        #region Variables
        [SerializeField] private float _speed;
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider;
        private Vector3 _direction;
        private bool _isCharging;
        public UnityEvent<Damageable> OnHitDamageable;
        public UnityEvent OnHitWall;
        #endregion

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();

        }
        #endregion

        #region Functions

        public void SetTarget(Vector3 dir)
        {
            _direction = dir;
        }
        public override void Execute()
        {
            base.Execute();
            Charging();
        }
        private async UniTask Charging()
        {
             _isCharging = true;
            _collider.enabled = true;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;
            while(_isCharging)
            {
                await UniTask.WaitForFixedUpdate();
                _rigidbody.velocity = _direction * _speed * Time.deltaTime;
            }

            _rigidbody.velocity = Vector3.zero;
            _collider.enabled = false;
            _rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;

            StopExecution();
            await WaitForCooldownAsync();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            _isCharging = false;

            Damageable damagable;
            if(other.gameObject.TryGetComponent(out damagable))
                OnHitDamageable?.Invoke(damagable);
            else
                OnHitWall?.Invoke();
        }
        #endregion
    }
}