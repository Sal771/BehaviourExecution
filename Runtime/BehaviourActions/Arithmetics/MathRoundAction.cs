using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Math Round")]
public class MathRoundAction : BehaviourAction
{
    private BehaviourBinding m_numberReference;
    private BehaviourBinding m_roundIntervalReference;
    private BehaviourBinding m_outputReference;
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        m_numberReference = actionContext.DeclareVariable<float>("Number");
        m_roundIntervalReference = actionContext.DeclareVariable<float>("RoundInterval");
        //To add round mode
        m_outputReference = actionContext.DeclareVariable<float>("Output");
    }

    protected override string GetActionName()
    {
        return "Math Round";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var number = executionContext.ReadVariable<float>(m_numberReference);
        var roundInterval = executionContext.ReadVariable<float>(m_roundIntervalReference);

        var roundedNumber = Mathf.Round(number * (1/roundInterval)) / (1/roundInterval);

        executionContext.WriteVariable<float>(m_outputReference, roundedNumber);

        return ExecutionActionResult.Successful;
    }
}