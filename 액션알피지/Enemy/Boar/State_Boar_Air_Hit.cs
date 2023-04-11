using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Boar_Air_Hit : StateMachineBehaviour
{
    Boar_Enemy KN;
    [SerializeField]
    float AirPower;
    [SerializeField]
    LayerMask Layer;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        KN = animator.GetComponent<Boar_Enemy>();
        animator.SetBool("Air", true);
        
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        ray.origin = animator.transform.position+new Vector3(0,0.5f,0);
        RaycastHit[] hits = Physics.RaycastAll(ray, 0.6f, Layer);
        
        if (hits.Length == 0)
        {

            Debug.Log("공중타격");

            KN._RD.velocity = Vector3.up * AirPower/8f;

        }
        else
        {
            Debug.Log("지상타격");
            KN._RD.velocity = Vector3.up * AirPower;
            //KN._RD.AddForce(Vector3.up * AirPower, ForceMode.Impulse);
        }
        KN._RD.useGravity = false;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        KN._RD.useGravity = true;
    }
}