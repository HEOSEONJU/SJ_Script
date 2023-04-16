
using UnityEngine;
using Hit_Type_NameSpace;

public class Attack_Type_Change : StateMachineBehaviour
{

    Player_Animaotr_Controller PAC;
    [SerializeField]
    EAttack_Special _type;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.TryGetComponent<Player_Animaotr_Controller>(out PAC);
        PAC.manager._Attack_Effect = _type;


    }

    

}
