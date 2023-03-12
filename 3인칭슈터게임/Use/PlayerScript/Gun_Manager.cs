using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.VFX;
using System;

public class Gun_Manager : MonoBehaviour
{
    public bool Reloading, Swaping, Shooting;//장전애니메이션/스왚/사격
    
    public bool HandGun, Grenade, PIN;



    public bool GunAction;

    [Header("Grenade")]
    public LineRenderer Line;
    //애니메이션 체크상태용 
    [Header("Animation")]
    
    public Animator SpineAction;
    public Camera cam;
    public Camera subcam;
    public Player_Manager Move;
    
    
    public bool ARshot;
    public bool SRshot;
    [SerializeField]
    public List<Base_Gun> GunList;
    public Base_Gun Equip_Gun;
    public GameObject Equip_Gun_Model;
    [SerializeField]
    bool BoomHave;

    public int Swap_GN;

    public void Init_GUN()
    {
        foreach (var gun in GunList) 
        {
            gun.Init(this, SpineAction);
        }
    }

    public void GunSWap()
    {
        if (Shooting == false && Reloading == false)
        {

            
            if (Input.GetKey(KeyCode.Alpha1))
            {
                Swap_GN = 1;
                if (Equip_Gun.GunID != Swap_GN)
                {
                    GunSwapFunction(Swap_GN, Equip_Gun.GunID);
}
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                Swap_GN = 2;
                if (Equip_Gun.GunID != Swap_GN)
                {
                    GunSwapFunction(Swap_GN, Equip_Gun.GunID);
                }
            }
            else if (Input.GetKey(KeyCode.Alpha3))
            {
                Swap_GN = 3;
                if (Equip_Gun.GunID != Swap_GN && BoomHave == true)
                {
                    GunSwapFunction(Swap_GN, Equip_Gun.GunID);
                }
            }

        }

    }
    public void GunSwapFunction(int GunNumber, int CurrentGN)
    {
        
        PIN = false;
        if (Line.enabled == true)
        {
            Line.enabled = false;
        }
        
        SpineAction.CrossFade("weaponChange_rest", 0.05f);
        
        

        

        
    }
    public void After_Swap()
    {
        int BefreGN = Equip_Gun.GunID;
        Equip_Gun_Model.SetActive(false);

        Equip_Gun = GunList[Swap_GN - 1];
        Equip_Gun_Model = Equip_Gun.GunModel;
        Equip_Gun_Model.SetActive(true);
        


        GameUI.GunSwapImage(BefreGN, Equip_Gun.GunID);
        if (Equip_Gun.GunID == 3)
        {
            SpineAction.SetTrigger("HandGun");
        }


        

        //Swaping = false;
        //SpineAction.SetBool("Change", Swaping);

    }


    public void CanAction()
    {
        if (Reloading == false & Swaping == false & Move.Action == false)//캐릭터가 행동가능조건추가해야함
        {
            GunAction=  false;

        }
        else
        {
            GunAction=  true;
        }
    }

    public void AimChange()
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
    public void AimMode()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Aimfloat, Time.deltaTime * 5.0f);
        subcam.fieldOfView = Mathf.Lerp(cam.fieldOfView, Aimfloat, Time.deltaTime * 5.0f);
    }












    public void AnimationsActive()
    {

        
        SpineAction.SetBool("ARSHOT", ARshot);
        if (SRshot)
        {
            SpineAction.SetTrigger("SRSHOT");
            SRshot = false;
        }


        SpineAction.SetBool("Change", Swaping);
    }


    [SerializeField]
    List<Collider> GunCollider;
    public void Collideroff()
    {
        if ((Swaping || Move.Action) == true)
        {
            for (int i = 0; i < GunCollider.Count; i++)
            {
                GunCollider[i].enabled = false;
            }
        }
        else
        {
            for (int i = 0; i < GunCollider.Count; i++)
            {
                GunCollider[i].enabled = true;
            }
        }
    }




    [Header("CrossHair")]
    public Image Up;
    public Image Down;
    public Image Right;
    public Image Left;
    public GameUIManager GameUI;
    public float Aimfloat=30f ;
    public bool Recoil;
    public bool Aim;
    public Vector3 LaserVector = Vector3.zero;

    public void Recoilimage()
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

        if (Equip_Gun.GunID == 1 & Shooting)
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
    public IEnumerator HitCross(float time)
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
        
        GameUI.BoomGet();
    }
}
