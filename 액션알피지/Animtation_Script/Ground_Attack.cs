using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Ground_Attack : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 deltaPosition = animator.deltaPosition;
        deltaPosition.y = Physics.gravity.y * Time.deltaTime;


        //PAC._Move.rb.Move(deltaPosition); base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
    
}
