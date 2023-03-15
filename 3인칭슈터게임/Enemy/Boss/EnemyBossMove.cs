using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBossMove : EnemyMove
{
    [SerializeField]
    Boss_Manager _Manager;
    public Animator Shield;

    public Transform PlayerPosition;

    public bool Live;



    public float BossMoveSpeed;
    public float BossAngle;

    public Rigidbody RD;
    public EnemyBossHP HP;

    public bool StopRotate;
    [SerializeField]
    float RotateSpeed;
    public bool Attack;
    public bool Action;
    public float distance;
    public float StopMoveFunction;
    CharacterController Move;

    public ParticleSystem MelleEffect;
    public Transform LegPosi;
    public bool AnimationWalk;
    public int NUM = 0;

    [SerializeField]
    EnemyBossBulletAttack EBBA;
    [SerializeField]
    EnemyBossRocketAttack EBRA;
    void Awake()
    {
        Live = true;
        Attack = false;
        StopRotate = true;
        Action = false;
        StopMoveFunction = 0;

        RotateSpeed = 0;


        Move = GetComponent<CharacterController>();
        AnimationWalk = false;


    }

    private void Start()
    {
        PlayerPosition = GameManager.instance.Char_Player_Attack.transform;

    }
    void FixedUpdate()
    {
        FIxedShiledMseh();



    }
    void Update()
    {
        /*   */
        if (!HP.Live || GameManager.instance.Hp <= 0)
        {
            return;
        }
        /*
        if (HP.Live &GameManager.instance.Hp>0)
        {
            distance = Vector3.Distance(PlayerPosition.transform.position, transform.position);
            
            if (Action == false)
            {
                
                if (Attack == false & Action == false)
                {
                    PlayerSearchAngle();
                    MoveBoss();
                    StopRocket();

                }

                

            }


            transform.position = new Vector3(transform.position.x, 0.1f, transform.position.z);
        }
        */



    }
    public float Distance_value;
    public override float Distance()
    {

        return Distance_value;


    }
    public override void Cal_Dis()
    {
        if (_Manager.Target == null)
        {
            Distance_value = Mathf.Infinity;
            return;
        }

        Distance_value = Vector3.Distance(_Manager.Target.position, transform.position);
    }


    public void BossStop()
    {
        EBBA.StopAttack();
        EBBA.StopAttack();
        StopAllCoroutines();


    }
    public void StopRocket()
    {
        if (Distance_value <= 15)
        {
            EBRA.StopAttack();
        }
    }
    void FIxedShiledMseh()
    {
        Shield.transform.localPosition = Vector3.zero;
    }

    public override void Follow_Target()
    {
        Vector3 Move_Vector = Vector3.zero;
        Move_Vector += new Vector3(0, -9.81f, 0);
        Move_Vector += transform.forward.normalized;
        Move.Move(Move_Vector * BossMoveSpeed * Time.deltaTime);

        Range_Attack();
        if (RotateSpeed == 0)
        {
            RotateSpeed += Time.deltaTime;

            StartCoroutine(RotateCoroutine());
        }

    }

    public void Range_Attack()
    {
        if (Distance_value <= 100 & Distance_value > 15)
        {
            NUM = 2;
        }
        else if (Distance_value <= 200 & Distance_value > 30)
        {

            NUM = 3;
        }
        switch (NUM)
        {
            case 2:
                
                if (EBBA.count == 0 && EBBA.StartAttack == false)
                {
                    Debug.Log("발사");
                    EBBA.AttackBullet(GameManager.instance.Char_Player_Attack.transform);
                }
                else if (distance <= 200 & distance > 30)
                {
                    if (EBRA.StartFire == false & EBRA.count == 0)
                    {
                        EBRA.AttackRocket(GameManager.instance.Char_Player_Attack.transform);
                    }
                }
                break;
            case 3:
                if (EBRA.StartFire == false & EBRA.count == 0)
                {
                    EBRA.AttackRocket(GameManager.instance.Char_Player_Attack.transform);
                }
                break;
            default:
                break;
        }
    }

    void RotateBoss()
    {
        StartCoroutine(RotateCoroutine());   
        
    }

    IEnumerator RotateCoroutine()
    {
        RotateSpeed += Time.deltaTime / 3;
        while (RotateSpeed<=1) 
        {
            RotateSpeed += Time.deltaTime / 3;
            Vector3 dir = PlayerPosition.position - transform.position;
            dir = new Vector3(dir.x, 0, dir.z);
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), RotateSpeed);
            yield return null;
        }
        RotateSpeed = 0;
    }
    void PlayerSearchAngle()
    {
        
        if(!BossSearchPlayer(transform.position,transform.forward, BossAngle, PlayerPosition.transform.position) && StopRotate==true)
        {
            StopRotate = false;
            RotateBoss();
            
            
        }


    }

    public bool BossSearchPlayer(Vector3 Pos, Vector3 forwardDir, float angle, Vector3 targetPos)//자기위치,전방벡터,앵글,타겟위치
    {
        targetPos = targetPos - Pos;
        Quaternion rot = Quaternion.AngleAxis(-angle, Vector3.up);
        Vector3 leftDir = rot * forwardDir;

        rot = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 rightDir = rot * forwardDir;

        
        

        Vector3 _1 = Vector3.Cross(forwardDir, targetPos);
        Vector3 _2 = Vector3.Cross(leftDir, targetPos);
        Vector3 _3 = Vector3.Cross(rightDir, targetPos);

        // 전방벡터의 왼쪽에 위치하고, 왼쪽 벡터의 오른쪽에 배치가 되어 있다면 true값을 리턴합니다.
        if (_1.y <= 0 && _2.y >= 0)
            return true;

        // 전방벡터의 오른쪽에 위치하고, 오른쪽 벡터의 왼쪽에 배치가 되어 있다면 true값을 리턴합니다.
        if (_1.y >= 0 && _3.y <= 0)
            return true;


        return false;
    }



    public void AttackFunction()
    {
        StartCoroutine(AttackSelect());
        

    }

    public void MeleeAttackFunction()
    {
        AnimationWalk = false;
        Attack = true;
        animator.SetTrigger("Stamp");
        Shield.SetTrigger("Stamp");
        transform.GetChild(0).GetComponent<EnemyBossBulletAttack>().StopAttack();
        transform.GetChild(0).GetComponent<EnemyBossRocketAttack>().StopAttack();
        Invoke("LandingTrigger", 1.0f);
    }

    IEnumerator AttackSelect()
    {
        
        float DelayTime = 5.0f;
        int NUM = 0;
        if (Attack==false & Action == false)
        {


            
            if (distance < 15)
            {
                NUM = 1;
                
            }

            else if (distance <= 100& distance>=15)
            {
                
                NUM = 2;
            }



            else if (distance <= 200 & distance > 30)
            {

                NUM = 3;
            }

            

            if (NUM>= 1)
            {
                switch (NUM)
                {
                    case 1:
                        //MeleeAttackFunction();
                        
                        break;
                    case 2:
                        
                        EnemyBossBulletAttack tempBullet = transform.GetChild(0).GetComponent<EnemyBossBulletAttack>();
                        
                        if (tempBullet.count == 0 &&tempBullet.StartAttack == false)
                        {

                            tempBullet.AttackBullet(PlayerPosition);
                        }
                        else if (distance<=200 & distance>30)
                        {
                            EnemyBossRocketAttack tempRocTemp = transform.GetChild(0).GetComponent<EnemyBossRocketAttack>();
                            if (tempRocTemp.StartFire == false & tempRocTemp.count == 0)
                            {
                                tempRocTemp.AttackRocket(PlayerPosition);
                            }
                        }
                        
                        break;
                    case 3:
                        EnemyBossRocketAttack tempRoc = transform.GetChild(0).GetComponent<EnemyBossRocketAttack>();
                        if (tempRoc.StartFire == false & tempRoc.count==0)
                        {
                            tempRoc.AttackRocket(PlayerPosition);
                        }
                        break;
                    default:
                        
                        break;
                }
                

            }

        }
        
        yield return new WaitForSeconds(DelayTime);
        StartCoroutine(AttackSelect());



    }

    public void DoneAttack()
    {

        Attack = false;
        
        //StartCoroutine(AttackSelect());
    }
    public void FirstDoneAttack()
    {
        Invoke("DoneAttack", 1.0f);
    }

    public void StopWalking()
    {
        

        StopMoveFunction = 0.0f;
        MeleeAttackFunction();
        
    }
    public void LandingTrigger()
    {
        animator.SetTrigger("Landing");
        Shield.SetTrigger("Landing");
    }
    public void MeleeFunction()
    {
        MelleEffect.transform.position=LegPosi.position;
        MelleEffect.Play();
        Ray ray = new Ray();
        ray.direction = Vector3.up;
        ray.origin = MelleEffect.transform.position;
        if(Physics.SphereCast(ray,5.0f))
        {
            RaycastHit[] hitInfos;
            int hit = 0;
            hitInfos = Physics.SphereCastAll(ray, 10.0f);
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].collider.gameObject.layer == 9)
                {
                    hit++;
                }
            }
            if (hit > 0)
            {
                GameManager.instance.PlayerDamage(400);
            }

        }

    }


    public void OverHeat()
    {
        StartCoroutine(OverHeatCoroutine());
    }

    

    IEnumerator OverHeatCoroutine()
    {
        animator.SetTrigger("OverHeat");
        Shield.SetTrigger("OverHeat");
        Action = true;
        yield return new WaitForSeconds(15.0f);
        

        animator.SetTrigger("Cool");
        Shield.SetTrigger("Cool");
    }

    public void CoolDownAction()
    {
        Action = false;
        Attack = false;
    }

}

