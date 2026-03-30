using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Math Round")]
public class MathRoundAction : BehaviourAction
{
    private int m_numberTypeIndex;
    private int m_roundModeIndex;
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
        m_numberTypeIndex = actionContext.DeclareEnum<NumberType>("Number Type");

        if (m_numberTypeIndex == (int)NumberType.Float )
        {
            actionContext.DeclareVariable<float>("Number", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<float>("Round Interval", IBehaviourActionReadMode.Input);
        }
        else
        {
            actionContext.DeclareVariable<int>("Number", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<int>("Round Interval", IBehaviourActionReadMode.Input);
        }
        
        m_roundModeIndex = actionContext.DeclareEnum<RoundMode>("Round Mode");

        if (m_numberTypeIndex == (int)NumberType.Float )
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
        var number = executionContext.ReadVariable<float>("Number");
        var roundInterval = executionContext.ReadVariable<float>("Round Interval");

        float roundedNumber = 0;

        switch ((RoundMode)m_roundModeIndex)
        {
            case RoundMode.Closest:
                roundedNumber = Mathf.Round(number * (1/roundInterval)) / (1/roundInterval);
            break;
            case RoundMode.RoundUp:
                roundedNumber = Mathf.Ceil(number * (1/roundInterval)) / (1/roundInterval);
            break;
            case RoundMode.RoundDown:
                roundedNumber = Mathf.Floor(number * (1/roundInterval)) / (1/roundInterval);
            break;
        }

        executionContext.WriteVariable<float>("Output", roundedNumber);

        return ExecutionActionResult.Successful;
    }
}