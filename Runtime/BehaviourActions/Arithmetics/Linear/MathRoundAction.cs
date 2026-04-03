using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Math Round")]
public class MathRoundAction : BehaviourAction
{
    public enum NumberType
    {
        Int = 0,
        Float = 1,
    }
    public enum RoundMode
    {
        Closest = 0,
        RoundUp = 1,
        RoundDown = 2
    }
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        var numberType = actionContext.DeclareEnum<NumberType>("Number Type");

        if (numberType == NumberType.Float )
        {
            actionContext.DeclareVariable<float>("Number", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<float>("Round Interval", IBehaviourActionReadMode.Input);
        }
        else
        {
            actionContext.DeclareVariable<int>("Number", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<int>("Round Interval", IBehaviourActionReadMode.Input);
        }
        
        actionContext.DeclareEnum<RoundMode>("Round Mode");

        if (numberType == NumberType.Float )
        {
            actionContext.DeclareVariable<float>("Output", IBehaviourActionReadMode.Output);
        }
        else
        {
            actionContext.DeclareVariable<int>("Output", IBehaviourActionReadMode.Output);
        }
    }

    protected override string GetActionName()
    {
        return "Math Round";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var numberType = executionContext.GetEnumValue<NumberType>("Number Type");
        var roundMode = executionContext.GetEnumValue<RoundMode>("Round Mode");

        var number = executionContext.ReadVariable<float>("Number");
        var roundInterval = executionContext.ReadVariable<float>("Round Interval");

        float roundedNumber = 0;

        switch (roundMode)
        {
            case RoundMode.Closest:
                roundedNumber = Mathf.Round(number / roundInterval) * roundInterval;
            break;
            case RoundMode.RoundUp:
                roundedNumber = Mathf.Ceil(number / roundInterval) * roundInterval;
            break;
            case RoundMode.RoundDown:
                roundedNumber = Mathf.Floor(number / roundInterval) * roundInterval;
            break;
        }

        if(numberType == NumberType.Float)
        {
            executionContext.WriteVariable<float>("Output", roundedNumber);
        }
        else
        {
            executionContext.WriteVariable<int>("Output", (int)roundedNumber);
        }

        return ExecutionActionResult.Successful;
    }
}