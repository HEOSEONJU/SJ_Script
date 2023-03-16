using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotMove : MonoBehaviour
{
    public     EnemyRobotAttack A1, A2, A3, A4;
    public Animator A1Animation;
    public Transform PlayerPosition;
    public GameObject Targeting;
    public float Angle;
    public float Dis;
    public bool TracePlayer,IdleTime;
    public bool Check;
    float Delay;
    float AttackDelay;
    public bool Live;
    int hp;
    public bool RotationCheck;
    
    void Awake()
    {
        Live = true;
        GameObject body = transform.GetChild(1).transform.GetChild(0).transform.GetChild(0).transform.GetChild(0).gameObject;
        A1 = body.transform.GetChild(0).transform.GetChild(0).GetComponent<EnemyRobotAttack>();
        A2 = body.transform.GetChild(1).transform.GetChild(0).GetComponent<EnemyRobotAttack>();
        A3 = body.transform.GetChild(4).transform.GetChild(0).GetComponent<EnemyRobotAttack>();
        A4 = body.transform.GetChild(5).transform.GetChild(0).GetComponent<EnemyRobotAttack>();
        A1Animation = GetComponent<Animator>();
        
        AttackDelay = 6.0f;
        Delay = AttackDelay;
        Targeting = transform.GetChild(2).gameObject;
        Angle = 135.0f;
        Dis = 50.0f;
        IdleTime = false;
        TracePlayer = false;
        Check=false;
        hp = 100;
        
    }

    public void Update()
    {
        if(hp<100)
        {
            Live = false;
        }
        if (Live)
        {
            Delay -= Time.deltaTime;
            PlayerPosition = GameManager.instance.Char_Player_Trace.transform;
            Check = RobotSearchPlayer(transform.position, transform.forward, Angle, PlayerPosition.position, Dis);
            if (Check == true & TracePlayer == false & IdleTime == false)
            //if (Check == true & TracePlayer == false)
            {
                StartCoroutine(FindPlayer());
            }

            if (TracePlayer)
            {

                Targeting.transform.LookAt(PlayerPosition);
                transform.rotation = Quaternion.Lerp(transform.rotation, Targeting.transform.rotation, Time.deltaTime * 3.0f);
                //transform.LookAt(PlayerPosition);
                //transform.Rotate(transform.rotation.x*2.0f,0.0f,0.0f);
                //transform.rotation = Quaternion.Euler(0.0f,transform.rotation.y, 0.0f);

                if (Delay <= 0 )
                {
                    AttackPlayerbullet();
                    Delay = AttackDelay;
                }
            }
        }
        else
        {
            StopCoroutine(AttackPlayerCoroutine());
        }
    }
    public void AttackPlayerbullet()
    {
        StartCoroutine(AttackPlayerCoroutine());
        
    }
    IEnumerator AttackPlayerCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        A1Animation.SetTrigger("ShotFirstAction");
        yield return new WaitForSeconds(0.1f);
        A1.DangerMarker(GameManager.instance.Char_Player_Attack.transform);
        A2.DangerMarker(GameManager.instance.Char_Player_Attack.transform);
        
        
        yield return new WaitForSeconds(0.5f);
        A1Animation.SetTrigger("ShotSecondAction");
        yield return new WaitForSeconds(0.1f);
        A3.DangerMarker(GameManager.instance.Char_Player_Attack.transform);
        A4.DangerMarker(GameManager.instance.Char_Player_Attack.transform);
        
    }
    





    IEnumerator FindPlayer()
    {
        TracePlayer = true;
        
        yield return new WaitForSeconds(5.0f);
        TracePlayer = false;
        IdleTime = true;
        yield return new WaitForSeconds(3.0f);
        IdleTime = false;
    }
    public bool RobotSearchPlayer(Vector3 playerPos,Vector3 forwardDir,float angle,Vector3 targetPos,float distance)//플레이어위치,전방벡터,앵글,타겟위치,최대탐색거리
    {
        Quaternion rot = Quaternion.AngleAxis(-angle, Vector3.up);
        Vector3 leftDir = rot * forwardDir;

        rot = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 rightDir = rot * forwardDir;

        // 공격 범위를 벗어났다면 false값을 리턴합니다.
        if (Vector3.Distance(playerPos, targetPos) > distance)
            return false;

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


}



