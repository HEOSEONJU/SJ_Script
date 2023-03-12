using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMove2 : MonoBehaviour
{
    //카메라 관절
    public CharacterController controller;
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
    float X;//민감도적용
    float Y;//민감도적용
    Vector3 D;//캐릭터의회전할벡터
    public float Sensitivy = 10.0f;//마우스민감도
    float CameraMaxAngle;//y축 앵글각도
    public float horizontal;//좌우입력
    public float vertical;//전후입력

    [Header("부스터")]
    float JSpeed;//
    float CurrentSpeed;//캐릭터의현재속도
    float WalkSpeed = 4.0f;//캐릭터의걷기스피드
    float DashSpeed = 8.0f;//캐릭터의대쉬속도
    float RotateSpeed = 150f;//캐릭터의좌우회전속도
    
    public float Booster = 300, BoosterMAX = 300;//스태미나와스태미나최대치

    public bool IsGround = true;//캐릭터 지면감지
    
    public bool BoosterState = false;//캐릭터 대쉬감지

    Vector3 direction;


    
    
    Gun A;
    

    
    float walkSpeed = 2;
    float runSpeed = 6;
    float gravity=9.8f;
    
    
    
    //가속도
    float movementAcceleration = 1;
    Vector3 movement, finalMovement;




    //총상태에니메이션
    bool BoosterAni;
    bool FrontWalk;
    bool BackWalk;
    bool LeftWalk;
    bool RightWalk;




    public ParticleSystem LSmoke,RSmoke;
    bool smoke;
    public Transform target;//상체바라볼오브젝트의위치
    Vector3 relativeVec1; //상체회전벡터
    Vector3 relativeVec2;//총기회전벡터
    Vector3 relativeVec3;//왼쪽다리회전
    Vector3 relativeVec4;//우측다리회전

    //총기반동
    Vector3 recoilkickback;
    public Transform recoilCam;

    //커서록온
    //public CursorLockMode cursorLockMode = CursorLockMode.Locked;
    //public bool cursorVisible = false;

    void Start()
    {
        GameManager.instance.Char_Player_Trace = gameObject;//캐릭터위치추적위치
        GameManager.instance.Char_Player_Attack = transform.GetChild(2).gameObject;//캐릭터에게 공격할위치

        A =GetComponent<Gun>();
        Spine = CharAni.GetBoneTransform(HumanBodyBones.Spine);
        TurnX = 0;
        TurnY = 0;
        CurrentSpeed = WalkSpeed;
        
        movement = Vector3.zero;
        finalMovement =Vector3.zero;
        BoosterAni= FrontWalk = BackWalk = LeftWalk = RightWalk = false;
        CameraMaxAngle = 80.0f;
        relativeVec1 = new Vector3(0,-40,-100);
        relativeVec2 = new Vector3(0, 90, -45);
        relativeVec3 = new Vector3(0, 77, 70);
        relativeVec4 = new Vector3(20, -110, 70);
        recoilkickback = new Vector3(0.05f, 0.2f, .0f);
        smoke = false;
    }
    void Awake()
    {

        //Cursor.lockState = cursorLockMode;
        //Cursor.visible = cursorVisible;

        
    }
    void FixedUpdate()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
        TurnX += Input.GetAxis("Mouse X");
        TurnY += Input.GetAxis("Mouse Y");

        BoosterActive();//부스터
        ControlSmoke();
        CharAngleSetting();//회전각
        CharMoveDir();//방향으로이동
        CharLegAni();
        
    }
    IEnumerator StopBooster()
    {
       

        yield return new WaitForSeconds(5.0f);

        

    }
    void LateUpdate()
    {
        AnimationActive();//다리움직임
        CharAniMove();//캐릭터 상하체조준움직임
        RecoilBack();
    }
    
    void CharAniMove()
    {
        //마우스움직으로 캐릭터회전
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(D), Time.fixedDeltaTime);
        //상체움직임
        Spine.LookAt(target.position);
        Spine.rotation = Spine.rotation * Quaternion.Euler(relativeVec1);
        /*
        Transform LLeg=CharAni.GetBoneTransform(HumanBodyBones.LeftUpperLeg);
        Transform RLeg=CharAni.GetBoneTransform(HumanBodyBones.RightUpperLeg);
        LLeg.LookAt(MoveDir.forward);
        LLeg.rotation = LLeg.rotation * Quaternion.Euler(relativeVec3);
        RLeg.LookAt(MoveDir.forward);
        RLeg.rotation = RLeg.rotation * Quaternion.Euler(relativeVec4);
        */
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
        else if (horizontal > 0) 
        {
            RightWalk = true;
        }
        else if (horizontal < 0)
        {
            LeftWalk = true;
        }
        else if (vertical > 0) 
        {
            FrontWalk = true;
        }
        else if(!IsGround)
        {
            FrontWalk = false;
            LeftWalk = false;
            RightWalk = false;
            BackWalk = false;

        }
        else
        {
            FrontWalk = false;
            LeftWalk = false;
            RightWalk = false;
            BackWalk = false;
            BoosterAni = false;

        }
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

        
    }
    void CharMoveDir()
    {
        direction = new Vector3(horizontal, 0, vertical);
        //벡터크기1로변환
        direction = Vector3.ClampMagnitude(direction, 1f);
        //캐릭터의 앞방향으로 방향받기
        direction = MoveDir.transform.TransformDirection(direction);
        //속도 적용
        direction *= CurrentSpeed;
        //이동방향과속도과적용된 값 입력
        movement = transform.TransformDirection(direction);
        //땅에있는지 확인
        IsGround = controller.isGrounded;
        if (!IsGround) //공중이라면 추락
        {
            movement.y -= gravity * Time.fixedDeltaTime*30;
        }
        //러프함수로 역동감

        //이동




        finalMovement = Vector3.Lerp(finalMovement, movement, Time.fixedDeltaTime * movementAcceleration);
        controller.Move(finalMovement * Time.fixedDeltaTime);
        //controller.Move(movement * Time.fixedDeltaTime);

        if (BoosterState == true)
        {
            finalMovement = Vector3.Lerp(finalMovement, movement, Time.fixedDeltaTime * movementAcceleration);
            
        }

    }
    void BoosterActive()
    {

        if (IsGround) //달리기조건추가
        {
            if (BoosterState == false)
                Booster += Time.deltaTime;
            if (BoosterMAX < Booster)
            {
                Booster = BoosterMAX;
            }
        }
        if (BoosterState == true)
        {
            Booster -= Time.deltaTime * 10;
        }
 
        if (Booster <= 0)
        {
            StartCoroutine(StopBooster());
            BoosterState = false;
            CurrentSpeed = walkSpeed;
        }
        if (Input.GetKey(KeyCode.LeftShift) & Booster >= 0)
        {
            BoosterState = true;
            CurrentSpeed = DashSpeed;
        }
        else
        {
            BoosterState = false;
            CurrentSpeed = WalkSpeed;
            
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))//때면 달리기상태해제
        {
            BoosterState = false;
            CurrentSpeed = WalkSpeed;
            

        }
    }

    void ControlSmoke()
    {

        if(BoosterState == true & smoke==false)
        {
            LSmoke.Play();
            RSmoke.Play();
            smoke = true;
        }
        else if(BoosterState == false)
        {
            LSmoke.Stop();
            RSmoke.Stop();
            smoke = false;
        }

    }
    void AnimationActive()
    {
        
        CharAni.SetBool("FrontWalk", FrontWalk);
        CharAni.SetBool("BackWalk", BackWalk);
        CharAni.SetBool("LeftWalk", LeftWalk);
        CharAni.SetBool("RightWalk", RightWalk);
        CharAni.SetBool("Booster", BoosterAni);
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
