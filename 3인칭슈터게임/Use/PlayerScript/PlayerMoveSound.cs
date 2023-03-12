using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveSound : MonoBehaviour
{
    Player_Manager _Manager;
    

    public bool Spine;


    [Header("걷는소리")]
    public AudioSource WalkSound;
    public AudioSource WalkSound2;
    [Header("부스터")]
    public AudioSource BoosterStart;
    public AudioSource Boostering;
    public AudioSource Boosterend;
    public ParticleSystem[] BoosterEffect;
    [Header("버툰소리")]
    public AudioSource FirstButton;
    public AudioSource SecondButton;
    public AudioSource ThirdButton;

    [Header("1번총")]
    public AudioSource[] ARReload;
    
    [Header("2번총")]
    public AudioSource SRCock;
    public AudioSource[] SRReload;
    public AudioSource SRFire;
    




    [Header("3번총")]
    public AudioSource HGFire;

    private void Start()
    {

        _Manager=GetComponentInParent<Player_Manager>();
    }
    private void Update()
    {
        if(_Manager.Action)
        {
            Boostering.Stop();
        }

    }


    public void DIeAnimation()
    {

        GameManager.instance.GameUI_Manager.FadeOut();
    }

    public void HGsound()
    {
        
        HGFire.Play();
    }
    public void ThrowGrenade()
    {
        _Manager.Grenade_Manager.GrendaeThrow_INDEX_Function();
        _Manager.Shoot_Manager.PIN = false;
    }

    






    public void WalkSoundActive()
    {
        WalkSound.Play();
    }
    public void WalkSoundActive2()
    {
        WalkSound2.Play();
    }
    public void BoosterStartActive()
    {
        if (!Boostering.isPlaying)
        {
            BoosterStart.Play();
            
        }
        for (int i = 0; i < BoosterEffect.Length; i++)
        {
            BoosterEffect[i].Play();
        }
    }
    public void BoosteringActive()
    {
        if (!Boostering.isPlaying)
        {
            Boostering.Play();   
        }
        
        

    }
    public void BoosterendActive()
    {
        if (!BoosterStart.isPlaying)
        {
            BoosterStart.Stop();
        }

        
            for (int i = 0; i < BoosterEffect.Length; i++)
            {
                BoosterEffect[i].Stop();
            }
        
            Boosterend.Play();
    }


    public void FirstButtonAction()
    {
        FirstButton.Play();
    }
    public void SecondButtonAction()
    {
        SecondButton.Play();
    }
    public void ThirdButtonAction()
    {
        ThirdButton.Play();
    }


    public void SRReloadFirst()
    {
        SRReload[0].Play();
    }
    public void SRReloadSecond()
    {
        SRReload[1].Play();
    }
    public void SRReloadThird()
    {
        SRReload[2].Play();
    }
    public void FireSR()
    {
        
        SRFire.Play();
    }
    public void SRCockAction()
    {
        SRCock.Play();

    }
    public void ARReloadFirst()
    {
        ARReload[0].Play();
    }
    public void ARReloadSecond()
    {
        ARReload[1].Play();
    }
    public void ARReloadThird()
    {
        ARReload[2].Play();
    }

}
