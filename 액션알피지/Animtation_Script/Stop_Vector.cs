using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEngine;

public class Stop_Vector : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    public float Stop_Speed=1;
    float Timer;
    Vector3 Start_Vector;
    Vector3 Arrive_Vector;
    public bool YReset;

    public int StopLevel;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        StopLevel=layerIndex;
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
        Timer = 0;
        Start_Vector = PAC._Move.rb.velocity;
        
        Arrive_Vector = PAC._Move.rb.velocity;
        Arrive_Vector.x = 0;
        Arrive_Vector.z = 0;
        if (YReset)
        {
            Start_Vector = Vector3.zero;
            Arrive_Vector.y = 0;
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch(StopLevel)
        {
            case 0:
                if (!animator.GetCurrentAnimatorStateInfo(1).IsTag("None"))
                {
                    return;
                }
                if (!animator.GetCurrentAnimatorStateInfo(2).IsTag("None"))
                {
                    return;
                }
                break;
            case 1:
                if (animator.GetCurrentAnimatorStateInfo(1).IsTag("None"))
                {
                    return;
                }
                    break;
            case 2:
                
                break;
        }
        
        

        Timer += Time.deltaTime* Stop_Speed;
        

        PAC._Move.rb.velocity = Vector3.Lerp(Start_Vector, Arrive_Vector, Timer);
        //PAC._Move.rb.velocity = Vector3.Lerp(Start_Vector, Arrive_Vector, Mathf.Pow(stateInfo.length, 2));

    }
}
