using com.Sal77.BehaviourExecution;
using UnityEngine;

[BehaviourCategory("Arithmetics/Math Basic Operator")]
public class MathBasicOperatorAction : BehaviourAction
{
    private int m_numberTypeIndex;
    private int m_operationTypeIndex;
    public enum NumberType
    {
        Int = 0,
        Float = 1,
    }
    public enum OperationType
    {
        Add = 0,
        Sub = 1,
        Mult = 2,
        Div = 3
    }
    protected override void DefineBindings(IBehaviourAction actionContext)
    {
        m_numberTypeIndex = actionContext.DeclareEnum<NumberType>("Number Type");

        if(m_numberTypeIndex == (int)NumberType.Float)
        {
            actionContext.DeclareVariable<float>("Number1", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<float>("Number2", IBehaviourActionReadMode.Input);
        }
        else
        {
            actionContext.DeclareVariable<int>("Number1", IBehaviourActionReadMode.Input);
            actionContext.DeclareVariable<int>("Number2", IBehaviourActionReadMode.Input);
        }
        
        m_operationTypeIndex = actionContext.DeclareEnum<OperationType>("Operation Type");

        if(m_numberTypeIndex == (int)NumberType.Float)
        {
            actionContext.DeclareVariable<float>("Result", IBehaviourActionReadMode.Output);
        }
        else
        {
            actionContext.DeclareVariable<int>("Result", IBehaviourActionReadMode.Output);
        }
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        float number1 = 0;

        if(m_numberTypeIndex == (int)NumberType.Float)
        {
            number1 = executionContext.ReadVariable<float>("Number1");
        }
        else
        {
            number1 = executionContext.ReadVariable<int>("Number1");
        }

        float number2 = 0;

        if(m_numberTypeIndex == (int)NumberType.Float)
        {
            number2 = executionContext.ReadVariable<float>("Number1");
        }
        else
        {
            number2 = executionContext.ReadVariable<int>("Number1");
        }

        float result = 0;

        switch ((OperationType)m_operationTypeIndex)
        {
            case OperationType.Add:
                result = number1 + number2;
                break;
            case OperationType.Sub:
                result = number1 - number2;
                break;
            case OperationType.Mult:
                result = number1 * number2;
                break;
            case OperationType.Div:
                result = number1 / number2;
                break;
        }

        if(m_numberTypeIndex == (int)NumberType.Float)
        {
            executionContext.WriteVariable<float>("Number1", result);
        }
        else
        {
            executionContext.WriteVariable<int>("Number1", (int)result);
        }

        return ExecutionActionResult.Successful;
    }

    protected override string GetActionName()
    {
        return "Math Basic Operator";
    }
}
