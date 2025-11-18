using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    { 
        FunctionTimer.Create(TestingAction, 3f);
        FunctionTimer.Create(TestingAction2, 5f);
    }

    private void TestingAction()
    {
        Debug.Log("TestingAction");
    }
    
    private void TestingAction2()
    {
        Debug.Log("TestingAction2");
    }
}
