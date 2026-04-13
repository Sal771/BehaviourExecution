using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Control/Random Chance")]
public class RandomChanceAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareActionBuffer("Actions");
        actionContext.DeclareVariable<float>("Chance (0-1)", IBehaviourActionReadMode.Input);
    }

    protected override string GetActionName()
    {
        return "Random Chance";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var randomChance = executionContext.ReadVariable<float>("Chance (0-1)");

        float randomValue = UnityEngine.Random.Range(0f, 1f);
            
        if (randomValue <= randomChance)
        {
            executionContext.ExecuteActionBuffer("Actions");
        }

        return ExecutionActionResult.Successful;
    }
}