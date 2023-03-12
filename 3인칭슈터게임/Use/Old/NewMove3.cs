using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMove3 : MonoBehaviour
{
    //카메라 관절
    
    public Camera cam;
    
    public Animator CharAni;
    //상체
    Transform Spine;
    //총
    public Transform RightArm;
    //캐릭터가 전진시 진행할방향
    public Transform MoveDir;

    [Header("상하좌우이동 회전입력")]
    public float TurnX;//마우스X
    public float TurnY;//마우스Y
    public float X;//민감도적용
    public float Y;//민감도적용
    Vector3 D;//캐릭터의회전할벡터
    public float Sensitivy = 10.0f;//마우스민감도
    float CameraMaxAngle;//y축 앵글각도
    public float horizontal;//좌우입력
    public float vertical;//전후입력

    public Rigidbody Rd;

    [Header("부스터")]
    public float JumpHeight;//
    public float CurrentSpeed;//캐릭터의현재속도
    public float WalkSpeed = 4.0f;//캐릭터의걷기스피드
    public float DashSpeed = 8.0f;//캐릭터의대쉬속도
    
    float RotateSpeed = 150f;//캐릭터의좌우회전속도
    
    public float Booster = 300, BoosterMAX = 300;//스태미나와스태미나최대치
    
    
    public bool IsGround = true;//캐릭터 지면감지
    
    public bool BoosterState = false;//캐릭터 대쉬감지

    Vector3 direction;// 캐릭터방향
    
    Gun A;//총 스왚인지 확인


    //총상태에니메이션
    bool BoosterAni;
    bool FrontWalk;
    bool BackWalk;
    bool LeftWalk;
    bool RightWalk;
    bool Fly;
    


    //총기반동
    Vector3 recoilkickback;
    public Transform recoilCam;



    public ParticleSystem LSmoke,RSmoke; //대쉬연기이펙트
    bool smoke;//대쉬시 연기이펙트 온오프
    public Transform target;//상체바라볼오브젝트의위치
    Vector3 relativeVec1; //상체회전벡터
    Vector3 relativeVec2;//총기회전벡터
    //Vector3 relativeVec3;//왼쪽다리회전
    //Vector3 relativeVec4;//우측다리회전

    //버툰인식
    public bool JumpActive;
    public bool FlyActive;
    public bool FlyFunctionActive;
    public bool RESETFLYVELOCITY;

    //커서록온
    public CursorLockMode cursorLockMode = CursorLockMode.Locked;
    public bool cursorVisible = false;

    void Start()
    {
        GameManager.instance.Char_Player_Trace = gameObject;//캐릭터위치추적위치
        GameManager.instance.Char_Player_Attack = transform.GetChild(2).gameObject;//캐릭터에게 공격할위치
        
        A =GetComponent<Gun>();
        Spine = CharAni.GetBoneTransform(HumanBodyBones.Spine);
        TurnX = 0;
        TurnY = 0;
        CurrentSpeed = WalkSpeed;
        JumpHeight = 6.0f;
        
        BoosterAni= FrontWalk = BackWalk = LeftWalk = RightWalk = false;
        CameraMaxAngle = 80.0f;
        relativeVec1 = new Vector3(0,-40,-100);
        relativeVec2 = new Vector3(0, 90, -45);
        //relativeVec3 = new Vector3(0, 77, 70);
        //relativeVec4 = new Vector3(20, -110, 70);
        recoilkickback = new Vector3(0.05f, 0.2f, .0f);
        smoke = false;
        JumpActive = false;
        FlyActive = false;
        FlyFunctionActive = false;
    }
    void Awake()
    {

        Cursor.lockState = cursorLockMode;
        Cursor.visible = cursorVisible;

        
    }
    void Update()
    {
        InputArrow();//방향키입력
        CharAngleSetting();//회전각
        CharJump();//점프버툰이식
        FlyInput();//공중부양입력
        BoosterActive();//부스터작동
        ChangeSpeed();//속도 변환
        ControlSmoke();//부스터상태면발바닥에서 연기발생
        
    }
    void FixedUpdate() 
    {
        CharMoveDir();//방향으로이동
        JumpFunction();//인식받은점프지역으로이동
        FlyFunction();//공중부양
        IsGrounded();//지면감지

    }
    void LateUpdate()
    {
        CharLegAni();
        AnimationActive();//다리움직임
        CharAniMove();//캐릭터 상하체조준움직임 상체회전까지
        RecoilBack();//총기반동
    }
    void InputArrow()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        TurnX += Input.GetAxis("Mouse X");
        TurnY += Input.GetAxis("Mouse Y");
    }
    void IsGrounded()
    {
        RaycastHit hit;
        Vector3 RayPoint = transform.position;
        RayPoint = new Vector3(RayPoint.x, RayPoint.y + 1.0f, RayPoint.z);
        CapsuleCollider col = GetComponent<CapsuleCollider>();
        IsGround = Physics.Raycast(RayPoint, Vector3.down, out hit, col.bounds.extents.y + 0.2f);
        
    }
    void CharAniMove()
    {
        //마우스움직으로 캐릭터회전
        
        //상체움직임
        Spine.LookAt(target.position);
        Spine.rotation = Spine.rotation * Quaternion.Euler(relativeVec1);
        //총 스왚에외는 조준
        if (A.Swaping == false)
        {
            RightArm.LookAt(target.position);
            RightArm.rotation = RightArm.rotation * Quaternion.Euler(relativeVec2);
        }
    }
    void CharLegAni()
    {
        if( BoosterState==true)
        {
            BoosterAni = true;
        }
        else if(BoosterState == false)
        {
            BoosterAni = false;
        }
        if (vertical < 0)
        {
            BackWalk = true;
        }
        else
        {
            BackWalk= false;
        }
        if (horizontal > 0) 
        {
            RightWalk = true;
        }
        else
        {
            RightWalk = false;
        }
        if (horizontal < 0)
        {
            LeftWalk = true;
        }
        else
        {
            LeftWalk = false;
        }
        if (vertical > 0) 
        {
            FrontWalk = true;
        }
        else
        {
            FrontWalk = false;
        }


        if(!IsGround)
        {
            FrontWalk = false;
            LeftWalk = false;
            RightWalk = false;
            BackWalk = false;

        }
        //else
        //{
        //    FrontWalk = false;
        //    LeftWalk = false;
        //    RightWalk = false;
        //    BackWalk = false;
        //    BoosterAni = false;

        //}
    }
    void CharAngleSetting()
    {
        //민감도적용
        X = -TurnY * Sensitivy;
        Y = TurnX * Sensitivy;
        //카메라 최대각도적용
        X = Mathf.Clamp(X, -CameraMaxAngle, CameraMaxAngle);
        //캐릭터움직이는방향회전
        D = new Vector3(0, Y, 0f);

        transform.rotation = Quaternion.Euler(D);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(D), Time.fixedDeltaTime);
    }
    public float power;
    public float CPower;
    public float ch=1;
    void CharMoveDir()
    {

        direction = new Vector3(horizontal, 0, vertical);

        CPower = direction.magnitude;
        
            Rd.AddRelativeForce(direction * 50.0f);

        Vector3 Power;

        Power = new Vector3(Mathf.Clamp(Rd.velocity.x, -CurrentSpeed, CurrentSpeed),
                                    Rd.velocity.y,
                                    Mathf.Clamp(Rd.velocity.z, -CurrentSpeed, CurrentSpeed));
        

        


        float VScale = direction.magnitude;
        if (BoosterState == false & VScale < 1.0f & CurrentSpeed <= 4.5)
        {
            Power = new Vector3(0.0f, Rd.velocity.y, 0.0f);
        }
        Rd.velocity = Power;


    }
    void CharJump()
    {
        
        if (Input.GetKeyDown(KeyCode.Space)& Fly == false & IsGround)
        {
            JumpActive = true;
        }
        if (Input.GetKey(KeyCode.Space)&Fly==true &Booster>=0)
        {
            FlyActive = true;  
        }
        else
        {
            FlyActive = false;
        }
        if(Booster<=0)
        {
            FlyActive=false;
        }
    }
    void JumpFunction()
    {
        Vector3 JV;
        if (JumpActive)
        {
            JumpActive = false;
            JV = Vector3.up * Mathf.Sqrt(JumpHeight * -Physics.gravity.y);
            CharAni.SetTrigger("Jump");
            Rd.AddForce(JV, ForceMode.VelocityChange);
            Booster -= 30.0f;
        }
        if(FlyActive &Booster>=0)
        {
            Booster -= Time.deltaTime * 35;
            JV = Vector3.up * Mathf.Sqrt(JumpHeight / 50 * -Physics.gravity.y);
            Rd.AddRelativeForce(JV);
        }

    }
    
    void FlyInput()
    {
        if(Input.GetKeyDown(KeyCode.F) & Fly==false)
        {
            FlyFunctionActive = true;
            RESETFLYVELOCITY = true;

        }
        else if (Input.GetKeyDown(KeyCode.F) & Fly == true)
        {
            FlyFunctionActive = false;
        }
    }
    void FlyFunction()
    {
        if (FlyFunctionActive == true)
        {
            
            Fly = true;
            Rd.useGravity = false;
            if(RESETFLYVELOCITY==true)
            {
                RESETFLYVELOCITY = false;
                Rd.velocity = new Vector3(Rd.velocity.x, 0.0f, Rd.velocity.z);
            }

        }
        if (FlyFunctionActive == false)
        {
            Fly = false;
            Rd.useGravity = true;
        }
    }
    void BoosterActive()
    {

        if (IsGround) //달리기조건추가
        {
            if (BoosterState == false & Fly==false)
                Booster += Time.deltaTime*40;
            if (BoosterMAX < Booster)
            {
                Booster = BoosterMAX;
            }
        }
        if (BoosterState == true)
        {
            Booster -= Time.deltaTime * 10;
        }
        if(Fly == true)
        {
            Booster -= Time.deltaTime * 10;
        }
        if ((Fly==true |BoosterState==true)& Booster <= 0)
        {
            //StartCoroutine(StopBooster());
            BoosterState = false;
            //CurrentSpeed = WalkSpeed;
            Fly = false;
            Rd.useGravity = true;
            FlyFunctionActive = false;
        }
        if (Input.GetKey(KeyCode.LeftShift) & Booster >= 0)
        {
            BoosterState = true;
            //CurrentSpeed = DashSpeed;
            //StopCoroutine(StopBooster());
            
        }
        
        if (Input.GetKeyUp(KeyCode.LeftShift))//때면 달리기상태해제
        {
            //StartCoroutine(StopBooster());
            BoosterState = false;

        }
    }
    void ChangeSpeed()
    {
        if(BoosterState == true)
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed,DashSpeed,Time.deltaTime);
        }
        else
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, WalkSpeed, Time.deltaTime);
        }

    }
    void ControlSmoke()
    {
        if (!IsGround)
        {
            LSmoke.Stop();
            RSmoke.Stop();
            smoke = false;
        }
        else
        {
            if (BoosterState == true & smoke == false)
            {
                LSmoke.Play();
                RSmoke.Play();
                smoke = true;
            }
            else if (BoosterState == false)
            {
                LSmoke.Stop();
                RSmoke.Stop();
                smoke = false;
            }
        }

    }
    void AnimationActive()
    {
        
        CharAni.SetBool("FrontWalk", FrontWalk);
        CharAni.SetBool("BackWalk", BackWalk);
        CharAni.SetBool("LeftWalk", LeftWalk);
        CharAni.SetBool("RightWalk", RightWalk);
        CharAni.SetBool("Booster", BoosterAni);
        CharAni.SetBool("Fly", Fly);
    }


    
    public void GunRecoilActive()
    {
        Vector3 recoilVector = new Vector3(Random.Range(-recoilkickback.x, recoilkickback.x), recoilkickback.y, recoilkickback.z);
        Vector3 recoilCamVector = new Vector3(-recoilVector.y*400.0f, recoilVector.x * 200.0f,0);

        //cam.transform.localPosition = Vector3.Lerp(transform.localPosition, transform.localPosition + recoilVector, Time.deltaTime);
        recoilCam.localRotation = Quaternion.Slerp(recoilCam.localRotation, Quaternion.Euler(recoilCam.localEulerAngles + recoilCamVector), Time.deltaTime);
        //cam.transform.localRotation = recoilCam.localRotation;
        //cam.transform.localPosition = new Vector3(2, cam.transform.localPosition.y, -3);
    }
    public void RecoilBack()
    {

        recoilCam.localRotation = Quaternion.Slerp(recoilCam.localRotation, Quaternion.identity, Time.deltaTime);
    }

}
