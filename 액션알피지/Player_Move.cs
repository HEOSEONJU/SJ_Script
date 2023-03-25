using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

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
    public float AirSpeed;
    public float GravitySpeed;
    public Rigidbody rb;

    Vector3 MoveDir;
    [SerializeField]
    Vector3 Normal_Vec;
    [SerializeField]
    Vector3 projectVeclocity;
    Transform Player_Camera;
    [SerializeField]
    float Jump_Height = 2;
    [SerializeField]
    float rotationSpeed = 10;
    [SerializeField]
    float Power;
    public float Gravity_Drag = 20;
    [SerializeField]
    float AirTimer = 0;
    public bool Airup;


    [SerializeField]
    float Control_Ground_Move;
    
    public void Init()
    {
        manager = GetComponent<Player_Manager>();
        _Input = GetComponent<Player_Input>();
        _Animator = GetComponent<Player_Animator>();
        Player_Camera = Camera.main.transform;
        //rb=GetComponent<Rigidbody>();

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
        else if (manager.WalkState)
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
        Power= _Input.Vertical + _Input.Horizontal;
        Power = Mathf.Clamp(Power, 0, 1);

        MoveDir = Player_Camera.transform.forward * _Input.Vertical;
        
        
        MoveDir += Player_Camera.transform.right * _Input.Horizontal;

        //MoveDir = this.transform.TransformDirection(Player_Camera.transform.GetChild(0).forward * _Input.Vertical);
        //MoveDir += this.transform.TransformDirection(Player_Camera.transform.GetChild(0).right *-1f * _Input.Horizontal);
        MoveDir.y = 0;
        //MoveDir.Normalize();
        //MoveDir.y = 0;
    }

    public void CharRotate(float delta)
    {
        if (!manager.Rotate)
        {
            return;
        }
        if (manager.IsInteracting)
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



        Vector3 targetDir = MoveDir;
        float moveOverrride = _Input.MoveAmount;

        //targetDir = Player_Camera.forward * _Input.Vertical;
        //targetDir += Player_Camera.right * _Input.Horizontal;


        //targetDir.Normalize();
        //targetDir.y = 0;
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
    [SerializeField]
    float V_value;
    public void GroundMove()
    {
        //Debug.Log("지상");

        Vector3 T = new Vector3(MoveDir.x, 0, MoveDir.z).normalized*CurrentSpeed;
        //Vector3 tRelativeVelocity = this.transform.TransformDirection(MoveDir);
        //rb.velocity = tRelativeVelocity;
        T.y = 0;
        rb.velocity = T;
        V_value = new Vector3(rb.velocity.x,0, rb.velocity.z).magnitude;
        return;
        #region 구 코드
        projectVeclocity = Vector3.ProjectOnPlane(MoveDir, Normal_Vec).normalized * CurrentSpeed;




        if (projectVeclocity.y == 0) projectVeclocity.y = Physics.gravity.y;
        //rb.AddForce(projectVeclocity, ForceMode.Force);
        //Vector3 Temp_Vector = rb.velocity;

        //Temp_Vector.x = Mathf.Clamp(Temp_Vector.x, -Control_Ground_Move, Control_Ground_Move);
        //Temp_Vector.z = Mathf.Clamp(Temp_Vector.z, -Control_Ground_Move, Control_Ground_Move);

        //Vector3 MoveValue = projectVeclocity.normalized * CurrentSpeed/ Control_Ground_Move;
        //MoveValue.y += Physics.gravity.y;
        //rb.velocity = MoveValue;


        return;


        /*
         * 
         * if (Normal_Vec.y == 1.0f)
        {
            rb.AddForce(Vector3.down);
        }
         * 
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
            
            
            
            
            //힘을주면 대각으로 올라갈때 남아있던 y축 벨로시티값에 공중에뜸 그래서 벨로시티수정으로 바꿈
            //rb.AddForce(projectVeclocity, ForceMode.VelocityChange);



        }
        */
        #endregion
    }

    public void Jumping()
    {
        //Debug.Log("상승");

        Vector3 T = rb.velocity;
        T.x = transform.forward.x * AirSpeed * Power;
        T.z = transform.forward.z * AirSpeed * Power;
        T.y = Jump_Height;
        rb.velocity = T;
        //rb.AddForce(T * Time.deltaTime, ForceMode.Force);
    }
    public void Jumping_Move()
    {
        //Debug.Log("점프?");
        Vector3 T = rb.velocity;
        T.x = transform.forward.x * AirSpeed * Power;
        T.z = transform.forward.z * AirSpeed * Power;
        rb.velocity = T;
        
    }
    public void FallingStart()
    {
        Vector3 T = Vector3.zero;
        T = rb.velocity;

        T.x = transform.forward.x * AirSpeed * Power;
        T.z = transform.forward.z * AirSpeed * Power;
        T.y += Physics.gravity.y * Time.deltaTime;
        //rb.AddForce(T * Time.deltaTime, ForceMode.Force);
    }

    public void Falling()
    {
        
        Vector3 T;
        T = rb.velocity;
        Ray ray = new Ray(transform.position, Vector3.down);
        //Debug.Log("추락");
        T.y += Physics.gravity.y * Time.deltaTime;
        T.y = Mathf.Clamp(T.y, -53, 0);
        if (Physics.SphereCast(ray, 0.2f, out RaycastHit hitInfo, (1.8f / 2f) + 0.2f))//원형미끌어짐생성
        {
            //Debug.Log(Vector3.Angle(Vector3.down, hitInfo.normal)); //충돌각
            if (Vector3.Angle(Vector3.down, hitInfo.normal) < 135) //135도이상이 나오면 걸어서올라갈수있는각도값일때 나옴 미끌어질필요없음
            {
                T.x += hitInfo.normal.x;
                T.z += hitInfo.normal.z;
                rb.velocity = T;
                return;
            }

        }
        
            T.x = transform.forward.x * AirSpeed * Power;
            T.z = transform.forward.z * AirSpeed * Power;
        

        rb.velocity = T;
        return;

        //rb.AddForce(Vector3.down, ForceMode.Force);

        #region 구코드
        if (!manager.IsGround && !manager.IsInteracting)
        {
            _Animator.PlayerTargetAnimation("Falling", false);
        }
        RaycastHit hit;
        Vector3 origin = transform.position;
        //Debug.DrawRay(origin, -Vector3.up * 1.2f, Color.black);

        if (!manager.IsGround)
        {
            AirTimer += Time.deltaTime;
            if (_Input.MoveAmount > 0.1f)
            {
                //rb.AddForce(transform.forward * CurrentSpeed / 100, ForceMode.Force);
            }
        }


        if (Physics.SphereCast(origin, 0.2f, -Vector3.up, out hit, Player_Height_Distance, LandLayer))
        {

            if (!manager.IsGround & AirTimer > 1.5f)//착지한순간
            {

                _Input.JumpCoolTime();

                Debug.Log("착지");
                _Animator.PlayerTargetAnimation("Landing", true);
            }
            else if (!manager.IsGround)
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

        #endregion



    }
    public void Jump_Function()
    {
        if (manager.Jump)
        {
            //점프펑션다시만들기
            //rb.AddForce(Vector3.up * Jump_Height, ForceMode.Impulse);
            
            _Animator._animator.CrossFade("Jump", 0f);
            //_Animator.PlayerTargetAnimation("Jump", true);
            manager.Jump = false;

        }
    }

    public void Speed_Control()//속도 매그니튜드 초과방지
    {

        /*
        Vector3 temp = rb.velocity;




        temp.y = 0;
        if (temp.magnitude > CurrentSpeed/15)
        {
            
            temp = temp.normalized * CurrentSpeed/15;

            rb.velocity = new Vector3(temp.x, rb.velocity.y, temp.z);
        }
    }
        */
    }
}
