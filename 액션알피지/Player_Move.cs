using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Move : MonoBehaviour
{
    
    Player_Manager manager;
    Player_Input _Input;
    Player_Animator _Animator;
    [SerializeField]
    private LayerMask LandLayer;
    [SerializeField]
    float Player_Height_Distance = 1.0f;

    public float WalkSpeed = 12.5f;
    public float MoveSpeed = 25;
    public float SprintSpeed = 50;
    public float CurrentSpeed = 0;
    public Rigidbody rb;
   
    Vector3 MoveDir;
    [SerializeField]
    Vector3 Normal_Vec;
    Vector3 projectVeclocity;
    Transform Player_Camera;
    [SerializeField]
    float Jump_Height = 2;
    [SerializeField]
    float rotationSpeed = 10;

    public float Gravity_Drag=20;
    [SerializeField]
    float AirTimer = 0;
    public bool Airup;

    public void Init()
    {
        manager = GetComponent<Player_Manager>();
        _Input=GetComponent<Player_Input>();
        _Animator= GetComponent<Player_Animator>();
        Player_Camera = Camera.main.transform;
        rb=GetComponent<Rigidbody>();
        
        CurrentSpeed = MoveSpeed;
        //ignoreLayer = ~(1 << 8 | 1 << 9 | 1 << 10);
    }

    public void SpeedSetting()
    {
        
        if (manager.SprintState)
        {
            if (manager.IsGround)
            {
                CurrentSpeed = SprintSpeed;
                return;
            }
            
        }
        else if(manager.WalkState)
        {
            if (manager.IsGround)
            {
                CurrentSpeed = WalkSpeed;
                return;
            }
            
        }
        
            CurrentSpeed = MoveSpeed;
        
    }

    public bool GC = false;
    public void CalDir()
    {
        


        MoveDir = Player_Camera.transform.forward * _Input.Vertical;
        MoveDir += Player_Camera.transform.right * _Input.Horizontal;
        MoveDir.y = 0;
        MoveDir.Normalize();
    }

    public void CharRotate(float delta)
    {
        if (!manager.Rotate)
        {
            return;
        }
        if(manager.IsInteracting)
        {
            return;
        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(0).IsTag("Landing"))
        {
            return;

        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(0).IsTag("Stop_Move"))
        {
            return;
        }

 

        Vector3 targetDir = Vector3.zero;
        float moveOverrride = _Input.MoveAmount;

        targetDir = Player_Camera.forward * _Input.Vertical;
        targetDir += Player_Camera.right * _Input.Horizontal;


        targetDir.Normalize();
        targetDir.y = 0;
        if (targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }
        float rs;
        if (manager.IsGround)
        {
            rs = rotationSpeed;
        }
        else
        {
            rs = rotationSpeed / 15f;
        }

        if (manager.LockOnMode)
        {

            transform.LookAt(manager._Camera.CurrentLockonTarget);
            Quaternion temp = transform.rotation;
            temp.Set(0, temp.y, 0, temp.w);
            transform.rotation = temp;
            targetDir = transform.forward;


            return;
        }

        Quaternion tr = Quaternion.LookRotation(targetDir);
        Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, rs * delta);

        transform.rotation = targetRotation;
    }
    public void GroundMove()
    {

        if (_Animator._animator.GetCurrentAnimatorStateInfo(0).IsTag("Landing"))
        {
            return;

        }
        if (_Animator._animator.GetCurrentAnimatorStateInfo(0).IsTag("Stop_Move"))
        {
            return;
        }
        if (manager.IsGround & !manager.IsInteracting)
        {





            //rb.useGravity = false;
            
            
            
            projectVeclocity = Vector3.ProjectOnPlane(MoveDir, Normal_Vec);
            if (Normal_Vec.y == 1.0f)
            {
                rb.AddForce(Vector3.down);
            }
            //rb.AddForce(projectVeclocity.normalized * CurrentSpeed, ForceMode.Force);
            rb.velocity = projectVeclocity.normalized * CurrentSpeed/10f;
            //힘을주면 대각으로 올라갈때 남아있던 y축 벨로시티값에 공중에뜸 그래서 벨로시티수정으로 바꿈
            //rb.AddForce(projectVeclocity, ForceMode.VelocityChange);



        }
    }
    
    public void Falling()
    {


        if (!manager.IsGround && !manager.IsInteracting)
        {
            _Animator.PlayerTargetAnimation("Falling",false);
        }
        RaycastHit hit;
        Vector3 origin = transform.position;
        //Debug.DrawRay(origin, -Vector3.up * 1.2f, Color.black);

        if(!manager.IsGround)
        {
            AirTimer+=Time.deltaTime;
            if (_Input.MoveAmount > 0.1f)
            {
                rb.AddForce(transform.forward * CurrentSpeed / 100, ForceMode.Force);
            }
        }
        

        if (Physics.SphereCast(origin,0.2f, -Vector3.up, out hit, Player_Height_Distance, LandLayer))
        {

            if (!manager.IsGround & AirTimer>1.5f)//착지한순간
            {
                
                _Input.JumpCoolTime();


                _Animator.PlayerTargetAnimation("Landing",true);
            }
            else if(!manager.IsGround)
            {
                //_Animator.PlayerTargetAnimation("Empty_IDLE", false);
            }
            Airup = false;
            Normal_Vec = hit.normal;
            manager.IsGround = true;
            AirTimer = 0;
            
            
        }
        else
        {
            manager.IsGround = false;
        }

        

    }
    public void Jump_Function()
    {
        if (manager.Jump )
        {
            
            rb.AddForce(Vector3.up * Jump_Height, ForceMode.Impulse);
            _Animator.PlayerTargetAnimation("Jump",true);
            manager.Jump = false;
            
        }
    }

    public void Speed_Control()//속도 매그니튜드 초과방지
    {
        Vector3 temp = rb.velocity;

   


        temp.y = 0;
        if(temp.magnitude> CurrentSpeed)
        {
            temp.Normalize();
            temp *= CurrentSpeed;
            rb.velocity = new Vector3(temp.x, rb.velocity.y, temp.z);
        }

        
        

        if(manager.IsGround)
        {
            rb.drag = Gravity_Drag;
        }
        else
        {
            rb.drag = Gravity_Drag;
        }

    }

}
