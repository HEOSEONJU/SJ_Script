using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Boss_Manager : Enemy_Manager
{

    [SerializeField]
    Animator FMS;
    [SerializeField]
    Animator Body_Animator;
    [SerializeField]
    Animator SH_Animator;
    [SerializeField]
    BossSoundManager SoundManager;

    [SerializeField]
    public EnemyBossBulletAttack EBBA;
    [SerializeField]
    public EnemyBossRocketAttack EBRA;

    private void Start()
    {
        Target = GameManager.instance.Char_Player_Trace.transform;
        

    }
    
    
    private void FixedUpdate()
    {
        _Move.Cal_Dis();
    }

    private void LateUpdate()
    {
        bool Follow = FMS.GetBool("Follow");
        Body_Animator.SetBool("Follow", Follow);
        SH_Animator.SetBool("Follow", Follow);

        if(!_HP.Live)
        {
            Body_Animator.SetBool("Death", true);
            SH_Animator.SetBool("Death", true);
        }


    }
    public void StompPlay()
    {
        SoundManager.StompPlay();
    }

    public void ShieldDestroyPlay()
    {
        SoundManager.ShieldDestroyPlay();
        EBBA.StopAttack();
        EBBA.StopAttack();

    }

    public void ShieldOnlnePlay()
    {
        SoundManager.ShieldOnlnePlay();
        
    }
    public void SHB()
    {
        FMS.SetTrigger("SHBroken");
            
        FMS.SetBool("Follow", false);


    }
    public override bool Search_Target()
    {
        return true;
    }
    public override void Attack_Function()
    {
        _Move.Attack_Order();
        

    }
    public override bool Check_Attack()
    {
        if (Current_Delay <= MAX_Delay)
        {
            Debug.Log("ÀåÀüÁß");
            return false;
        }
        
        Ray ray = new Ray();
        ray.direction = Vector3.up;
        ray.origin = transform.position;
        RaycastHit[] hit;
        hit = Physics.SphereCastAll(ray, Attack_Range, 1, Layer);
        if(hit.Length>0)
        {
            transform.GetChild(0).GetComponent<EnemyBossBulletAttack>().StopAttack();
            transform.GetChild(0).GetComponent<EnemyBossRocketAttack>().StopAttack();
            return true;
        }
        
        
        
        




        return false;
    }

}
