using Cysharp.Threading.Tasks;
using TollanWorlds.Combat;
using UnityEngine;
using UnityEngine.Events;
namespace TollanWorlds.Abilities
{
    /// <summary>
    /// WideSlash Ability
    /// </summary>
    public class WideSlash : Ability
    {
        #region Variables
        [SerializeField] private float _damage;
        [SerializeField] private float _angle;
        [SerializeField] private float _range;
        [SerializeField] private UnityEvent _onHit;
        [SerializeField] private LayerMask _targetMask;
        private Vector3 _target;
        #endregion

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();
            SetTarget(transform.position + Vector3.right);
        }
        #endregion

        #region Functions
        public void SetTarget(Vector3 target)
        {
            _target = target;
        }
        public void Slash(Vector3 point)
        {
            Vector2 distance = point - transform.position;
            var overlapResult = Physics2D.OverlapCircleAll(transform.position, _range, _targetMask);
            CheckAndApplyDamage(overlapResult, distance.normalized);
        }

        private void CheckAndApplyDamage(Collider2D[] overlapHits, Vector2 direction)
        {
            foreach(var hit in overlapHits)
            {
                Vector2 distanceTarget = hit.transform.position - transform.position;

                if(Vector2.Angle(distanceTarget.normalized, direction) < _angle / 2)
                {
                    IDamageable target;
                    if(hit.TryGetComponent(out target))
                    {
                        target.Damage(_damage);
                        _onHit?.Invoke();
                    }
                }
            }
        }
        public override void Execute()
        {
            base.Execute();

            Slashing();
        }
        private async UniTask Slashing()
        {
            Slash(_target);
            await WaitForExecutionAsync();
            StopExecution();
            await WaitForCooldownAsync();
        }
        #endregion
    }
}