using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TollanWorlds.Abilities;
using TollanWorlds.Abilities.BossAbilities;
using TollanWorlds.Combat;
using Unity.VisualScripting;
using UnityEngine;

namespace TollanWorlds.Boss
{
    public class Boss : Damageable
    {
        private List<IAbility> _abilities;

        public override void Awake()
        {
            base.Awake();

            _abilities = GetComponentsInChildren<IAbility>(true).ToList();
        }

        private void Update()
        {
            OnTeleport();
        }

        private void OnCharge()
        {
            if(!IsAlive)
                return;

            var ability = _abilities?.Find(x => x.GetType() == typeof(Abilities.BossAbilities.Charge));
            if (ability != null && ability.IsActive)
                ability.Execute();
        }

        private void OnSlash()
        {
            if (!IsAlive)
                return;

            var ability = _abilities?.Find(x => x.GetType() == typeof(Abilities.BossAbilities.WideSlash));
            if (ability != null && ability.IsActive)
                ability.Execute();
        }

        private void OnTeleport()
        {
            if (!IsAlive)
                return;

            var ability = _abilities?.Find(x => x.GetType() == typeof(Abilities.BossAbilities.Teleport));
            if (ability != null && ability.IsActive)
                ability.Execute();
        }

        private void OnApocalypse()
        {
            if (!IsAlive)
                return;

            var ability = _abilities?.Find(x => x.GetType() == typeof(Abilities.BossAbilities.Apocalypse));
            if (ability != null && ability.IsActive)
                ability.Execute();
        }
    }
}
