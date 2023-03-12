using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMove : MonoBehaviour
{
    ////public CursorLockMode cursorLockMode = CursorLockMode.Locked;
    ////public bool cursorVisible = false;
    //public CharacterController Controller;
    //public Animator SpineAni;
    //public Transform SpineTrans,GUN1,GUN2;
    //public GameObject MoveDir;
    //public GameObject Trace;
    //public Gun FirstGun;
    ////카메라관련
    //public float TurnX;
    //public float TurnY;
    //public float Sensitivy = 10.0f;//마우스민감도
    //float CameraMaxAngle = 30;//y축 앵글각도
    //public GameObject a;
    //public GameObject test;
    //public Transform cam;
    ////캐릭터움직임
    //public float horizontal;//좌우입력
    //public float vertical;//전후입력
    //public Transform waist;
    //public Vector3 velocity;
    //public Vector3 D,C,B;
    ////속도
    //public float JSpeed;
    //public float CurrentSpeed;
    //public float WalkSpeed = 4.0f;//캐릭터의걷기스피드
    //public float DashSpeed = 8.0f;//캐릭터의대쉬속도
    //public float RotateSpeed = 150f;//캐릭터의좌우회전속도
    //public float CSpeed = 8.0f;//캐릭터의기어가는속도
    //public float Booster = 300, BoosterMAX = 300;//스태미나와스태미나최대치
    //float LegAngle = 0;
    //float High;
    
    //float FlySpeed = 5;
    ////상태
    //public bool IsGround = true;//캐릭터 지면감지
    //public bool IsFlying = false;//캐릭터 날기감지
    //public bool Falling = false;
    //public bool BoosterState = false;

    ////구상태

    //public PlayerStatus PlayerInfo;//플레이어체력



    ////
    //[Header("Movement")]
    //public float walkSpeed = 2;
    //public float runSpeed = 6;
    //public float gravity;
    //[Space]
    //[Header("Look")]
    //public Transform cameraPivot;
    //public float lookSpeed = 45;
    //public bool invertY = true;
    //[Space]
    //[Header("Smoothing")]
    //public float movementAcceleration = 1;
    //public Vector3 movement, finalMovement;
    //float speed;
    //Quaternion targetRotation, targetPivotRotation;

    //public Vector3 ChestOffset = new Vector3(-40, -50, -40);
    //public Vector3 LeftLegOffset = new Vector3(0, -40, -100);
    //public Vector3 RightLegOffset = new Vector3(0, 4, -260);
    //Vector3 ChestDir = new Vector3();

    //void Start()
    //{
    //    TurnX = 0;
    //    TurnY = 0;
        
    //    SpineTrans = SpineAni.GetBoneTransform(HumanBodyBones.Spine);
    //    GUN1 = SpineAni.GetBoneTransform(HumanBodyBones.RightUpperArm);
    //    GUN2 = SpineAni.GetBoneTransform(HumanBodyBones.RightLowerArm);



    //    //MoveDir = transform.GetChild(2).GetComponent<Transform>();
    //    Controller = GetComponent<CharacterController>();
    //    CurrentSpeed = WalkSpeed;
    //    GameManager.instance.Char_Player_Trace = MoveDir;
    //    GameManager.instance.Char_Player_Attack = Trace;
    //}
    //void Awake()
    //{

    //    //Cursor.lockState = cursorLockMode;
    //    //Cursor.visible = cursorVisible;
        
    //    targetRotation = targetPivotRotation = Quaternion.identity;
    //}

    //// Update is called once per frame
    //void Update()
    //{



    //    horizontal = Input.GetAxis("Horizontal");
    //    vertical = Input.GetAxis("Vertical");




    //    if (IsGround) //달리기조건추가
    //    {
    //        if(BoosterState == false)
    //        Booster += Time.deltaTime;
    //        if (BoosterMAX < Booster)
    //        {
    //            Booster = BoosterMAX;
    //        }
    //    }
    //    if(BoosterState ==true)
    //    {
    //        Booster -= Time.deltaTime*10;
    //    }


    //    if (Input.GetKey(KeyCode.Space))
    //    {

    //        if (Booster >= 0.0f)
    //        {
    //            Booster -= Time.deltaTime;
    //            //gravity -= Time.deltaTime * 10f;
    //        }



    //    }

    //    else
    //    {
    //        //if (IsGround)
    //          //  gravity = 9.8f;


    //    }
        

    //    if (Booster <= 0)
    //    {
    //        StartCoroutine(StopBooster());

    //    }


    //    if (Input.GetKey(KeyCode.LeftShift)& Booster>=0)//누르면달리기상태
    //    {

    //        //Character.run = true;
            
    //        //SpineAni.SetBool("Booster", true);
            
    //        BoosterState = true;
    //        CurrentSpeed = DashSpeed;

    //    }
    //    else
    //    {
            
    //        //SpineAni.SetBool("Booster", false);
            
    //        BoosterState = false;
    //        CurrentSpeed = WalkSpeed;
    //    }
    //    if (Input.GetKeyUp(KeyCode.LeftShift))//때면 달리기상태해제
    //    {

            
    //        //SpineAni.SetBool("Booster", false);
            
    //        BoosterState = false;
    //        CurrentSpeed = WalkSpeed;

    //    }

    //}
    //public void FixedUpdate()
    //{
    //    TurnX += Input.GetAxis("Mouse X");
    //    TurnY += Input.GetAxis("Mouse Y");


    //    //방향   
    //    //민감도적용
    //    float X = -TurnY * Sensitivy;
    //    float Y = TurnX * Sensitivy;
    //    //카메라 최대각도적용
    //    X = Mathf.Clamp(X, -CameraMaxAngle, CameraMaxAngle);
        
    //    D = new Vector3(X, Y, 0f);

        
    //    C = new Vector3(0f, Y, 0f);
    //    MoveDir.transform.localRotation = Quaternion.Euler(C);
    //    transform.rotation = Quaternion.Euler(D);
      
        
    ////    Vector3 direction = new Vector3(horizontal, 0, vertical);
    ////    direction = Vector3.ClampMagnitude(direction, 1f);
    ////    direction = MoveDir.transform.TransformDirection(direction);
    ////    direction *= CurrentSpeed;
    ////    /*

    ////    movement = transform.TransformDirection(direction);
    ////    IsGround = Controller.isGrounded;
    ////    if (!IsGround)
    ////    {
    ////        movement.y -= gravity * Time.fixedDeltaTime;
    ////    }
    ////    //Controller.Move(movement * Time.fixedDeltaTime);
    ////    finalMovement = Vector3.Lerp(finalMovement, movement, Time.fixedDeltaTime * movementAcceleration);
    ////    Controller.Move(finalMovement * Time.fixedDeltaTime);


    ////    velocity = Controller.velocity;

    //}

    
    //public void LateUpdate()
    //{



        
    //    //민감도적용
    //    float X = -TurnY * Sensitivy;
    //    float Y = TurnX * Sensitivy;
    //    //카메라 최대각도적용
    //    X = Mathf.Clamp(X, -CameraMaxAngle, CameraMaxAngle);

    //    D = new Vector3(X, Y, 0f);

        
        
        
    //    X = Mathf.Clamp(X, -CameraMaxAngle, CameraMaxAngle);
    //    //상체회전
    //    //SpineTrans.LookAt(a.transform.position);
    //    //SpineTrans.rotation = SpineTrans.rotation * Quaternion.Euler(ChestOffset);
    //    //캐릭터움직이는방향회전
    //    C = new Vector3(0f, Y, 0f);
    //    //MoveDir.transform.rotation = Quaternion.Euler(C);
        


        
        
    //    //캐릭터 지상에서 걷기
    //    if (vertical > 0 | horizontal < 0 | horizontal > 0 & IsGround)
    //    {
    //        //LegAni.SetBool("WalkFront", true);
    //    }
    //    else
    //    {
    //        //LegAni.SetBool("WalkFront", false);
    //    }
    //    if (vertical < 0 & IsGround)
    //    {
    //        //LegAni.SetBool("WalkBack", true);
    //    }
    //    else
    //    {
    //        //LegAni.SetBool("WalkBack", false);
    //    }

    //    //캐릭터지상에서 발회전
    //    if (horizontal > 0 & IsGround)
    //    {
    //       LegAngle = MoveDir.transform.rotation.y + horizontal * 90 - (vertical * 45);
            
    //        //waist.transform.rotation = Quaternion.Euler(0, 180, -90);
    //        //waist.transform.rotation = Quaternion.Lerp(waist.transform.rotation, Quaternion.Euler(0, 210, -90), Time.deltaTime*2);
    //    }
    //    else if (horizontal < 0 & IsGround)
    //    {
    //        LegAngle = MoveDir.transform.rotation.y + horizontal * 90 + (vertical * 45);
    //        //waist.transform.rotation = Quaternion.Euler(0, 0, -90);
    //        //waist.transform.rotation = Quaternion.Lerp(waist.transform.rotation, Quaternion.Euler(0, 30, -90), Time.deltaTime*2);
    //    }
    //    else
    //    {
    //        //waist.transform.rotation = Quaternion.Euler(0, 120,-90);
    //        //waist.transform.rotation = Quaternion.Lerp(waist.transform.rotation, Quaternion.Euler(0, 60, -90), Time.deltaTime*2);
    //    }

    //    //test.transform.rotation = Quaternion.Euler(0, Y, 0);

    //    //캐릭터 움직이는방향
    //    Vector3 direction = new Vector3(horizontal, 0, vertical);
    //    direction = Vector3.ClampMagnitude(direction, 1f);
    //    direction = test.transform.TransformDirection(direction);
    //    direction *= CurrentSpeed;
    //    /*
    //if (JSpeed > 0)
    //    Controller.Move(direction * Time.fixedDeltaTime);
    //else
    //    Controller.SimpleMove(direction);
    //*/
        
    //    movement = transform.TransformDirection(direction);
    //    Debug.Log(movement);
    //    IsGround = Controller.isGrounded;
    //    if (!IsGround)
    //    {
    //        movement.y -= gravity * Time.fixedDeltaTime;
    //    }
    //    if(movement.y>0)
    //    {
    //        movement.y = 0;
    //    }
    //    //Controller.Move(movement * Time.fixedDeltaTime);
    //    finalMovement = Vector3.Lerp(finalMovement, movement, Time.fixedDeltaTime * movementAcceleration);
    //    Controller.Move(finalMovement * Time.fixedDeltaTime);


    //    velocity = Controller.velocity;


        


        
    //    /*
    //    if (Controller.isGrounded)
    //    {



    //        var translation = new Vector3(horizontal, 0, vertical);
    //        speed = CurrentSpeed;
    //        movement = transform.TransformDirection(translation * speed);
    //    }
    //    else 
    //    {
    //        movement.y -= gravity * Time.deltaTime;
    //    }
    //    finalMovement = Vector3.Lerp(finalMovement, movement, Time.deltaTime * movementAcceleration);
    //    Controller.Move(finalMovement * Time.deltaTime);
    //    */
    //}





    //IEnumerator StopBooster()
    //{
    //    Falling = true;

    //    yield return new WaitForSeconds(5.0f);

    //    Falling = false;

    //}
    //finalMovement = Vector3.Lerp(finalMovement, movement, Time.fixedDeltaTime * movementAcceleration);
}
//if (JSpeed > 0)
//    Controller.Move(direction * Time.fixedDeltaTime);
//else
//    Controller.SimpleMove(direction);
//*/
