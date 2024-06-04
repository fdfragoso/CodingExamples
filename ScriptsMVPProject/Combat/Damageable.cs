using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Combat
{
    /// <summary>
    /// Damageable Base Class For Inheriting In Any Object That's Damageable
    /// </summary>
    public class Damageable : MonoBehaviour, IDamageable
    {
        #region Variables
        private float _remainingHP;
        [SerializeField] private float _health;
        [SerializeField] private UnityEvent<float, float> _onDamaged;
        [SerializeField] private UnityEvent<float, float> _onHealed;
        [SerializeField] private UnityEvent<bool> _onHealing;
        [SerializeField] private UnityEvent<bool> _onFreezing;
        [SerializeField] private UnityEvent _onDeath;
        private bool _isFreezing;

        public float Health { get => _health; set => _health = value; }
        public UnityEvent<float, float> OnDamaged => _onDamaged;
        public UnityEvent<float, float> OnHealed => _onHealed;
        public UnityEvent<bool> OnHealing => _onHealing;
        public UnityEvent<bool> OnFreezing => _onFreezing;
        public UnityEvent OnDeath => _onDeath;
        public bool IsAlive => _remainingHP > 0;
        public bool IsFreezing { get => _isFreezing; set => _isFreezing = value; }
        public float RemainingPercentage => 100 * _remainingHP / Health;
        #endregion

        #region Monobehaviour Callbacks
        public virtual void Awake()
        {
            _remainingHP = Health;
        }
        #endregion

        #region Functions
        public virtual void Damage(float damage)
        {
            _remainingHP -= damage;
            OnDamaged?.Invoke(damage, _remainingHP);
            if(_remainingHP <= 0)
                Death();
        }

        public virtual void Freez(float duration)
        {
            if(!_isFreezing)
                Freezing(duration);
        }

        private async UniTask Freezing(float duration)
        {
            IsFreezing = true;
            OnFreezing?.Invoke(IsFreezing);
            await UniTask.WaitForSeconds(duration);
            IsFreezing = false;
            OnFreezing?.Invoke(IsFreezing);
        }

        public async UniTask Heal(float amount, float applyingDuration)
        {
            OnHealing?.Invoke(true);
            var remainingAmount = amount;
            var amountPerHalfSec = amount / applyingDuration;

            while(remainingAmount > 0)
            {
                var addingAmount = amountPerHalfSec;
                _remainingHP += addingAmount;
                remainingAmount -= addingAmount;
                OnHealed?.Invoke(addingAmount, _remainingHP);
                if(_remainingHP >= Health)
                    remainingAmount = 0;
                await UniTask.WaitForSeconds(1);
            }
            _remainingHP += amount;
            _remainingHP = Mathf.Clamp(_remainingHP, 0, Health);
            OnHealing?.Invoke(false);
        }

        public virtual void Death()
        {
            OnDeath?.Invoke();
        }
        #endregion
    }

    #region Interfaces
    public interface IDamageable
    {
        public float Health { get; set; }
        public float RemainingPercentage { get; }
        public bool IsAlive { get; }
        public bool IsFreezing { get; set; }
        public void Damage(float damage);
        public void Freez(float duration);
        public UniTask Heal(float amount, float applyingDuration);
        public UnityEvent<float, float> OnDamaged { get; }
        public UnityEvent<float, float> OnHealed { get; }
        public UnityEvent<bool> OnHealing { get; }
        public UnityEvent<bool> OnFreezing { get; }
        public UnityEvent OnDeath { get; }
    }
    #endregion 
}