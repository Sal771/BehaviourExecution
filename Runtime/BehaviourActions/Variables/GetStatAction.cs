using System;
using System.Collections.Generic;
using UnityEngine;
using com.Sal77.BehaviourExecution;

[BehaviourCategory("Variables/Get Stat")]
public class GetStatAction : BehaviourAction
{
    public override void DefineBindings(IBehaviourVariable actionContext)
    {
        
    }

    protected override string GetActionName()
    {
        return "Get Stat";
    }

    public override ExecutionActionResult Execute(IBehaviourExecution executionContext)
    {
        return ExecutionActionResult.Successful;
    }
}