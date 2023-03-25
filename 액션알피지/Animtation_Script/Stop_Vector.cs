using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stop_Vector : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    public float Stop_Speed=1;
    float Timer;
    Vector3 Start_Vector;
    Vector3 Arrive_Vector;
    public bool YReset;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
        Timer = 0;
        Start_Vector = PAC._Move.rb.velocity;
        Arrive_Vector = PAC._Move.rb.velocity;
        Arrive_Vector.x = 0;
        Arrive_Vector.z = 0;
        if (YReset) Arrive_Vector.y = 0;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Timer += Time.deltaTime* Stop_Speed;
        

        PAC._Move.rb.velocity = Vector3.Lerp(Start_Vector, Arrive_Vector, Timer);
       
    }
}
