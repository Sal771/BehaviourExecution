using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Control/Random Branching")]
public class RandomBranchingAction : BehaviourAction
{
    private int m_branchAmount;
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        m_branchAmount = actionContext.DeclareNumberOption("Branch Amount");

        for(int i=0; i<m_branchAmount; i++)
        {
            actionContext.DeclareActionBuffer("Branch " + i);
            actionContext.DeclareVariable<float>("Weight " + i, IBehaviourActionReadMode.Input);
        }
    }

    protected override string GetActionName()
    {
        return "Random Branching";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        float[] weights = new float[m_branchAmount];

        float totalWeight = 0;
        for (int i = 0; i < m_branchAmount; i++)
        {
            weights[i] = executionContext.ReadVariable<float>("Weight " + i);
            totalWeight += weights[i];
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);
        float currentWeight = 0;

        for (int i = 0; i < m_branchAmount; i++)
        {
            currentWeight += weights[i];
            if (randomValue <= currentWeight)
            {
                executionContext.ExecuteActionBuffer("Branch " + i);
                break;
            }
        }

        return ExecutionActionResult.Successful;
    }
}