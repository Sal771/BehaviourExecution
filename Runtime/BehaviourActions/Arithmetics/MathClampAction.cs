using com.Sal77.BehaviourExecution;
using System;

[BehaviourCategory("Arithmetics/Math Clamp")]
public class MathClampAction : BehaviourAction
{
    private BehaviourBinding m_numberReference;
    private BehaviourBinding m_clampMinReference;
    private BehaviourBinding m_clampMaxReference;
    private BehaviourBinding m_outputReference;
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        m_numberReference = actionContext.DeclareVariable<float>("Number");
        m_clampMinReference = actionContext.DeclareVariable<float>("ClampMin");
        m_clampMaxReference = actionContext.DeclareVariable<float>("ClampMax");
        m_outputReference = actionContext.DeclareVariable<float>("Output");
    }

    protected override string GetActionName()
    {
        return "Math Clamp";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var number = executionContext.ReadVariable<float>(m_numberReference);
        var clampMin = executionContext.ReadVariable<float>(m_clampMinReference);
        var clampMax = executionContext.ReadVariable<float>(m_clampMaxReference);

        var clampedNumber = Math.Clamp(number, clampMin, clampMax);

        executionContext.WriteVariable<float>(m_outputReference, clampedNumber);

        return ExecutionActionResult.Successful;
    }
}