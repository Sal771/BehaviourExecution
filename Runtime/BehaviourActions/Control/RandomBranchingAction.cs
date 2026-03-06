using com.Sal77.BehaviourExecution;

[BehaviourCategory("Control/Random Branching")]
public class RandomBranchingAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        
    }

    protected override string GetActionName()
    {
        return "Random Branching";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }
}