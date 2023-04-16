using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Ground_Attack : StateMachineBehaviour
{
    Player_Animaotr_Controller PAC;
    [SerializeField]
    float Combo_Timer;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent<Player_Animaotr_Controller>(out PAC);
        PAC.manager._comboTimer = Combo_Timer;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {




        Vector3 deltaPosition = PAC.deltaPosition / Time.deltaTime;

        deltaPosition.y = 0;
        PAC._Move._rigidBody.velocity = deltaPosition;
        if (PAC.manager.LockOnMode)
        {
            PAC.transform.LookAt(PAC.manager._camera.CurrentLockonTarget);
            Quaternion temp = PAC.transform.rotation;
            temp.Set(0, temp.y, 0, temp.w);
            PAC.transform.rotation = temp;

        }



        //PAC._Move.rb.Move(deltaPosition); base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetBool("Infinity"))
        {
            return;
        }
        PAC._Move._rigidBody.velocity = Vector3.zero;

        //Debug.Log(animator.name + "쿼터니언초기화");
        animator.transform.localRotation= Quaternion.identity;
    }
}
