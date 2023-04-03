using UnityEngine;
public class Knock_Back_Animation : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;

    Vector3 Arrive_Vector;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
        animator.transform.localRotation = Quaternion.identity;//조준상태 로테이션초기화
        PAC._Move.rb.velocity = Vector3.zero;
        PAC._Move.rb.AddForce(animator.GetFloat("Knock") * -PAC.transform.forward, ForceMode.VelocityChange);
        
        Arrive_Vector = PAC._Move.rb.velocity;
        Arrive_Vector.x = 0;
        Arrive_Vector.z = 0;
        

    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log(PAC._Move.rb.velocity.magnitude + "벡터의크기");
        PAC._Move.rb.velocity = Vector3.Lerp(PAC._Move.rb.velocity, Arrive_Vector, Mathf.Pow(stateInfo.normalizedTime,2));
    }
}
