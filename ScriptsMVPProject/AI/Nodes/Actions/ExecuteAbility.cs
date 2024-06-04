using TheKiwiCoder;
using TollanWorlds.Abilities;
using Cysharp.Threading.Tasks;

[System.Serializable]
public class ExecuteAbility : ActionNode
{
    public Abilities Ability;
    public bool WaitUntilFinish;

    private State state;
    protected override void OnStart() 
    {
        state = State.Running;
        Execute();
    }

    private async UniTask Execute()
    {
        if(context.abilities.ContainsKey(Ability)) 
        {
            var ability = context.abilities[Ability];
            if(ability.IsActive)
            {
                ability.Execute();

                if(WaitUntilFinish)
                    await UniTask.WaitUntil(() => !ability.IsExecuting);
            }
        }
        state = State.Success;
    }

    protected override void OnStop() {
    }

    protected override State OnUpdate() {
        return state;
    }
}
