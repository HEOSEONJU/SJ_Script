using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSoundManager : MonoBehaviour
{
    public EnemyBossHP HPanimation;
    public EnemyBossMove Moveanimation;


    [Header("°È´Â¼Ò¸®")]
    public AudioSource WalkSound;
    public AudioSource WalkSound2;


    [Header("½ÇµåÆÄ±«")]
    public AudioSource ShieldDestroy;
    public AudioSource ShieldOnlne;

    [Header("½ºÅèÇÁ")]
    public AudioSource Stomp;

    [Header("Á×À½")]
    public AudioSource Death;


    

    public void RegenShieldFunction()
    {
        HPanimation.RegenShieldFunction();
    }
    public void DoneAttack()
    {
        Moveanimation.DoneAttack();
    }
    public void FirstDoneAttack()
    {
        Moveanimation.FirstDoneAttack();
    }
    public void StopWalking()
    {
        Moveanimation.StopWalking();
    }
    public void LandingTrigger()
    {
        Moveanimation.LandingTrigger();
    }
    public void MeleeFunction()
    {
        Moveanimation.MeleeFunction();
    }
    public void WlakSound1Play()
    {
        
        WalkSound.Play();
    }
    public void WlakSound2Play()
    {
        WalkSound2.Play();
    }
    public void ShieldDestroyPlay()
    {
        ShieldDestroy.Play();
    }

    public void ShieldOnlnePlay()
    {
        ShieldOnlne.Play();
    }


    public void StompPlay()
    {
        

        Stomp.Play();
    }

    
    public void DeathPlay()
    {
        Death.Play();
    }





}
