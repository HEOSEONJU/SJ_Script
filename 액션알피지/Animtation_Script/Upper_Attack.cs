using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Upper_Attack : StateMachineBehaviour
{
    
    Player_Animaotr_Controller PAC;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PAC = animator.GetComponent<Player_Animaotr_Controller>();
        PAC._Move.rb.velocity = Vector3.zero;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        Vector3 deltaPosition_air = PAC.deltaPosition/Time.deltaTime; 

        
        PAC._Move.rb.velocity=deltaPosition_air;
        if (PAC.manager.LockOnMode)
        {
            PAC.transform.LookAt(PAC.manager._Camera.CurrentLockonTarget);
            Quaternion temp = PAC.transform.rotation;
            temp.Set(0, temp.y, 0, temp.w);
            PAC.transform.rotation = temp;

        }
        
    }

}
