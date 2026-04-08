using com.Sal77.BehaviourExecution;
using UnityEngine;

public class HelloBehaviour : MonoBehaviour
{
    public BehaviourObject BehaviourObject;
    private void Start()
    {
        GetComponent<ExecutionRunner>().ExecuteDirectly(BehaviourObject);
    }
}