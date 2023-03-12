using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Base_Gun : MonoBehaviour
{


    #region//참조오브젝트
    public Gun_Manager Main;
    public Camera cam;
    public GameObject GunModel;
    [SerializeField]
    protected GameObject FirePosition;
    [SerializeField]
    protected GameObject Pooling;
    protected int EffectCount = 0;
    #endregion
    #region//총의 현재상태
    public float Delay;//총의현재딜레이
    protected float Reload; //최대장전
    [SerializeField]
    protected float ReloadTime;//장전시간
    protected float Pack;//잔탕
    public float MAXPack;//최대잔탕
    #endregion
    #region//총의 정보
    public int GunID = 1;//들고있는 총의 번호
    [SerializeField]
    protected float GunDistance;
    protected float BoomGunDelay;
    protected float BoomGunMAXDelay;
    protected int GrenadeCount;
    protected int GrenadeMAXCount;
    [SerializeField]
    protected LayerMask layer;
    public float CurrentDelay;//총의딜레이
    public float CurrentAmmo;
    public float CurrentReload;
    public float CurrentPack;
    [SerializeField]
    protected int GunDamage = 200;
    #endregion

    [SerializeField]
    protected Animator _animator;
    

    public abstract void Init(Gun_Manager main,Animator _ani);
    public  abstract void Shoot();
    public abstract void Reload_Function();
    public abstract void Reload_After_Function();
    




}
