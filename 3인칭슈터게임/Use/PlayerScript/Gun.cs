using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using System;
public class Gun : MonoBehaviour
{
    [Header("Player")]
    public float angle;
    public bool BoomHave;
    public bool Action;

    [Header("GunInfo")]
    //들고있는총의 정보
    public float Delay;//총의현재딜레이
    public float Reload; //최대장전
    public float ReloadTime;//장전시간
    public float Pack;//잔탕
    public float MAXPack;//최대잔탕
    public bool Reloading, Swaping, Shooting;//장전애니메이션/스왚/사격
    public int GN = 1;//들고있는 총의 번호
    public bool HandGun, Grenade, PIN;
    public float GunDistance1, GunDistance2, GunDistance3;
    public float BoomGunDelay;
    public float BoomGunMAXDelay;
    public int GrenadeCount;
    public int GrenadeMAXCount;

    //들고있는 총의상태
    [Header("EqupGunInfo")]
    public float CurrentDelay;//총의딜레이
    public float CurrentAmmo;
    public float CurrentReload;
    public float CurrentPack;

    [Header("수류탄")]
    public GameObject GrenadePooling;
    public int GrenadeIndex = 0;
    public Transform LeftHand;
    public float ThrowPower = 30.0f;
    public float ThrowDelay = 2.0f;
    public float ThrowMAXDelay = 2.0f;
    public float MAXThrowAngle = 45.5f;
    public float MINThrowAngle = 43.5f;


    public int GunDamage1 = 10;
    public int GunDamage2 = 100;

    //총
    [Header("GunIEffect")]
    public BoomGun BoomGunObject;
    public GameObject PICKGUN0, PICKGUN1, PICKGUN2;//현재들고있는 총
    public GameObject IDLEGUN0, IDLEGUN1;//등에 매고있는 총
    public GameObject L1, L2, L3; //  격발위치
    public ParticleSystem L1E;
    public VisualEffect L2E;
    public GameObject Pooling;
    public GameObject L2HitEffect;
    int EffectCount1 = 0;
    int EffectCount2 = 0;
    public ParticleSystem SE1;
    public Transform SE3Postion;
    public GameObject hit1_effect;//첫번째총
    public GameObject hit2_effect;//두번째총
    public LineRenderer Line;
    public Transform GrenadePosi;
    public float step = 0.01f;
    //에임좌표
    [Header("CrossHair")]
    public Image Up;
    public Image Down;
    public Image Right;
    public Image Left;
    public GameUIManager GameUI;
    public float Aimfloat;
    public bool Recoil;
    public bool Aim;
    public Vector3 LaserVector = Vector3.zero;
    [Header("GunSound")]
    public AudioSource[] FirstGunSound;
    int Fcount;
    public AudioSource[] FLSound;
    public AudioSource[] FHSound;
    int SoundCount;
    public AudioSource SecondGunSound;
    //애니메이션 체크상태용 
    [Header("Animation")]
    public Collider[] GunModel;
    public Animator SpineAction;
    public Camera cam;
    public Camera subcam;
    public Player_Manager Move2;
    public bool ARRealod;
    public bool SRRealod;

    public  bool ARshot;
    public bool SRshot;

    private void Awake()
    {
        L2E.Stop();
    }
    void Start()
    {
        GrenadeCount = 0;
        GrenadeMAXCount = 5;
        PIN = false;
        EffectCount1 = 0;
        EffectCount2 = 0;
        ARRealod = SRRealod = ARshot = SRshot = false;
        //격발이펙트
        L1E = L1.transform.GetChild(0).GetComponent<ParticleSystem>();
        SE1 = L2.transform.GetChild(0).GetComponent<ParticleSystem>();
        SE3Postion = L2.transform.GetChild(1).GetComponent<Transform>();
        BoomGunObject = GetComponent<BoomGun>();
        BoomGunMAXDelay = BoomGunDelay = 60.0f;
        //총세팅
        //Delay = GameManager.instance.Delay1;
        Swaping = false;
        Shooting = false;
        Reloading = false;
        //Reload = GameManager.instance.Reload1;
        ReloadTime = 2.2f;
        //Pack = GameManager.instance.Pack1;
        //MAXPack = GameManager.instance.MAXPack1;
        //CurrentAmmo = GameManager.instance.Ammo1;
        CurrentDelay = Delay;
        CurrentReload = Reload;
        CurrentPack = Pack;
        GunDistance1 = 200;
        GunDistance2 = 200;
        GN = 1;
        HandGun = false;
        Recoil = false;
        Move2 = GetComponent<Player_Manager>();
        Aim = false;
        Aimfloat = 60.0f;
        SoundCount = 0;
        Fcount = 0;
        Pooling = Move2.MoveDir.GetChild(0).gameObject;
        L2.SetActive(false);

    }
    private void Update()
    {
        //Debug.DrawRay(cam.transform.position, cam.transform.forward * 200, Color.blue);
        if (Pack > MAXPack)
        {
            Pack = MAXPack;
        }
        if (CurrentDelay > 0)
        {
            CurrentDelay -= Time.deltaTime;
        }
        if (BoomGunDelay < BoomGunMAXDelay)
        {
            BoomGunDelay += Time.deltaTime;
        }
        CanAction();//하단의행동이가능한 상태인지 체크
        AimChange();
        if (Action == false)
        {
            ThrowGrenade();
            ThrowLine();
            GunShot();//사격 총알이없거나r누르면 장전
            GunSWap();//무기교체
        }
        AimMode();
        Recoilimage();
        if (Swaping || Move2.Action == false)
        {
            for (int i = 0; i < GunModel.Length; i++)
            {
                GunModel[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < GunModel.Length; i++)
            {
                GunModel[i].enabled = true;
            }
        }
    }

    void FixedUpdate()
    {
        AnimationsActive();
    }
    void CanAction()
    {
        if (Reloading == false & Swaping == false & Move2.Action == false)//캐릭터가 행동가능조건추가해야함
        {
            Action = false;

        }
        else
        {
            Action = true;
        }
    }
    void AimChange()
    {
        if (Input.GetMouseButtonDown(1))
        {

            if (Aim == false)
            {
                Aimfloat = 30.0f;
                Aim = true;
            }
            else
            {
                Aimfloat = 60.0f;
                Aim = false;
            }
        }
    }
    void AimMode()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Aimfloat, Time.deltaTime * 5.0f);
        subcam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Aimfloat, Time.deltaTime * 5.0f);
    }

    void ThrowGrenade()
    {

        if (ThrowDelay <= ThrowMAXDelay)
        {
            ThrowDelay += Time.deltaTime;
        }
        if (Shooting == true && Action == false)
        {
            PIN = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4) & GrenadeCount >= 1 & ThrowDelay >= ThrowMAXDelay)
        {
            if (PIN == false)
            {
                PIN = true;
            }
            else if (PIN == true)
            {
                Debug.Log(1);
                ThrowDelay = 0.0f;
                GrenadeCount -= 1;
                if (HandGun == true)
                {
                    SpineAction.SetTrigger("HG_Grenade");
                }
                else
                {
                    SpineAction.SetTrigger("AR_Grenade");
                }
            }
        }
    }
    public void GrendaeThrowFunction()
    {
        //GrenadePooling.transform.GetChild(GrenadeIndex).transform.position = LeftHand.position;
        //GrenadePooling.transform.GetChild(GrenadeIndex).transform.rotation = LeftHand.rotation;
        //GrenadePooling.transform.GetChild(GrenadeIndex).GetComponent<GrenadeBoom>().Throw(GrenadePosi.forward, ThrowPower, angle);
        //GrenadeIndex += 1;
        //if (GrenadeIndex >= 5)
        //{
        //    GrenadeIndex = 0;
        //}
    }
    public void ThrowLine()
    {
        if (PIN == true & Action == false & Swaping == false & Reloading == false)
        {
            if (Line.enabled == false)
            {
                Line.enabled = true;
            }
            Vector3 direction = GrenadePosi.forward;
            Vector3 Grounddirection = new Vector3(direction.x, 0, direction.z);
            float t = (0.5f - cam.transform.GetComponent<GameCamera>().currXRot / 60);

            angle = Mathf.Lerp(MAXThrowAngle, MINThrowAngle, t);

            step = Mathf.Max(0.01f, step);
            DrewLine2(Grounddirection.normalized, ThrowPower, angle, step);


        }
        else
        {
            Line.enabled = false;
        }
    }
    public void DrewLine2(Vector3 direction, float Power, float angle, float step)
    {
        float time = 6.0f;
        Line.positionCount = (int)(time / step) + 2;
        int count = 0;
        for (float i = 0; i < time; i += step)
        {
            float x = Power * i * Mathf.Cos(angle);
            float y = Power * i * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(i, 2);
            Line.SetPosition(count, GrenadePosi.position + direction * x + Vector3.up * y);
            count++;
        }
        float Finalx = Power * time * Mathf.Cos(angle);
        float Finaly = Power * time * Mathf.Sin(angle) - 0.5f * -Physics.gravity.y * Mathf.Pow(time, 2);
        Line.SetPosition(count, GrenadePosi.position + direction * Finalx + Vector3.up * Finaly);
    }
    void GunShot()
    {
        if (Input.GetKeyDown(KeyCode.R) && (GN == 1 || GN == 2))
        {
            if (CurrentPack >= 1 & CurrentReload != CurrentAmmo)
            {
                GunReload();
            }
        }
        else
        {
            if (GN == 1)
            {
                if (((Input.GetMouseButton(0) && CurrentAmmo == 0) && CurrentPack >= 1))
                {
                    GunReload();
                }

                else if (Input.GetMouseButton(0) && CurrentDelay <= 0 & CurrentAmmo >= 1)
                {
                    PIN = false;
                    CurrentDelay = Delay;

                    CurrentAmmo -= 1;
                    Shooting = true;

                    HitGUN1();
                    L1E.Play();
                    FirstGunSound[Fcount].Play();
                    Fcount++;
                    if (Fcount == FirstGunSound.Length)
                    {
                        Fcount = 0;
                    }
                    ARshot = true;
                }
                else
                {
                    ARshot = false;
                    Shooting = false;
                }
                if (ARshot == true)
                {
                    Recoil = true;
                }
                else
                {
                    Recoil = false;
                }
            }
            else if (GN == 2)
            {
                if (Shooting == false)
                {
                    if (((Input.GetMouseButton(0) && CurrentAmmo == 0) && CurrentPack >= 1))
                    {

                        GunReload();
                    }
                    else if (Input.GetMouseButton(0) && CurrentDelay < 0 & CurrentAmmo >= 1)
                    {
                        PIN = false;
                        Shooting = true;
                        CurrentDelay = Delay;
                        CurrentAmmo -= 1;
                        HitGUN2();
                    }
                }
            }
            else if (GN == 3)
            {
                if (Shooting == false)
                {
                    if (Input.GetMouseButtonDown(0) && BoomGunDelay >= BoomGunMAXDelay & CurrentAmmo >= 1)
                    {
                        PIN = false;
                        SpineAction.SetTrigger("HandGunShot");
                        //BoomGunObject.Shoot();
                        BoomGunDelay = 0.0f;
                    }
                }
            }

        }
    }

    public void GunSWap()
    {
        if (Shooting == false && Reloading == false)
        {

            int SwapGunNumber;
            if (Input.GetKey(KeyCode.Alpha1))
            {
                SwapGunNumber = 1;
                if (GN != SwapGunNumber)
                {
                    StartCoroutine(GunSwapFunction(SwapGunNumber, GN));//총바꾸기
                }
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                SwapGunNumber = 2;
                if (GN != SwapGunNumber)
                {
                    StartCoroutine(GunSwapFunction(SwapGunNumber, GN));//총바꾸기
                }
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                SwapGunNumber = 3;
                if (GN != SwapGunNumber && BoomHave == true)
                {
                    StartCoroutine(GunSwapFunction(SwapGunNumber, GN));//총바꾸기
                }
            }

            else
            {
                SwapGunNumber = 0;
            }
        }

    }
    IEnumerator GunSwapFunction(int GunNumber, int CurrentGN)
    {
        Swaping = true;
        PIN = false;
        if (Line.enabled == true)
        {
            Line.enabled = false;
        }
        SpineAction.SetTrigger("ChangeTrigger");

        yield return new WaitForSeconds(1.25f);

        switch (GN)//집어넣는총
        {
            case 1:
                //GameManager.instance.Pack1 = CurrentPack;
                //GameManager.instance.Ammo1 = CurrentAmmo;
                PICKGUN0.SetActive(false);
                L1.SetActive(false);
                break;
            case 2:
                //GameManager.instance.Pack2 = CurrentPack;
                //GameManager.instance.Ammo2 = CurrentAmmo;
                PICKGUN1.SetActive(false);
                L2.SetActive(false);
                break;
            case 3:
                //GameManager.instance.Pack3 = CurrentPack;
                //GameManager.instance.Ammo3 = CurrentAmmo;
                PICKGUN2.SetActive(false);
                HandGun = false;
                break;

            default:
                break;
        }

        switch (GunNumber)//꺼내는총
        {
            case 1:
                //Delay = GameManager.instance.Delay1;
                //Reload = GameManager.instance.Reload1;
                //ReloadTime = 2.2f;
                //CurrentDelay = GameManager.instance.Delay1;
                //CurrentPack = GameManager.instance.Pack1;
                //CurrentAmmo = GameManager.instance.Ammo1;
                //CurrentReload = GameManager.instance.Reload1;
                //MAXPack = GameManager.instance.MAXPack1;
                GN = 1;
                PICKGUN0.SetActive(true);
                L1.SetActive(true);

                break;
            case 2:  //0번무기에서 1번무기로
                //Delay = GameManager.instance.Delay2;
                //Reload = GameManager.instance.Reload2;
                //ReloadTime = 2.2f;
                //CurrentDelay = GameManager.instance.Delay1;
                //CurrentPack = GameManager.instance.Pack2;
                //CurrentAmmo = GameManager.instance.Ammo2;
                //CurrentReload = GameManager.instance.Reload2;
                //MAXPack = GameManager.instance.MAXPack2;
                GN = 2;

                PICKGUN1.SetActive(true);

                L2.SetActive(true);
                L2E.Stop();
                break;

            case 3:
                //Delay = GameManager.instance.Delay3;
                //Reload = GameManager.instance.Reload3;
                //ReloadTime = 2.2f;
                //CurrentDelay = GameManager.instance.Delay3;
                //CurrentPack = GameManager.instance.Pack3;
                //CurrentAmmo = GameManager.instance.Ammo3;
                //CurrentReload = GameManager.instance.Reload3;
                //MAXPack = GameManager.instance.MAXPack3;
                GN = 3;

                PICKGUN2.SetActive(true);
                HandGun = true;
                break;

        }
        GameUI.GunSwapImage(CurrentGN, GN);
        if (HandGun)
        {
            SpineAction.SetTrigger("HandGun");
        }


        yield return new WaitForSeconds(0.05f);


        Swaping = false;
        SpineAction.SetBool("Change", Swaping);
    }



    public void GunReload()
    {
        if (Line.enabled == true)
        {
            Line.enabled = false;
        }
        ARshot = false;
        SRshot = false;
        Shooting = false;
        StartCoroutine(ReloadFunction());
    }

    IEnumerator ReloadFunction()
    {

        PIN = false;
        Reloading = true;
        if (GN == 1)
        {
            ARRealod = true;
        }
        else if (GN == 2)
        {
            SRRealod = true;
        }
        yield return new WaitForSeconds(ReloadTime + 0.3f);
        if (Reload > CurrentPack)
        {
            CurrentAmmo = CurrentPack;
            CurrentPack = 0;
        }
        else
        {
            float temp = CurrentAmmo;
            CurrentAmmo = CurrentReload;
            CurrentPack -= CurrentReload;
            CurrentPack += temp;
        }
        if (GN == 1)
        {
            ARRealod = false;
        }
        else if (GN == 2)
        {
            SRRealod = false;
        }

        Reloading = false;
    }


    void HitGUN1()
    {
        if (SoundCount >= FLSound.Length)
        {
            SoundCount = 0;
        }

        RaycastHit hitInfo;
        LayerMask layer = (1 << LayerMask.NameToLayer("Enemy") | 1 << LayerMask.NameToLayer("Ground"));

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, GunDistance1, layer))
        {
            Vector3 dir = hitInfo.point - L1.transform.position;
            if (Physics.Raycast(L1.transform.position, dir, out hitInfo, GunDistance1, layer))
            {
                if (hitInfo.collider.gameObject.layer == 8)
                {

                    EffectCount1 = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.GetChild(0).gameObject, EffectCount1, 1);
                    FLSound[SoundCount].transform.position = hitInfo.transform.position;
                    FLSound[SoundCount].Play();
                    SoundCount++;

                }
                HitHeadShot1(hitInfo);

            }

        }
        Move2.GunRecoilActive();
        L1E.Stop();
    }

    void HitHeadShot1(RaycastHit hitInfo)
    {
        if (hitInfo.transform.CompareTag("Head") & hitInfo.collider.gameObject.layer == 10)
        {
            StartCoroutine(HitCross(0.3f));



            


            Base_HP temp=hitInfo.transform.GetComponent<Base_HP>();

            if(temp.Armor)
            {
                
                FHSound[SoundCount].transform.position = hitInfo.transform.position;
                FHSound[SoundCount].Play();
                SoundCount++;
            }
            else
            {
                FLSound[SoundCount].transform.position = hitInfo.transform.position;
                FLSound[SoundCount].Play();
                SoundCount++;
            }
            temp.Damged(GunDamage1*5);



            EffectCount1 = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.GetChild(0).gameObject, EffectCount1, 1);

        }
        else
        {
            HitShot1(hitInfo);
        }
    }
    void HitShot1(RaycastHit hitInfo)
    {

        if (hitInfo.transform.CompareTag("hitbox") & hitInfo.collider.gameObject.layer == 10)
        {
            StartCoroutine(HitCross(0.3f));





            Base_HP temp =hitInfo.transform.GetComponent<Base_HP>();

            temp.Damged(GunDamage1);
            
            if(temp.Armor)
            {
                FHSound[SoundCount].transform.position = hitInfo.transform.position;
                FHSound[SoundCount].Play();
                SoundCount++;
            }
            else
            {
                FLSound[SoundCount].transform.position = hitInfo.transform.position;
                FLSound[SoundCount].Play();
                SoundCount++;
            }
            EffectCount1 = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.GetChild(0).gameObject, EffectCount1, 1);



        }
        else if (hitInfo.transform.CompareTag("Bullet"))
        {
            StartCoroutine(HitCross(0.3f));


            Base_Bullet temp = hitInfo.transform.GetComponent<Base_Bullet>();

            temp.Damaged(GunDamage1);
            if(temp.Armor)
            {
                FHSound[SoundCount].transform.position = hitInfo.transform.position;
                FHSound[SoundCount].Play();
                SoundCount++;
            }
            else
            {
                FLSound[SoundCount].transform.position = hitInfo.transform.position;
                FLSound[SoundCount].Play();
                SoundCount++;
            }
        }
    }
    void HitGUN2()
    {

        StartCoroutine(GunDelay(2.5f));
        SE1.Play();
        Vector3 RecoilUp = new Vector3(0, 45, 0);
        Vector3 RecoilDown = new Vector3(0, -45, 0);
        Vector3 RecoilLeft = new Vector3(-45, 00, 0);
        Vector3 RecoilRight = new Vector3(45, 0, 0);
        Up.rectTransform.localPosition = RecoilUp;
        Down.rectTransform.localPosition = RecoilDown;
        Left.rectTransform.localPosition = RecoilLeft;
        Right.rectTransform.localPosition = RecoilRight;


        RaycastHit[] hitInfos;
        Vector3 posi = cam.transform.GetChild(1).position; //startposi;
        Vector3 endposi = cam.transform.GetChild(0).position; //endposi;
        RaycastHit hit;

        GameObject VE = Pooling.transform.GetChild(1).gameObject;
        

        
        if (Physics.SphereCast(cam.transform.position, 0.75f, cam.transform.forward, out hit, GunDistance2))
        {
            hitInfos = Physics.SphereCastAll(cam.transform.position, 0.75f, cam.transform.forward, GunDistance2);

            Array.Sort(hitInfos, (RaycastHit x, RaycastHit y) => x.distance.CompareTo(y.distance)); // cast한거거리순으로정렬
                                                                                                    //foreach (RaycastHit hits in hitInfos)
                                                                                                    //{
                                                                                                    //    Debug.Log(hits.transform.name);
                                                                                                    //}

            List<int> attackEnemy = new List<int>();
            for (int i = 0; i < hitInfos.Length; i++)
            {
                if (hitInfos[i].collider.gameObject.layer == 8)

                {
                    Vector3 a = hitInfos[i].point - cam.transform.position;
                    EffectCount2 = GunEffectCount(hitInfos[i].point, hitInfos[i].normal, VE, EffectCount2, 2);
                    
                    break;

                }
                else if (hitInfos[i].collider.gameObject.layer == 10 & hitInfos[i].collider.tag != "Head")
                {

                    EffectCount2 = GunEffectCount(hitInfos[i].point, hitInfos[i].normal, VE, EffectCount2, 2);
                    StartCoroutine(HitCross(1.0f));

                    int k = 0;
                    bool ck = true;
                    Base_HP temp= hitInfos[i].transform.GetComponent<Base_HP>();
                    if(temp !=null)
                    {

                        k = temp.ID;

                        for (int j = 0; j < attackEnemy.Count; j++)
                        {
                            if (attackEnemy[j] == k)
                            {
                                ck = false;
                            }
                            if (ck == true)
                            {
                                attackEnemy.Add(k);
                                if (temp.Live)
                                {
                                    temp.Damged(GunDamage2,true);
                                }



                            }
                        }
                    }


                }
                else if (hitInfos[i].transform.CompareTag("Bullet"))
                {
                    StartCoroutine(HitCross(0.3f));

                    hitInfos[i].transform.GetComponent<Base_Bullet>().Damaged(20);
                }
            }
            L2E.Play();
        }
    }

    IEnumerator GunDelay(float Del)
    {
        SRshot = true;
        Shooting = true;
        yield return new WaitForSeconds(Del);
        SRshot = false;
        Shooting = false;
    }
    int GunEffectCount(Vector3 posi, Vector3 rot, GameObject VE, int EffectCount, int n)
    {
        if (EffectCount >= 40)
        {
            EffectCount = 0;
        }
        else if (EffectCount >= 10 & n == 2)
        {
            EffectCount = 0;
        }
        ParticleSystem effect;
        switch (n)
        {
            case 1:
                effect = VE.transform.GetChild(EffectCount).GetComponent<ParticleSystem>();
                effect.transform.position = posi;
                effect.transform.LookAt(cam.transform.position);
                effect.Play();
                break;
            case 2:
                for (int i = 0; i < 5; i++)
                {

                    effect = VE.transform.GetChild(EffectCount).GetChild(i).GetComponent<ParticleSystem>();
                    effect.transform.position = posi;
                    effect.transform.LookAt(cam.transform.position);
                    effect.transform.localScale = Vector3.one * 0.5f;
                    effect.Play();
                }
                break;
        }
        EffectCount++;


        return EffectCount;
    }
    void AnimationsActive()
    {

        SpineAction.SetBool("ARReload", ARRealod);
        SpineAction.SetBool("SRReload", SRRealod);
        SpineAction.SetBool("ARSHOT", ARshot);
        if (SRshot)
        {
            SpineAction.SetTrigger("SRSHOT");
            SRshot = false;
        }


        SpineAction.SetBool("Change", Swaping);
    }

    void Recoilimage()
    {
        Vector3 UpOriginPos = new Vector3(0, 10, 0);
        Vector3 DownOriginPos = new Vector3(0, -10, 0);
        Vector3 LeftOriginPos = new Vector3(-10, 0, 0);
        Vector3 RightOriginPos = new Vector3(10, 0, 0);
        Vector3 RecoilUp = new Vector3(0, 20, 0);
        Vector3 RecoilDown = new Vector3(0, -20, 0);
        Vector3 RecoilLeft = new Vector3(-20, 00, 0);
        Vector3 RecoilRight = new Vector3(20, 0, 0);
        //RecoilRightLeft= new Vector3(?, 2, 0); ?만큼 증가감소
        float CrossSpeed = 5.0f;

        if (GN == 1 & Shooting)
        {
            Up.rectTransform.localPosition = Vector3.Lerp(Up.rectTransform.localPosition, RecoilUp, Time.deltaTime * CrossSpeed * 12);
            Down.rectTransform.localPosition = Vector3.Lerp(Down.rectTransform.localPosition, RecoilDown, Time.deltaTime * CrossSpeed * 12);
            Left.rectTransform.localPosition = Vector3.Lerp(Left.rectTransform.localPosition, RecoilLeft, Time.deltaTime * CrossSpeed * 12);
            Right.rectTransform.localPosition = Vector3.Lerp(Right.rectTransform.localPosition, RecoilRight, Time.deltaTime * CrossSpeed * 12);
        }
        else
        {
            Up.rectTransform.localPosition = Vector3.Lerp(Up.rectTransform.localPosition, UpOriginPos, Time.deltaTime * CrossSpeed / 2);
            Down.rectTransform.localPosition = Vector3.Lerp(Down.rectTransform.localPosition, DownOriginPos, Time.deltaTime * CrossSpeed / 2);
            Left.rectTransform.localPosition = Vector3.Lerp(Left.rectTransform.localPosition, LeftOriginPos, Time.deltaTime * CrossSpeed / 2);
            Right.rectTransform.localPosition = Vector3.Lerp(Right.rectTransform.localPosition, RightOriginPos, Time.deltaTime * CrossSpeed / 2);
        }
    }
    public  IEnumerator HitCross(float time)
    {
        Up.color = Color.red;
        Right.color = Color.red;
        Left.color = Color.red;
        Down.color = Color.red;
        yield return new WaitForSeconds(time);
        Up.color = Color.white;
        Right.color = Color.white;
        Left.color = Color.white;
        Down.color = Color.white;
    }

    public void BoomGet()
    {
        BoomHave = true;
        BoomGunDelay = BoomGunMAXDelay;
        GameUI.BoomGet();
    }
}





