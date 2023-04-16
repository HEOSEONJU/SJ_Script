using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Air_Attack : StateMachineBehaviour
{

    Player_Animaotr_Controller PAC;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent<Player_Animaotr_Controller>(out PAC);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 deltaPosition_air = animator.deltaPosition;
        deltaPosition_air.y *= 1.5f * Time.deltaTime;


        //PAC._Move.rb.Move(deltaPosition_air);
    }

}
