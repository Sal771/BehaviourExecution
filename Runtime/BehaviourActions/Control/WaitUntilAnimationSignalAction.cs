using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Control/Wait Until Animation Signal")]
public class WaitUntilAnimationSignalAction : BehaviourAction
{
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        actionContext.DeclareVariable<GameObject>("Game Object", IBehaviourActionReadMode.Input);
        actionContext.DeclareVariable<string>("Animation Signal", IBehaviourActionReadMode.Input);
    }

    protected override string GetActionName()
    {
        return "Wait Until Animation Signal Action";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var gameObject = executionContext.ReadVariable<GameObject>("Game Object");
        var animationSignal = executionContext.ReadVariable<string>("Game Object");

        Animator animator = gameObject.GetComponent<Animator>();

        if(animator == null) return ExecutionActionResult.Successful;

        var animationClip = animator.GetCurrentAnimatorClipInfo(0)[0];

        AnimationEvent targetAnimationEvent = null;

        foreach(var animationEvent in animationClip.clip.events)
        {
            if(animationEvent.functionName == animationSignal)
            {
                targetAnimationEvent = animationEvent;
            }
        }

        if(targetAnimationEvent == null) return ExecutionActionResult.Successful;

        var animationState = animator.GetCurrentAnimatorStateInfo(0);

        if(animationState.normalizedTime > targetAnimationEvent.time / animationClip.clip.length)
        {
            return ExecutionActionResult.Successful;
        }

        return ExecutionActionResult.Waiting;
    }
    public override bool WaitCondition(IBehaviourExecution executionContext)
    {
        var gameObject = executionContext.ReadVariable<GameObject>("Game Object");
        var animationSignal = executionContext.ReadVariable<string>("Game Object");

        Animator animator = gameObject.GetComponent<Animator>();

        if(animator == null) return true;

        var animationClip = animator.GetCurrentAnimatorClipInfo(0)[0];

        AnimationEvent targetAnimationEvent = null;

        foreach(var animationEvent in animationClip.clip.events)
        {
            if(animationEvent.functionName == animationSignal)
            {
                targetAnimationEvent = animationEvent;
            }
        }

        if(targetAnimationEvent == null) return true;

        var animationState = animator.GetCurrentAnimatorStateInfo(0);

        if(animationState.normalizedTime > targetAnimationEvent.time / animationClip.clip.length)
        {
            return true;
        }

        return false;
    }
}