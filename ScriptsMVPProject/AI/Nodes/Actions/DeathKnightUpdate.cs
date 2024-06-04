using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TheKiwiCoder;
using TollanWorlds.Abilities;
using TollanWorlds.Combat;
using System;
[System.Serializable]
public class DeathKnightUpdate : ActionNode
{
    public NodeProperty<GameObject> player;
    public NodeProperty<Vector3> playerPosition;
    public NodeProperty<Vector3> teleportTarget;
    public NodeProperty<int> abilityIndex;
    public NodeProperty<float> fightDistance;
    public NodeProperty<int> secondPhase;
    public NodeProperty<int> thirdPhase;
    private bool _forceTeleport = false;
    private bool _forceSlash = false;
    private Vector3 _forceSlashTarget;
    protected override void OnStart()
    {
        (context.abilities[Abilities.Charge] as Charge).OnHitWall.AddListener(() => _forceTeleport = true);
        (context.abilities[Abilities.Charge] as Charge).OnHitDamageable.AddListener((Damageable target) => {
            _forceSlash = true;
            _forceSlashTarget = target.transform.position;
        });
        context.abilities[Abilities.Teleport].OnExecuted.AddListener(() => _forceTeleport = false);
        context.abilities[Abilities.WideSlash].OnExecuted.AddListener(() => _forceSlash = false);

        context.healthSystem.OnDamaged.AddListener((float damage, float hp) => 
        {
            if(context.healthSystem.RemainingPercentage < 33)
            {
                if(thirdPhase.Value==0)
                    thirdPhase.Value = 1;
            }
            else if(context.healthSystem.RemainingPercentage < 66)
            {
                if(secondPhase.Value==0)
                    secondPhase.Value = 1;
            }
        });
    }

    void OnHitWall()
    {

    }
    void OnTeleportExecuted()
    {

    }

    protected override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        playerPosition.Value = player.Value.transform.position;
        context.spriteRenderer.flipX = playerPosition.Value.x >= context.transform.position.x ? false : true;

        var teleport = context.abilities[Abilities.Teleport];
        var slash = context.abilities[Abilities.WideSlash];
        var darkShroud = context.abilities[Abilities.DarkShroud];
        var charge = context.abilities[Abilities.Charge];

        if(!teleport.IsExecuting && !slash.IsExecuting && !darkShroud.IsExecuting && !charge.IsExecuting)
        {
            if(_forceTeleport || Vector2.Distance(context.transform.position, playerPosition.Value) > fightDistance.Value)
            {
                (teleport as Teleport).SetTarget(teleportTarget.Value);
                if(abilityIndex.Value != 0)
                    abilityIndex.Value = 0;
            }
            else if(slash.IsActive)
            {
                (slash as WideSlash).SetTarget(playerPosition.Value);
                if(abilityIndex.Value != 1)
                    abilityIndex.Value = 1;
            }
            else if(_forceSlash)
            {
                slash.ForceActivate();
                (slash as WideSlash).SetTarget(_forceSlashTarget);
                if(abilityIndex.Value != 1)
                    abilityIndex.Value = 1;
            }
            else if(darkShroud.IsActive)
                abilityIndex.Value = 2;
            else if(charge.IsActive)
            {
                (charge as Charge).SetTarget((playerPosition.Value - context.transform.position).normalized);

                if(abilityIndex.Value != 3)
                    abilityIndex.Value = 3;
            }
        }
        return State.Running;
    }
}
