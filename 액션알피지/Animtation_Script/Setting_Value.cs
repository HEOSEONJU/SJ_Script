using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Setting_Value : StateMachineBehaviour
{
    [SerializeField]
    string Key;
    [SerializeField]
    float Value;
    [SerializeField]
    StateType TYPE;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch(TYPE)
        {
            case StateType.INT:
                animator.SetInteger(Key, Convert.ToInt32(Value));
                break;
            case StateType.FLOAT:
                animator.SetFloat(Key, Value);
                break;
        }
            }
    // Start is called before the first frame update

    enum StateType
    {
        INT,
        FLOAT,
        
    }
}
