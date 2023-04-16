using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Knight_AI : MonoBehaviour
{
    [SerializeField]
    Knight_Enemy _Knight_Enemy;

    public Rigidbody _Rigidbody;
    [SerializeField]
    float DirectionRadius = 5;
    [SerializeField]
    float Detect_MaxAngle = 50;
    


    [SerializeField]
    float Attack_MaxAngle = 30;
    
    [SerializeField]
    LayerMask DetectLayer;
    [SerializeField]
    float RotateSpeed = 1;


    public NavMeshAgent Agent;
    [SerializeField]
    float MinDistance;
    [SerializeField]
    float MaxDistance;
    public Transform Current_Player;
    public float Distance_Currennt_Player;


    [SerializeField]
    public List<string> AttackList;
    [SerializeField]
    float AttackDelay = 0;

    public  void Init()
    {
        
        Agent.speed = _Knight_Enemy.Move_Speed;

    }


    public bool Detection()
    {
        if (_Knight_Enemy.Aggro_Time > 0)
        {

            return true;
        }
        Collider[] colliders = Physics.OverlapSphere(transform.position, DirectionRadius, DetectLayer);
        Collider PM = Array.Find(colliders, x => x.GetComponent<Player_Manager>());
        if (PM!=null)
        {
            
            Vector3 TargetDirection = PM.transform.position - transform.position;
            float Angle = Vector3.Angle(TargetDirection, transform.forward);
            
            if (Angle < Detect_MaxAngle)
            {
                _Knight_Enemy.Aggro_Time = 15;
                Current_Player = PM.gameObject.transform;
                return true;
            }
        }
        Current_Player = null;
        return false;
    }

    public bool Move_Target(Animator _Animator)
    {
        if(Current_Player==null)
        {
            return false;
        }
        Vector3 Dir = Current_Player.position - transform.position;
        Distance_Currennt_Player = Vector3.Distance(Current_Player.position, transform.position);
        if(Distance_Currennt_Player> MaxDistance)
        {
            _Animator.SetFloat("Attack_Range", 0);
            Current_Player = null;
            return false;
        }


        _Animator.SetFloat("Attack_Range", Distance_Currennt_Player);
        
        _Knight_Enemy.Knight_Animator._Animator.SetFloat("Vertical", 1, 0.1f, Time.deltaTime);
        Rotate_Target();
        Agent.transform.localPosition = Vector3.zero;
        Agent.transform.localRotation = Quaternion.identity;
        return true;
    }
    public void Move_OFF()
    {
        _Knight_Enemy.Knight_Animator._Animator.SetFloat("Vertical", 0, 0.1f, Time.deltaTime);
        Agent.velocity = Vector3.zero;
        Agent.enabled = false;
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


    public bool Attack_Think()
    {
        if (AttackDelay > 0)
        {
            return false;
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

                string AttackOrder = AttackList[UnityEngine.Random.Range(0, AttackList.Count)];
                _Knight_Enemy.Knight_Animator.PlayerTargetAnimation(AttackOrder, true);
                AttackDelay += 5;
                return true;
                
        }
        return false;
    }

    public bool Attack_Angle()
    {

        Vector3 TargetDirection = Current_Player.transform.position - transform.position;
        float Angle = Vector3.Angle(TargetDirection, transform.forward);


        if (Angle < Attack_MaxAngle)
        {
            return true;
        }
        return false;

    }
    private void Update()
    {
        _Knight_Enemy.Aggro_Time -= Time.deltaTime;
    }
    public void Update_Attack_Delay(float delta)
    {
        AttackDelay -= delta;
        
    }


}
