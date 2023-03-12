using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySoldierMove : EnemyMove
{
    
    
    

    public float DelayTrigger;
    
    public bool walk,run;
    public Transform GunFire;
    public GameObject[] Bullet;
    public GameObject[] BulletEffect;
    public int count;
    public ParticleSystem MuzzleFlash;
    public Vector3 relativeVec1;
    
    EnemySoldierHP Hp;
    public Vector3 Myposi;
    public float SquadDis;
    public AudioSource AttackSound;
    void Awake()
    {
        animator = GetComponent<Animator>();
        
        //InvokeRepeating("MoveToNextWayPoint",0.5f ,2.0f);
        Delay = 2.0f;
        Aggro = 0;
        MaxDis= 30.0f;
        SquadDis = 0;
        WayCount = 0;
        MaxAggro = 10;
        walk = false;
        run = false;
        DelayTrigger = 0;
        Hp =GetComponent<EnemySoldierHP>();
        count = 0;
        for(int i=0;i<BulletEffect.Length;i++)
        {
            Bullet[i].transform.parent = null;
            BulletEffect[i].transform.parent = null;
        }
        
        
    }
    private void Start()
    {
        if(Waypoint.Length>0)
        agent.SetDestination(Waypoint[0].position);
    }

    public void Update()
    {
        if (Hp.Live)
        {
            Myposi = transform.position;
            if(DelayTrigger<=1.0f)
            DelayTrigger += Time.deltaTime;

            if(Target!=null)
            {
                dis = Vector3.Distance(Target.position, transform.position);
                if(dis>MaxDis)
                {
                    LostOnTarget();
                }
                
            }

            if (LockOn & Target != null)
            {
                walk = false;
                agent.speed = 3.5f;
                run = false;
                agent.isStopped = false;
                agent.velocity = Vector3.zero;
                
                
                if (Delay <= 2.0f)
                {
                    Delay += Time.deltaTime;
                }
                if (Delay >= 2.0f)
                {
                    if (dis < MaxDis)
                    {
                        AttackTarget();
                    }
                    
                }


            }

            else if (Target != null)
            {
                agent.SetDestination(Target.position);

                agent.speed = 7;
                run = true;
                dis = Vector3.Distance(Target.position, transform.position);
                if (dis> MaxDis)
                    Aggro -= Time.deltaTime;
            }
            if (Target == null)
            {
                walk = true;
                agent.speed = 3.5f;
                run = false;
            }
            

            if(Aggro <=0&Target!=null)
            {
                ReMoveTargeting();
                NextWayMove();
            }
        }
        else
        {
            walk=false;
            
            agent.isStopped = false;
            agent.velocity= Vector3.zero;

        }

    }
    public void FixedUpdate()
    {
        animator.SetBool("Walk", walk);
        animator.SetBool("Run", run);

    }
    public void LateUpdate()
    {
        if (LockOn & Target != null &Hp.Live)
        {




            Transform Spine = animator.GetBoneTransform(HumanBodyBones.Spine);
            Spine.LookAt(Target.position);
            Spine.rotation *= Quaternion.Euler(relativeVec1);

        }
    }
    public void DesBullet()
    {
        int Len = Bullet.Length;
        for(int i=0;i< Len; i++)
        {
            Destroy(Bullet[i].gameObject);
        }
        Len = BulletEffect.Length;
        for (int i = 0; i < Len; i++)
        {
            Destroy(BulletEffect[i].gameObject);
        }
    }
    void AttackTarget()
    {

        //transform.LookAt(Target.position);
        RaycastHit hit;
        GunFire.transform.LookAt(GameManager.instance.Char_Player_Attack.transform.position);
        
        
        if (Physics.Raycast(GunFire.transform.position, GunFire.transform.forward, out hit))//플레이어 어택포지션위치 높아지면 사격안나갈수있음
        {
            
            
            if (hit.collider.gameObject.layer == 9)
            {
                MuzzleFlash.Play();
                AttackSound.Play();
                animator.SetTrigger("Attack");
                Delay = 0;
                
                Aggro = MaxAggro;
                Bullet[count].transform.position = GunFire.position;
                Bullet[count].transform.rotation = GunFire.rotation;
                Bullet[count].SetActive(true);
                Bullet[count].GetComponent<EnemySoldierBullet>().BulletAction();
                count++;
                if(count==Bullet.Length)
                {
                    count = 0;
                }
            }
            else
                LostOnTarget();
        }
        else
            LostOnTarget();
    }
    void NextWayMove()
    {
        if (Target == null & DelayTrigger >= 1.0f)
        {
                DelayTrigger = 0.0f;
                if (Waypoint.Length != 0)
                {
                    WayCount += 1;
                    if (WayCount >= Waypoint.Length)
                    {
                        WayCount = 0;
                    }
                    agent.SetDestination(Waypoint[WayCount].position);
                }
            
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        
        if (Target == null & DelayTrigger>=1.0f)
        {
            if (other.tag == "Patroll" & Waypoint[WayCount].gameObject.name==other.name)
            {
                DelayTrigger = 0.0f;
                if (Waypoint.Length != 0)
                {
                    WayCount += 1;
                    if (WayCount >= Waypoint.Length)
                    {
                        WayCount = 0;
                    }
                    agent.SetDestination(Waypoint[WayCount].position);
                    
                }
            }
        }
    }

}
