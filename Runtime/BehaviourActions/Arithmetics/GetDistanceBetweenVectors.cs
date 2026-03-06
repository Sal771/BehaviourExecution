using com.Sal77.BehaviourExecution;

using UnityEngine;

[BehaviourCategory("Arithmetics/Get Distance Between Vectors")]
public class GetDistanceBetweenVectors : BehaviourAction
{
    private BehaviourBinding m_vectorAReference;
    private BehaviourBinding m_vectorBReference;
    private BehaviourBinding m_resultReference;

    public override void DefineBindings(IBehaviourVariable executionContext)
    {
        m_vectorAReference = executionContext.DeclareVariable<Vector3>("VectorA");
        m_vectorBReference = executionContext.DeclareVariable<Vector3>("VectorB");
        m_resultReference = executionContext.DeclareVariable<float>("Result");
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var vectorA = executionContext.ReadVariable<Vector3>(m_vectorAReference);
        var vectorB = executionContext.ReadVariable<Vector3>(m_vectorBReference);
        
        float distance = Vector3.Distance(vectorA, vectorB);
        executionContext.WriteVariable(m_resultReference, distance);

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Get Distance Between Vectors";
    }
}
