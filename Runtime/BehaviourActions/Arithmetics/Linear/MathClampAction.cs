using com.Sal77.BehaviourExecution;
using System;

[BehaviourCategory("Arithmetics/Math Clamp")]
public class MathClampAction : BehaviourAction
{
    private int m_numberTypeIndex;
    public enum NumberType
    {
        Int = 0,
        Float = 1,
    }
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        //TODO fix this
        actionContext.DeclareVariable<float>("Number");
        actionContext.DeclareVariable<float>("ClampMin");
        actionContext.DeclareVariable<float>("ClampMax");
        actionContext.DeclareVariable<float>("Output");
    }

    protected override string GetActionName()
    {
        return "Math Clamp";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var number = executionContext.ReadVariable<float>("Number");
        var clampMin = executionContext.ReadVariable<float>("ClampMin");
        var clampMax = executionContext.ReadVariable<float>("ClampMax");

        var clampedNumber = Math.Clamp(number, clampMin, clampMax);

        executionContext.WriteVariable<float>("Output", clampedNumber);

        return ExecutionActionResult.Successful;
    }
}