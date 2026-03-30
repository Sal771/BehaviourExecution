using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Misc/Get GameObject Transform")]
public class GameObjectTransformAction : BehaviourAction
{

    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<GameObject>("GameObject", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<Transform>("Result", IBehaviourActionReadMode.Input);
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var gameObject = executionContext.ReadVariable<GameObject>("GameObject");
        
        executionContext.WriteVariable("Result", gameObject.transform);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Get GameObject Transform";
    }
}
