using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class EnemyOldTankMove : EnemyMove
{
    [SerializeField]
    Old_Tank_Manager _Manager;
    public Vector3 posi;
    public Transform PlayerPosition;
    public GameObject Targeting;
    public GameObject Cannon;
    public GameObject[] OldTankBullet;
    public GameObject[] EXP;

    public float Angle;
    public float SearchDis, AttackDis;

    public bool Check;

    Transform Pooling;

    public bool Live;
    ParticleSystem boom;
    public int State;
    public bool RotationCheck;
    public float TankSpeed;
    public Transform MYCANNON;
    //public BoxCollider BodySensor;
    //public Transform CannonHeadSensor;
    //public BoxCollider other;
    public Rigidbody RD;
    public EnemyOldTankHP HP;
    public AudioSource FireAudio;
    public AudioSource MoveAudio;
    bool SoundPlaying;
    int count;
    public bool BossArea;
    void Awake()
    {
        BossArea = false;
        Live = true;
        
        Angle = 150.0f;
        SearchDis = 1000.0f;
        AttackDis = 50.0f;
        Check = false;
        boom = Targeting.transform.GetChild(0).GetComponent<ParticleSystem>();
        SoundPlaying = false;
        RD = GetComponent<Rigidbody>();
        RD.drag = float.MaxValue;
        //Aggro = 0;
        //MaxDis = 20;
        //MaxAggro = 30;
        HP = GetComponent<EnemyOldTankHP>();
        count = 0;
        for (int i = 0; i < EXP.Length; i++)
        {
            OldTankBullet[i].transform.parent = null;
            EXP[i].transform.parent = null;
        }

    }



    public void FindPooling(Transform Posi)
    {
        Pooling = Posi;
        for (int i = 0; i < EXP.Length; i++)
        {
            OldTankBullet[i].transform.parent = Posi;
            EXP[i].transform.parent = Posi;
        };
    }
    public void TankMoveSoundControl()
    {
        if (agent.velocity != Vector3.zero & SoundPlaying != true)
        {
            MoveAudio.Play();
            SoundPlaying = true;
        }
        if (agent.velocity == Vector3.zero)
        {
            MoveAudio.Pause();
            SoundPlaying = false;
        }
    }

    float Distance_value;
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
    public override void Attack_Order()
    {
        
        //FireCannon();
        _Manager.Current_Delay = 0;
        
        StartCoroutine(DelayFire());
    }
    public override void Follow_Target()
    {
        TankMoveSoundControl();
        agent.SetDestination(_Manager.Target.position);

        agent.speed = 4.5f;

        float dis = Vector3.Distance(_Manager.Target.position, transform.position);
        if (dis > _Manager.MAX_Dis)
            _Manager.Aggro -= Time.deltaTime;
    }
    public override void Stop_Enemy()
    {
        agent.speed = 3.5f;
        agent.isStopped = false;
        agent.velocity = Vector3.zero;
    }
    public void FireCannon()
    {
        boom.Play();
        FireAudio.Play();
        StartCoroutine(Effectoff());

        OldTankBullet[count].transform.position = Targeting.transform.position;
        OldTankBullet[count].transform.rotation = Targeting.transform.rotation;
        OldTankBullet[count].SetActive(true);
        count++;
        if (count == EXP.Length)
        {
            count = 0;
        }
        //GameObject Clone = Instantiate(OldTankBullet, Targeting.transform.position, Targeting.transform.rotation);
        //Clone.transform.GetComponent<OldTankBullet>().Exp = EXP;
        //LostOnTarget();
    }
    IEnumerator DelayFire()
    {
        
        yield return new WaitForSeconds(2.0f);
        FireCannon();
        _Manager.Current_Delay = 0;


    }




    IEnumerator Effectoff()
    {
        yield return new WaitForSeconds(1.0f);
        boom.Stop();
    }






    public bool RobotSearchPlayer(Vector3 Pos, Vector3 forwardDir, float angle, Vector3 targetPos, float distance)//자기위치,전방벡터,앵글,타겟위치,최대탐색거리
    {
        targetPos = targetPos - Pos;
        Quaternion rot = Quaternion.AngleAxis(-angle, Vector3.up);
        Vector3 leftDir = rot * forwardDir;

        rot = Quaternion.AngleAxis(angle, Vector3.up);
        Vector3 rightDir = rot * forwardDir;

        // 공격 범위를 벗어났다면 false값을 리턴합니다.
        if (Vector3.Distance(Pos, targetPos) > distance)
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
//if (RobotSearchPlayer(transform.position, transform.forward, Angle, PlayerPosition.position, SearchDis))
//{
//    RaycastHit[] hits;
//    LayerMask PlayerLayer = 9;
//    LayerMask GroundLayer = 8;
//    Vector3 ta=PlayerPosition.position-transform.position;
//    hits =Physics.RaycastAll(transform.position, ta, AttackDis);
//    Array.Sort(hits, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance)); // cast한거거리순으로정렬
//    foreach (RaycastHit hit in hits)
//    {
//        if (hit.collider.gameObject.layer == PlayerLayer)
//        {
//            Target = GameManager.instance.Char_Player_Trace.transform;
//            LockOnTarget();
//        }

//        else if (hit.collider.gameObject.layer == GroundLayer)
//        {
//            break;
//        }
//    }


//}