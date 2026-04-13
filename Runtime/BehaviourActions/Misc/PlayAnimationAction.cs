using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Misc/Play Animation")]
public class PlayAnimationAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<GameObject>("Game Object", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<string>("Animation Name", IBehaviourActionReadMode.Input);
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var animationName = executionContext.ReadVariable<string>("Animation Name");
        var gameObject = executionContext.ReadVariable<GameObject>("Game Object");

        Animator animator = gameObject.GetComponent<Animator>();

        if(animator == null) return ExecutionActionResult.Successful;

        animator.Play(animationName);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Play Animation";
    }
}