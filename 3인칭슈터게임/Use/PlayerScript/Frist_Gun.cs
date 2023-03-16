using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Frist_Gun : Base_Gun
{

    public override void Init(Gun_Manager main, Animator _ani)
    {
        Main=main;
        _animator=_ani;
    }
    
    #region //소리오브젝트/이펙트
    int Fcount;
    public AudioSource[] FLSound;
    public AudioSource[] FHSound;
    public AudioSource[] FirstGunSound;
    int SoundCount;
    public ParticleSystem L1E;
    #endregion

    public override void Shoot()
    {
        #region //장전된총알이없고 장전할총알이있을경우 장전작동
        if (((Input.GetMouseButton(0) && CurrentAmmo == 0) && CurrentPack >= 1))
        {
            Reload_Function();
        }
        #endregion
        #region //발사

        else if (Input.GetMouseButton(0) && Delay <= 0 & CurrentAmmo >= 1)//장전된총알이 있고 총알딜레이가 0이하일경우 발사
        {
            Main.PIN = false;
            Delay = CurrentDelay; //딜레이을 상승
            CurrentAmmo -= 1;//총알소모
            Main.Shooting = true;//사격애니메이션작동을위한변수
            HitGUN1();//사격
            L1E.Play();//머즐플래시작동
            FirstGunSound[Fcount].Play();//오브젝트풀링해둔소리오브젝트 작동 (총을쏠때 같은소리가 아닌 비슷한다른소리로 좀더 디테일을 넣음)
            Fcount++;//플레이 이후상승하는 이유는 배열을 초과할수있기때문
            if (Fcount == FirstGunSound.Length)
            {
                Fcount = 0;
            }
            Main.ARshot = true;
        }
        #endregion
        #region//작동되지않았음
        else
        {
            Main.ARshot = false;
            Main.Shooting = false;
        }
        #endregion
        #region//반동작동
        if (Main.ARshot == true)//쐇다면 반동적용
        {
            Main.Recoil = true;
        }
        else
        {
            Main.Recoil = false;
        }
        #endregion

    }
    public override void Reload_Function()
    {
        if(CurrentAmmo==CurrentReload)
            return;
        Main.PIN = false;

        _animator.CrossFade("reload_shoot_ar", 0.2f);
    }
    public override void Reload_After_Function()
    {
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
    }
    








    void HitGUN1()//사격기능
    {
        #region //목표물 적중시 인덱스값 확인
        if (SoundCount >= FLSound.Length)
        {
            SoundCount = 0;
        }
        #endregion
        RaycastHit hitInfo;//레이캐스트 히트 선언
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hitInfo, GunDistance, layer))
        //카메라 기준 전방으로 발사되어 총의사거리만큼 레이캐스트를 실행
        {
            Vector3 dir = hitInfo.point - FirePosition.transform.position;
            if (Physics.Raycast(FirePosition.transform.position, dir, out hitInfo, GunDistance, layer))
            //총기발사구에서 히트된대상방향으로 총의사거리만큼 레이캐스트를 실행
            //레이캐스틀 두번하는 이유는 시점상 플레이어총구는 벽을향하는데 카메라가 적을 볼 수 있는 상황에서 총이 적중을하면 괴리감이 보이기 때문입니다.
            {
                #region//착탄지점으로 오브젝트이동후 착탄소리작동
                if (hitInfo.collider.gameObject.layer == 8)
                {
                    EffectCount = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.gameObject, EffectCount);
                    FLSound[SoundCount].transform.position = hitInfo.transform.position;
                    FLSound[SoundCount].Play();
                    SoundCount++;
                }
                #endregion
                HitHeadShot1(hitInfo);//사격적중시작동
            }
        }
        Main.Move.GunRecoilActive();////총기반동작동
        L1E.Stop();//머즐플래시동작멈춤
    }


    int GunEffectCount(Vector3 posi, Vector3 rot, GameObject VE, int EffectCount)
    {
        if (EffectCount >= Pooling.transform.childCount)//40을 풀링최대갯수찾아서넣기
        {
            EffectCount = 0;
        }

        ParticleSystem effect;

        effect = VE.transform.GetChild(EffectCount).GetComponent<ParticleSystem>();
        effect.transform.position = posi;
        effect.transform.LookAt(cam.transform.position);
        effect.Play();


        EffectCount++;


        return EffectCount;
    }





    void HitHeadShot1(RaycastHit hitInfo)
    {
        
        if (hitInfo.transform.CompareTag("Head") & hitInfo.collider.gameObject.layer == 10)//약점부분인지태그,적인지 레이어 비교
        {
            #region//약점적중기능

            StartCoroutine(Main.HitCross(0.3f));
            Base_HP temp = hitInfo.transform.GetComponent<Base_HP>();
            if (temp != null)
            {
                if (temp.Armor)
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

                temp.Damged(GunDamage * 5);



                EffectCount = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.gameObject, EffectCount);
            }
            #endregion
        }
        else
        {

            HitShot1(hitInfo);//약점이아니라면 일반 적중
        }
    }//약점 부위 명중했을 경우 작동 약점부위가아니라면 HitShot작동
    void HitShot1(RaycastHit hitInfo)
    {

        if (hitInfo.transform.CompareTag("hitbox") & hitInfo.collider.gameObject.layer == 10)//히트박스태그, 적인지 레이어 비교
        {
            Debug.Log("충돌");
            StartCoroutine(Main.HitCross(0.3f));//적중시 0.3초간 적중을 표시하기위해 크로스헤어 색깔변경    
            Base_HP temp = hitInfo.transform.GetComponent<Base_HP>();//추상클래스인 체력을 가져옴
            temp.Damged(GunDamage);//총만큼 데미지
            #region//적의 방어타입에 따라 소리출력
            if (temp.Armor)
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
            EffectCount = GunEffectCount(hitInfo.point, hitInfo.normal, Pooling.transform.gameObject, EffectCount);
            #endregion
        }
        else if (hitInfo.transform.CompareTag("Bullet"))
        {
            StartCoroutine(Main.HitCross(0.3f));//적중시 0.3초간 적중을 표시하기위해 크로스헤어 색깔변경
            Base_Bullet temp = hitInfo.transform.GetComponent<Base_Bullet>();//추상클래스인 적공격의 투사체의 체력을 가져옴
            //적의 공격 투사체를 맞춰서 격추시킬 수 있는 기능
            temp.Damaged(GunDamage);//총만큼 데미지
            #region//투사체의 방어타입에 따라 소리출력
            if (temp.Armor)
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
            #endregion
        }
    }//사격시 명중했을경우 작동
}