using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class AmmoBox : MonoBehaviour
{
    public float firstAmmo;
    public float SecondAmmo;
    GameObject positionObject;
    public Vector3 position;
    public float Xrot;
    public Animator ani;
    AudioSource open;
    public CanvasGroup canvas;
    bool canvascheck;
    void Start()
    {
        positionObject = transform.GetChild(2).gameObject;
        position = positionObject.transform.position;
        open=GetComponent<AudioSource>();
        canvas.alpha = 0.0f;
        canvascheck = false;
        
        ani = GetComponent<Animator>();
        int a = UnityEngine.Random.Range(1, 6);
        if(a ==5)
        {
            firstAmmo = 40;
            SecondAmmo = 10;
        }
        else if (a>=2)
        {
            firstAmmo = 80;
            SecondAmmo = 5;
        }
        else
        {

            firstAmmo = 120;
            SecondAmmo = 0;
        }
    }
    public void SelfDestroy(float time)
    {
        GameManager.instance.Score += 10;
        transform.GetComponent<Outline>().enabled = false;
        Destroy(gameObject,time);
    }
    public void PlayerOpenSound()
    {
        open.Play();
    }
    private void Update()
    {
        if(canvascheck)
        {
            canvas.alpha=Mathf.Lerp(canvas.alpha,1.0f,Time.deltaTime*3);
        }
        else
        {
            canvas.alpha = Mathf.Lerp(canvas.alpha, 0.0f, Time.deltaTime*3);
        }
    }

    public void OnTriggerStay(Collider other)
    {
        LayerMask Player = 9;
        
        
        if (other.gameObject.layer == Player)
        {
            
            canvascheck = true;
            
        }
        
    }
    public void OnTriggerExit(Collider other)
    {
        LayerMask Player = 9;
        
        if (other.gameObject.layer == Player)
        {
            
            canvascheck = false;
        }
        
    }

}
