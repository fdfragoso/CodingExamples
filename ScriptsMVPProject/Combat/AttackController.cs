using TollanWorlds.Combat.Weapons;
using UnityEngine;

namespace TollanWorlds.Combat
{
    public class AttackController : MonoBehaviour
    {
        #region Variables
        [HideInInspector] public bool IsEnabled = true;

        [SerializeField] private Weapon _primaryWeapon;
        [SerializeField] private Weapon _secondaryWeapon;
        #endregion

        #region Functions
        public void PrimaryAttack(Vector3 point) => _primaryWeapon?.Attack(point);
        public void SecondaryAttack(Vector3 point) => _secondaryWeapon?.Attack(point);

        #endregion
    }
}