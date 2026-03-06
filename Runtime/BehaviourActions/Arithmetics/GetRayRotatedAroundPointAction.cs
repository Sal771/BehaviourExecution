using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Get Ray Rotated Around Point")]
public class GetRayRotatedAroundPointAction : BehaviourAction
{
    private BehaviourBinding m_rayReference;
    private BehaviourBinding m_pivotPositionReference;
    private BehaviourBinding m_rotationReference;
    private BehaviourBinding m_outputReference;
    
    public override void DefineBindings(IBehaviourVariable executionContext)
    {
        m_rayReference = executionContext.DeclareVariable<Ray>("Ray");
        m_pivotPositionReference = executionContext.DeclareVariable<Vector3>("Pivot Position");
        m_rotationReference = executionContext.DeclareVariable<Quaternion>("Rotation");
        m_outputReference = executionContext.DeclareVariable<Ray>("Output");
    }

    protected override string GetActionName()
    {
        return "Get Ray Rotated Around Point";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        var ray = executionContext.ReadVariable<Ray>(m_rayReference);
        var pivotPosition = executionContext.ReadVariable<Vector3>(m_pivotPositionReference);
        var rotation = executionContext.ReadVariable<Quaternion>(m_rotationReference);

        var direction = ray.direction;
        var rotatedDirection = rotation * direction;
        var newOrigin = pivotPosition + rotatedDirection;
        
        var result = new Ray(newOrigin, rotatedDirection);
        executionContext.WriteVariable<Ray>(m_outputReference, result);

        return ExecutionActionResult.Successful;
    }
}
