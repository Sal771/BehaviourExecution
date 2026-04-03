using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Control/Execute Behaviour Object")]
public class ExecuteBehaviourObjectAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<BehaviourObject>("Behaviour Object", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<GameObject>("Game Object", IBehaviourActionReadMode.Input);
    }

    protected override string GetActionName()
    {
        return "Execute Behaviour Object";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var behaviourObject = executionContext.ReadVariable<BehaviourObject>("Behaviour Object");
        var gameObject = executionContext.ReadVariable<GameObject>("Game Object");

        gameObject.GetComponent<ExecutionRunner>()?.Execute(behaviourObject);//TODO Redesign this

        return ExecutionActionResult.Successful;
    }
}