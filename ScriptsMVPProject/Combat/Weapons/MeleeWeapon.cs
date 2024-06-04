using UnityEngine;

namespace TollanWorlds.Combat.Weapons
{
    /// <summary>
    /// For inheriting in any type of MeleeWeapon classes
    /// </summary>
    public class MeleeWeapon : Weapon
    {
        #region Variables
        [SerializeField] private float _angle;
        #endregion

        #region Functions
        public override bool Attack(Vector3 point)
        {
            if(!base.Attack(point))
                return false;

            Vector2 distance = point - transform.position;
            var overlapResult = Physics2D.OverlapCircleAll(transform.position, Range, TargetMask);
            CheckAndApplyDamage(overlapResult, distance.normalized);

            return true;
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
                        target.Damage(Damage);
                        OnHit?.Invoke();
                    }
                }
            }
        }
        #endregion
    }
}