using com.Sal77.BehaviourExecution;
using System;

[BehaviourCategory("Arithmetics/Math Clamp")]
public class MathClampAction : BehaviourAction
{
    public enum NumberType
    {
        Int = 0,
        Float = 1,
    }
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        var numberType = actionContext.DeclareEnum<NumberType>("Number Type");

        if(numberType == NumberType.Float)
        {
            actionContext.DeclareVariable<float>("Number", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<float>("ClampMin", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<float>("ClampMax", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<float>("Output", IBehaviourActionReadMode.Output);
        }
        else
        {
            actionContext.DeclareVariable<int>("Number", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<int>("ClampMin", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<int>("ClampMax", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<int>("Output", IBehaviourActionReadMode.Output);
        }
    }

    protected override string GetActionName()
    {
        return "Math Clamp";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var numberType = executionContext.GetEnumValue<NumberType>("Number Type");

        if(numberType == NumberType.Float)
        {
            var number = executionContext.ReadVariable<float>("Number");
            var clampMin = executionContext.ReadVariable<float>("ClampMin");
            var clampMax = executionContext.ReadVariable<float>("ClampMax");

            var clampedNumber = Math.Clamp(number, clampMin, clampMax);

            executionContext.WriteVariable<float>("Output", clampedNumber);
        }
        else
        {
            var number = executionContext.ReadVariable<int>("Number");
            var clampMin = executionContext.ReadVariable<int>("ClampMin");
            var clampMax = executionContext.ReadVariable<int>("ClampMax");

            var clampedNumber = Math.Clamp(number, clampMin, clampMax);

            executionContext.WriteVariable<int>("Output", clampedNumber);
        }

        return ExecutionActionResult.Successful;
    }
}