using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Manager : MonoBehaviour
{
    //카메라 관절
    [Header("Camera")]
    public Camera cam;
    [SerializeField]
    public Gun_Manager Shoot_Manager;//총 매니저
    [SerializeField]
    public Grenade_Shooter Grenade_Manager;
    [Header("Move")]
    public CharacterController controller;
    public Transform MoveDir;
    public Rigidbody Rd;
    [Header("Animation")]
    public Animator CharAni;
    public Animator LegAni;
    //상체
    Transform Spine;
    public Transform GunPosi;
    public Transform target;//상체바라볼오브젝트의위치
    public Vector3 relativeVec1; //상체회전벡터
    public Vector3 relativeVec2;//총기회전벡터
    public Vector3 relativeVec3;//총기이동벡터





    [Header("상하좌우이동 회전입력")]
    public float TurnX;//마우스X
    public float TurnY;//마우스Y
    public float X;//민감도적용
    public float Y;//민감도적용
    Vector3 D;//캐릭터의회전할벡터
    //Vector3 direction;// 캐릭터방향
    public float Sensitivy = 10.0f;//마우스민감도
    float CameraMaxAngle;//y축 앵글각도
    public float horizontal;//좌우입력
    public float vertical;//전후입력



    [Header("속도")]
    public float JumpHeight;//
    public float CurrentSpeed;//캐릭터의현재속도
    public float WalkSpeed = 4.0f;//캐릭터의걷기스피드
    public float DashSpeed = 8.0f;//캐릭터의대쉬속도
    public float LandingSpeed = 0.3f;//착지시 캐릭터속도
    public float HeightPoint;
    public float MinBoost;
    public float Hold;
    public float Booster = 300, BoosterMAX = 300;//스태미나와스태미나최대치

    [Header("상태")]
    public bool IsGround = true;//캐릭터 지면감지
    public bool BoosterState = false;//캐릭터 대쉬감지
    public bool Action;


    public bool Landing;
    public bool Landing_Action;
    public bool JumpActive;
    //총상태에니메이션
    bool BoosterAni;
    bool FrontWalk;
    bool BackWalk;
    bool LeftWalk;
    bool RightWalk;

    [Header("총기반동")]
    Vector3 recoilkickback;
    public Transform recoilCam;
    public GameObject GroundCheker;

    [Header("이펙트")]
    public ParticleSystem LSmoke, RSmoke; //대쉬연기이펙트

    bool smoke;//대쉬시 연기이펙트 온오프




    Vector3 velocity;



    [Header("효과음")]

    public AudioSource Boostering;



    public Collider ActionObjcet;
    public Collider ActionBox;

    //public BoomBox Boombox;//접촉한텔레포터작동기
    //커서록온
    bool Aim;


    public void OnEnable()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.Char_Player_Trace = gameObject;//캐릭터위치추적위치
            GameManager.instance.Char_Player_Attack = transform.GetChild(3).gameObject;//캐릭터에게 공격할위치
            GameManager.instance.Cam_Player_Position = cam.transform;
        }


    }

    void Awake()
    {
        Shoot_Manager = GetComponent<Gun_Manager>();
        Spine = CharAni.GetBoneTransform(HumanBodyBones.Spine);
        TurnX = 0;
        TurnY = 0;
        CurrentSpeed = WalkSpeed;
        JumpHeight = 2.0f;
        BoosterAni = FrontWalk = BackWalk = LeftWalk = RightWalk = false;
        CameraMaxAngle = 80.0f;
        relativeVec1 = new Vector3(0, -40, -100);
        relativeVec2 = new Vector3(0, 90, -45);
        smoke = false;
        JumpActive = false;

        Action = false;
        Landing = false;
        Landing_Action = false;
        HeightPoint = 0;
        MinBoost = 0;
        Hold = 0.0f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        Shoot_Manager.Init_GUN();
    }
    private void Start()
    {
        if (GameManager.instance.Char_Player_Trace == null)
        {
            GameManager.instance.Char_Player_Trace = gameObject;//캐릭터위치추적위치

        }
        if (GameManager.instance.Char_Player_Attack == null)
        {
            GameManager.instance.Char_Player_Attack = transform.GetChild(2).gameObject;//캐릭터에게 공격할위치
        }
    }
    void Update()
    {




        if (Action == false)
        {

            #region//입력
            ActiveAction();//Active액션입력
            ActiveRd();
            InputArrow();//방향키입력
            CharJump();//점프입력
            Input_Grenade();//수류탄투척
            BoosterActive();//부스터입력
            #endregion
            #region//캐릭터회전
            CharAngleSetting();//캐릭터화면회전   //Action중에는 다른화면바라봐야하므로 액션상태에선비적용
            CharLegAni();//방향키입력과 부스터입력에 다리애니메이션정하기
            #endregion
        }

        ChangeSpeed();//입력에따라 속도 변환
        IsGrounded();//지면감지
        AnimationActive();//다리움직임
        RecoilBack();//총기반동시 화면돌아옴
        ControlSmoke();//부스터상태면발바닥에서 연기발생 //액션상태와상관없음


        #region//총딜레이감소

        for (int i = 0; i < Shoot_Manager.GunList.Count; i++)
        {
            if (Shoot_Manager.GunList[i].Delay > 0)
            {
                Shoot_Manager.GunList[i].Delay -= Time.deltaTime;
            }

        }
        #endregion
        Shoot_Manager.CanAction();//하단의행동이가능한 상태인지 체크
        Shoot_Manager.AimChange();//조준상태변환
        if (Shoot_Manager.GunAction == false)
        {
            #region//수류탄
            Grenade_Manager.Regen_Granade();//수류탄재사용대기시간
            Grenade_Manager.ThrowLine();//궤적그려주기
            #endregion
            Shoot_Manager.Equip_Gun.Shoot();//사격 총알이없거나r누르면 장전
            Shoot_Manager.GunSWap();//무기교체
            #region//장전
            if (Input.GetKeyDown(KeyCode.R))
            {
                if (Shoot_Manager.Line.enabled == true)
                {
                    Shoot_Manager.Line.enabled = false;
                }
                Shoot_Manager.ARshot = false;
                Shoot_Manager.SRshot = false;
                Shoot_Manager.Shooting = false;

                Shoot_Manager.Equip_Gun.Reload_Function();
            }
            #endregion
        }
        Shoot_Manager.AimMode();//조준상태에 따라 카메라 거리변경
        Shoot_Manager.Recoilimage();//사격반동으로 인한 조준선 이동
        Shoot_Manager.Collideroff();//특정행동중 총기의 콜라이더를 오프하여 총기가 벽에충돌방지

        



    }
    void FixedUpdate()
    {
        if (Action == false)
        {
            CharMoveDir();//방향으로이동 //Action중에는 이동하면 안되므로 액션에서 비적용
        }
        Gravity_Jump();//중력과 점프기능
        Shoot_Manager.AnimationsActive();//애니메이션작동
    }
    void LateUpdate()
    {
        if (Action == false)
        {
            CharAniMove();//캐릭터 상하체조준움직임 상체회전까지
            #region//이동후 벨로시티 초기화
            Rd.velocity = Vector3.zero;
            Rd.angularVelocity = Vector3.zero;
            #endregion








            Shoot_Manager.Swaping = CharAni.GetCurrentAnimatorStateInfo(1).IsTag("Swap");
            Shoot_Manager.Reloading= CharAni.GetCurrentAnimatorStateInfo(1).IsTag("Reload");

        }


    }

    void Input_Grenade()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Grenade_Manager.Throw_Grande();
        }
    }

    void InputArrow()
    {

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        TurnX += Input.GetAxis("Mouse X");
        TurnY += Input.GetAxis("Mouse Y");

    }
    void ActiveRd()
    {
        if (Action == true)
        {
            GetComponent<Rigidbody>().isKinematic = true;
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
        }
    }
    void IsGrounded()
    {

        IsGround = controller.isGrounded;


        if (IsGround == false)
        {
            HeightPoint += Time.deltaTime;

        }
        else if (HeightPoint >= 1.5f & IsGround == true)
        {

            if (Landing_Action == true)
            {

                StartCoroutine(LandingAction());
            }


        }
        else
        {

            Landing_Action = true;
            HeightPoint = 0.0f;

        }



    }
    IEnumerator LandingAction()
    {

        Landing_Action = false;
        Landing = true;
        CharAni.SetTrigger("Landing");
        LegAni.SetTrigger("Landing");


        yield return new WaitForSeconds(0.5f);
        Landing = false;
        HeightPoint = 0.0f;

    }

    void ActiveAction()
    {
        if (Input.GetKeyDown(KeyCode.E) & IsGround)
        {
            if (ActionBox != null)
            {

                StartCoroutine(Picking());

            }
            else if (ActionObjcet != null)
            {

                StartCoroutine(ActionDoit());

            }

        }
    }
    void CharAniMove()
    {
        Spine = CharAni.GetBoneTransform(HumanBodyBones.Hips);
        Spine.LookAt(target.position);
        Spine.rotation = Spine.rotation * Quaternion.Euler(relativeVec3);



    }
    void CharLegAni()
    {
        if (BoosterState == true)
        {
            BoosterAni = true;

        }
        else if (BoosterState == false)
        {
            BoosterAni = false;

        }
        if (vertical < 0)
        {
            BackWalk = true;
        }
        else
        {
            BackWalk = false;
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




        if (!IsGround)
        {
            FrontWalk = false;
            LeftWalk = false;
            RightWalk = false;
            BackWalk = false;
            BoosterState = false;

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
        D = new Vector3(0f, Y, 0f);

        if (Action == false)
            transform.rotation = Quaternion.Euler(D);


    }

    void CharMoveDir()
    {
        Vector3 motion = transform.right * horizontal + transform.forward * vertical;
        controller.Move(motion * CurrentSpeed * Time.deltaTime);
    }
    void CharJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) & IsGround & Landing == false)
        {
            if (BoosterState == false)
                JumpActive = true;
        }
    }
    void Gravity_Jump()
    {
        if (IsGround && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        if (JumpActive & Action == false)
        {
            JumpActive = false;

            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);


            CharAni.SetTrigger("Jump");
            LegAni.SetTrigger("Jump");

            Booster -= 30.0f;

            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Physics.gravity.y);

        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
    void BoosterActive()
    {
        if (Input.GetKey(KeyCode.LeftShift) & Booster > 0)
        {
            if (BoosterState == false & IsGround)
            {

            }

            BoosterState = true;



        }
        else if (MinBoost >= 1.0f & Booster > 0)
        {
            BoosterState = false;
            MinBoost = 0.0f;

        }
        else if (Booster <= 0 & Hold <= 0)
        {
            BoosterState = false;
            MinBoost = 0.0f;
            Hold = 5.0f;

        }

        if (Hold > 0)
        {
            Hold -= Time.deltaTime;
        }
        if (Boostering.isPlaying & BoosterState == false)
            Boostering.Stop();
    }
    void ChangeSpeed()
    {
        if (Landing == true)
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, LandingSpeed, Time.deltaTime * 3);
        }
        else if (BoosterState == true & Landing == false)
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, DashSpeed, Time.deltaTime);
            Booster -= Time.deltaTime * 10;
        }
        else
        {
            CurrentSpeed = Mathf.Lerp(CurrentSpeed, WalkSpeed, Time.deltaTime);
            if (IsGround) //회복조건
            {
                if (BoosterState == false & Hold <= 0)
                {
                    Booster += Time.deltaTime * 60;
                }
                if (BoosterMAX < Booster)
                {
                    Booster = BoosterMAX;
                }
            }
        }
        if (BoosterState == true)
        {
            MinBoost += Time.deltaTime;
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

        LegAni.SetBool("FrontWalk", FrontWalk);
        LegAni.SetBool("BackWalk", BackWalk);
        LegAni.SetBool("LeftWalk", LeftWalk);
        LegAni.SetBool("RightWalk", RightWalk);
        LegAni.SetBool("Booster", BoosterAni);
        CharAni.SetBool("Booster", BoosterAni);
        LegAni.SetBool("Grounded", IsGround);
        CharAni.SetBool("Grounded", IsGround);



    }



    public void GunRecoilActive()
    {
        if (Aim == true)
        {
            recoilkickback = new Vector3(0.4f, 0.15f, .0f);
        }
        else if (Aim == false)
        {
            recoilkickback = new Vector3(0.05f, 0.2f, .0f);
        }

        Vector3 recoilVector = new Vector3(Random.Range(-recoilkickback.x, recoilkickback.x), recoilkickback.y, recoilkickback.z);
        Vector3 recoilCamVector = new Vector3(-recoilVector.y * 400.0f, recoilVector.x * 200.0f, 0);


        recoilCam.localRotation = Quaternion.Slerp(recoilCam.localRotation, Quaternion.Euler(recoilCam.localEulerAngles + recoilCamVector), Time.deltaTime);

    }
    public void RecoilBack()
    {

        recoilCam.localRotation = Quaternion.Slerp(recoilCam.localRotation, Quaternion.identity, Time.deltaTime);
    }

    IEnumerator Picking()
    {

        if (ActionBox.GetComponent<AmmoBox>() != null)
        {
            AmmoBoxOpenBefore();
        }
        else if (ActionBox.GetComponent<BoomBox>() != null)
        {
            if (ActionBox.GetComponent<BoomBox>().ck)
                BoomBoxOpenBefore();
        }

        yield return new WaitForSeconds(1);



        if (ActionBox.GetComponent<AmmoBox>() != null)
        {
            AmmoBoxOpenAfter(ActionBox.GetComponent<AmmoBox>());
        }

        else if (ActionBox.GetComponent<BoomBox>() != null)
        {
            if (ActionBox.GetComponent<BoomBox>().ck)
                BoomBoxOpenAfter(ActionBox.GetComponent<BoomBox>());
        }


    }
    void BoomBoxOpenBefore()
    {


        ActionReady();

        BoomBox temp = ActionBox.GetComponent<BoomBox>();
        temp.ani.SetTrigger("Open");

        transform.position = temp.position;
        transform.LookAt(temp.transform.position);
        TurnX = temp.Xrot;
        CharAni.SetTrigger("PickUp");
        LegAni.SetTrigger("PickUp");

    }
    void BoomBoxOpenAfter(BoomBox temp)
    {
        Shoot_Manager.BoomGet();
        Grenade_Manager.GrenadeCount += 3;


        if (Grenade_Manager.GrenadeCount > Grenade_Manager.GrenadeMAXCount)
        {
            Grenade_Manager.GrenadeCount = Grenade_Manager.GrenadeMAXCount;

        }
        temp.ck = false;

        ActionDone();
    }


    void AmmoBoxOpenBefore()
    {
        ActionReady();
        CharAni.SetTrigger("PickUp");
        LegAni.SetTrigger("PickUp");
        AmmoBox temp = ActionBox.GetComponent<AmmoBox>();
        temp.ani.SetTrigger("Open");
        temp.PlayerOpenSound();
        transform.position = temp.position;
        transform.LookAt(temp.transform.position);
        TurnX = temp.Xrot;

    }
    void AmmoBoxOpenAfter(AmmoBox temp)
    {



        Base_Gun tempGun = Shoot_Manager.GunList[0];
        tempGun.CurrentPack += temp.firstAmmo;
        if (tempGun.CurrentPack > tempGun.MAXPack)
        {
            tempGun.CurrentPack = tempGun.MAXPack;
        }


        tempGun = Shoot_Manager.GunList[1];
        tempGun.CurrentPack += temp.SecondAmmo;
        if (tempGun.CurrentPack > tempGun.MAXPack)
        {
            tempGun.CurrentPack = tempGun.MAXPack;
        }





        temp.SelfDestroy(1.0f);
        ActionBox = null;
        ActionDone();
    }
    IEnumerator ActionDoit()
    {
        ActionReady();
        float timer = 0;
        if (ActionObjcet.GetComponent<OpenDoor>() != null)
        {
            OpenDoorBefore();
            timer = 4.0f;
        }
        if (ActionObjcet.GetComponent<OpenPortal>() != null)
        {
            OpenPortalBefore();
            timer = 4.0f;
        }

        yield return new WaitForSeconds(timer);

        if (ActionObjcet.GetComponent<OpenDoor>() != null)
        {
            OpenDoorAfter();

        }
        if (ActionObjcet != null)
        {
            if (ActionObjcet.GetComponent<OpenPortal>() != null)
            {
                OpenPortalAfter();

            }
        }
    }
    void OpenDoorBefore()
    {
        OpenDoor temp = ActionObjcet.GetComponent<OpenDoor>();
        transform.position = temp.position;
        transform.rotation = Quaternion.Euler(temp.rotate);
        CharAni.SetTrigger("Action");
        LegAni.SetTrigger("Action");
    }
    void OpenDoorAfter()
    {
        OpenDoor temp = ActionObjcet.GetComponent<OpenDoor>();
        if (temp.State)
        {
            temp.CantOpenDoor();
            ActionDone();
        }
        else
        {
            temp.OpenAction();
            ActionObjcet = null;
            ActionDone();
        }
    }
    void OpenPortalBefore()
    {
        OpenPortal temp = ActionObjcet.GetComponent<OpenPortal>();
        transform.position = temp.position;
        transform.rotation = Quaternion.Euler(temp.rotate);
        CharAni.SetTrigger("Action");
        LegAni.SetTrigger("Action");
    }
    void OpenPortalAfter()
    {
        OpenPortal temp = ActionObjcet.GetComponent<OpenPortal>();
        if (temp.State)
        {
            temp.OpenPortalFunction();

            ActionObjcet = null;
            ActionDone();
        }
    }


    public void PauseFunction(bool ck)
    {
        if (ck == true)
        {
            Action = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            FrontWalk = false;
            LeftWalk = false;
            RightWalk = false;
            BackWalk = false;
            BoosterState = false;

        }
        else
        {
            Debug.Log("끄기작동");
            Action = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }


    void ActionReady()
    {
        Action = true;
        FrontWalk = false;
        LeftWalk = false;
        RightWalk = false;
        BackWalk = false;
        BoosterAni = false;
        BoosterState = false;

        AnimationActive();
        Shoot_Manager.Equip_Gun_Model.SetActive(false);

    }
    void ActionDone()
    {

        Shoot_Manager.Equip_Gun_Model.SetActive(true);
        Action = false;
    }


    public void ResetAllAnimation()
    {
        FrontWalk = false;
        BackWalk = false;
        LeftWalk = false;
        RightWalk = false;
        BoosterAni = false;
        IsGround = false;
        LegAni.SetBool("FrontWalk", FrontWalk);
        LegAni.SetBool("BackWalk", BackWalk);
        LegAni.SetBool("LeftWalk", LeftWalk);
        LegAni.SetBool("RightWalk", RightWalk);
        LegAni.SetBool("Booster", BoosterAni);
        CharAni.SetBool("Booster", BoosterAni);
        LegAni.SetBool("Grounded", IsGround);
        CharAni.SetBool("Grounded", IsGround);
    }


    public void OnTriggerStay(Collider other)
    {
        LayerMask item = 11;
        LayerMask ActionArea = 12;
        if (other.gameObject.layer == item)
        {
            ActionBox = other;
        }
        else if (other.gameObject.layer == ActionArea)
        {
            ActionObjcet = other;

        }
    }
    public void OnTriggerExit(Collider other)
    {
        LayerMask item = 11;
        LayerMask ActionArea = 12;
        if (other.gameObject.layer == item)
        {
            ActionBox = null;
        }
        else if (other.gameObject.layer == ActionArea)
        {
            ActionObjcet = null;

        }
    }


}

