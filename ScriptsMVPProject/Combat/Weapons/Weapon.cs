using UnityEngine;
using UnityEngine.Events;

namespace TollanWorlds.Combat.Weapons
{
    /// <summary>
    /// Weapon base class for inheriting in any type of weapon classes.
    /// </summary>
    public class Weapon : MonoBehaviour, IWeapon
    {
        #region Variables
        [SerializeField] private float _damage;
        [SerializeField] private float _rate;
        [SerializeField] private float _range;
        [SerializeField] private UnityEvent _onAttack;
        [SerializeField] private UnityEvent _onHit;
        [SerializeField] private LayerMask _targetMask;

        [HideInInspector] public float LastAttack;
        public float Damage { get => _damage; set => _damage = value; }
        public float Rate { get => _rate; set => _rate = value; }
        public float Range { get => _range; set => _range = value; }
        public LayerMask TargetMask { get => _targetMask; set => _targetMask = value; }
        public UnityEvent OnAttack => _onAttack;
        public UnityEvent OnHit => _onHit;
        #endregion

        #region Functions

        /// <summary>
        /// If attack request will be more than weapon fire rate, it will return false.
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual bool Attack(Vector3 point)
        {
            var gap = Time.time - LastAttack;
            if(gap < 1 / Rate)
                return false;

            OnAttack?.Invoke();
            LastAttack = Time.time;
            return true;
        }
        #endregion
    }

    #region Interfaces
    public interface IWeapon
    {
        public float Damage { get; set; }
        public float Rate { get; set; }
        public float Range { get; set; }
        public LayerMask TargetMask { get; set; }
        public UnityEvent OnAttack { get; }
        public UnityEvent OnHit { get; }

        public bool Attack(Vector3 point);
    }
    #endregion
}