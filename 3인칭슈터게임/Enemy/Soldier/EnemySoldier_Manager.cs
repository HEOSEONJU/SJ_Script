using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySoldier_Manager : Enemy_Manager
{
    [SerializeField]
    EnemySoldierSound _Sound;

    [SerializeField]
    Animator _Animator;

    public Vector3 relativeVec1;

    public Transform GunFire;
    public ParticleSystem MuzzleFlash;
    public AudioSource AttackSound;
    public GameObject[] Bullet;
    public GameObject[] BulletEffect;
    public int Bullet_INDEX;
    public void Update()
    {
        if(!_HP.Live)
        {
            _Move.agent.isStopped = false;
            _Move.agent.velocity = Vector3.zero;
            return;
        }
        _Move.DelayTrigger  += Time.deltaTime;
        Current_Delay += Time.deltaTime;

        if(_Move.Distance()>=MAX_Dis)
        {
            Aggro-=Time.deltaTime;
        }

    }
    private void FixedUpdate()
    {
        _Move.Cla_Dis();
    }

    private void LateUpdate()
    {
        if(!_HP.Live) { return; }
        if (Target != null)
        {




            Transform Spine = _Animator.GetBoneTransform(HumanBodyBones.Spine);
            Spine.LookAt(Target.position);
            Spine.rotation *= Quaternion.Euler(relativeVec1);

        }
    }
    public override void Attack_Function()
    {
        
        MuzzleFlash.Play();
        AttackSound.Play();
        Aggro = MAX_Aggro;
        Bullet[Bullet_INDEX].transform.position = GunFire.position;
        
        Bullet[Bullet_INDEX].SetActive(true);
        
        Bullet[Bullet_INDEX].GetComponent<EnemySoldierBullet>().BulletAction(GameManager.instance.Char_Player_Attack.transform);
        Bullet_INDEX++;
        if (Bullet_INDEX == Bullet.Length)
        {
            Bullet_INDEX = 0;
        }
        
    }
    public override bool Search_Target()
    {
        if(Target== null) return false;
        return true;




        
    }


    public void Targeting(Transform Temp)
    {
        if(Aggro<=0)
        {
            Aggro = MAX_Aggro;
        }
            Target = Temp;
            
        

    }
    public void DesBullet()
    {
        int Len = Bullet.Length;
        for (int i = 0; i < Len; i++)
        {
            Destroy(Bullet[i].gameObject);
        }
        Len = BulletEffect.Length;
        for (int i = 0; i < Len; i++)
        {
            Destroy(BulletEffect[i].gameObject);
        }
    }


}
