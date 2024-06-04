using System.Collections.Generic;
using System.Linq;
using TollanWorlds.Abilities;
using TollanWorlds.Combat;
using TollanWorlds.Movement;
using UnityEngine;
using UnityEngine.InputSystem;

namespace TollanWorlds
{
    /// <summary>
    /// For handling user inputs and player damages
    /// </summary>
    public class Player : Damageable
    {
        #region Variables
        [SerializeField] private MovementController _movement;
        [SerializeField] private AttackController _attack;

        private List<IAbility> _abilities;
        #endregion

        #region Monobehaviour Callbacks
        public override void Awake()
        {
            base.Awake();

            _abilities = GetComponentsInChildren<IAbility>(true).ToList();
        }
        #endregion

        #region Functions
        public override void Damage(float damage)
        {
            if(!IsAlive)
                return;

            base.Damage(damage);
        }
        public override void Death()
        {
            base.Death();
            enabled = false;
        }
        #endregion

        #region Input System Callbacks
        private void OnMove(InputValue inputValue)
        {
            if(!IsAlive)
                return;

            var input = inputValue.Get<Vector2>();
            input = IsFreezing?Vector2.zero : input;
            _movement?.Move(input);
        }

        private void OnPrimaryAttack(InputValue inputValue)
        {
            if(!IsAlive || IsFreezing)
                return;

            var point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _movement?.Look(point);
            _attack?.PrimaryAttack(point);
        }

        private void OnSecondaryAttack(InputValue inputValue)
        {
            if(!IsAlive || IsFreezing)
                return;

            var point = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue());
            _movement?.Look(point);
            _attack?.SecondaryAttack(point);
        }
        private void OnDash(InputValue inputValue)
        {
            if(!IsAlive || IsFreezing)
                return;

            var ability = _abilities?.Find(x => x.GetType() == typeof(Abilities.Dash));
            if(ability != null && ability.IsActive)
                ability.Execute();
        }
        #endregion
    }
}