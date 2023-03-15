using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Knight_AI : MonoBehaviour
{
    Knight_Enemy _Knight_Enemy;

    public Rigidbody _Rigidbody;
    [SerializeField]
    float DirectionRadius = 5;
    [SerializeField]
    float Detect_MaxAngle = 50;
    [SerializeField]
    float Detect_MinAngle = -50;


    [SerializeField]
    float Attack_MaxAngle = 30;
    [SerializeField]
    float Attack_MinAngle = -30;
    [SerializeField]
    LayerMask DetectLayer;
    [SerializeField]
    float RotateSpeed = 1;


    public NavMeshAgent Agent;
    [SerializeField]
    float MinDistance;
    public Transform Current_Player;
    public float Distance_Currennt_Player;


    [SerializeField]
    public List<string> AttackList;
    [SerializeField]
    float AttackDelay = 0;

    private void Start()
    {
        _Knight_Enemy = GetComponent<Knight_Enemy>();
        _Rigidbody = GetComponent<Rigidbody>();
        Agent = GetComponentInChildren<NavMeshAgent>();
        Agent.speed = _Knight_Enemy.Move_Speed;

    }


    public void Detection()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, DirectionRadius, DetectLayer);

        for (int i = 0; i < colliders.Length; i++)
        {

            Player_Manager temp = colliders[i].transform.GetComponent<Player_Manager>();
            if (temp != null)
            {

                Vector3 TargetDirection = temp.transform.position - transform.position;
                float Angle = Vector3.Angle(TargetDirection, transform.forward);


                if (Angle > Detect_MinAngle && Angle < Detect_MaxAngle)
                {
                    Current_Player = temp.transform;
                }
            }
        }

    }

    public void Move_Target()
    {
        Vector3 Dir = Current_Player.position - transform.position;
        Distance_Currennt_Player = Vector3.Distance(Current_Player.position, transform.position);
        float Angle = Vector3.Angle(Dir, transform.forward);

        switch (_Knight_Enemy.IsInteracting)
        {
            case true:
                _Knight_Enemy.Knight_Animator._Animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
                Agent.enabled = false;
                break;
            case false:
                if (Current_Player != null)
                {
                    if (Distance_Currennt_Player > MinDistance)
                    {
                        _Knight_Enemy.Knight_Animator._Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                    }
                    else
                    {
                        Agent.velocity = Vector3.zero;
                        Attack_Think();
                    }
                }
                break;
        }
        Rotate_Target();
        Agent.transform.localPosition = Vector3.zero;
        Agent.transform.localRotation = Quaternion.identity;
    }
    public void Rotate_Target()
    {
        switch(_Knight_Enemy.IsInteracting)
        {
            case true:
                return;
            case false:
                _Knight_Enemy.Knight_Animator._Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
                Vector3 Dir = transform.InverseTransformDirection(Agent.desiredVelocity);
                Vector3 TargetVelocity = _Rigidbody.velocity;

                Agent.enabled = true;
                Agent.SetDestination(Current_Player.position);
                _Rigidbody.velocity = Agent.velocity;

                transform.rotation = Quaternion.Slerp(transform.rotation, Agent.transform.rotation, RotateSpeed / Time.deltaTime);
                break;

        }

    }


    public void Attack_Think()
    {
        if (_Knight_Enemy.IsInteracting || AttackDelay > 0)
        {
            return;
        }
        switch(Attack_Angle())
        {
            case true:
                Vector3 Dir = Current_Player.transform.position - transform.position;
                Dir.y = 0;
                Dir.Normalize();

                if (Dir == Vector3.zero)
                {
                    Dir = transform.forward;
                }

                Quaternion TargetRotation = Quaternion.LookRotation(Dir);
                transform.rotation = TargetRotation;

                int temp = Random.Range(0, AttackList.Count);

                string AttackOrder = AttackList[temp];

                _Knight_Enemy.Knight_Animator.PlayerTargetAnimation(AttackOrder, true);
                AttackDelay += 5;
                break;
        }

    }

    public bool Attack_Angle()
    {

        Vector3 TargetDirection = Current_Player.transform.position - transform.position;
        float Angle = Vector3.Angle(TargetDirection, transform.forward);


        if (Angle > Attack_MinAngle && Angle < Attack_MaxAngle)
        {
            return true;
        }
        return false;

    }

    public void Update_Attack_Delay(float delta)
    {
        AttackDelay -= delta;
    }


}
