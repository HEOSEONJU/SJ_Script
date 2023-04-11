using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class State_Boar_Gravity : StateMachineBehaviour
{
    Boar_Enemy KN;
    [SerializeField]
    LayerMask Layer;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        KN = animator.GetComponent<Boar_Enemy>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Ray ray = new Ray();
        ray.direction = Vector3.down;
        ray.origin = animator.transform.position + new Vector3(0, 0.5f, 0);
        RaycastHit[] hits = Physics.RaycastAll(ray, 0.6f, Layer);

        if (hits.Length != 0)
        {
            animator.SetBool("Air", false);

        }
        else
        {
            KN._RD.velocity+=new Vector3(0,Physics.gravity.y,0)*Time.deltaTime*3;
        }
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        
    }

}



