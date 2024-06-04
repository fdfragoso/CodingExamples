using PixelCrushers;
using TollanWorlds.Utility;
using UnityEngine;

namespace TollanWorlds.Combat.Weapons
{
    /// <summary>
    ///  For inheriting in any type of RangeWeapon classes.
    /// </summary>
    public class RangeWeapon : Weapon
    {
        #region Variables
        [SerializeField] private Bullet _bulletPrefab;
        [SerializeField] private float _bulletSpeed;
        [SerializeField] private Transform _shootOrigin;
        private ObjectPool<Bullet> _bulletPool;
        #endregion

        #region Monobehaviour Callbacks
        private void Start()
        {
            _bulletPool = new ObjectPool<Bullet>(_bulletPrefab,5,gameObject.name+"-Bullet Pool");
        }
        #endregion

        #region Functions
        public override bool Attack(Vector3 point)
        {
            if(!base.Attack(point))
                return false;

            Vector2 distance = point - transform.position;
            var bullet = _bulletPool.Renew(_shootOrigin.position, Quaternion.identity);
            bullet.Init(this, distance.normalized, _bulletSpeed);

            return true;
        }
        #endregion
    }
}