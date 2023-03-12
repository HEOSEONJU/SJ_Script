using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoomBox : MonoBehaviour
{


    public bool ck;
    public Vector3 position;//상자열때플레이어위치
    public float Xrot;//상자열떄플레이어각도 수동으로잡아야줘야함
    public Animator ani;//상자열리는애니메이션
    AudioSource open;//상자열리는소리
    public CanvasGroup canvas;//버툰UI
    bool canvascheck;//버툰보이는지안보이는지

    void Start()
    {
        Xrot = 0;
        position = transform.GetChild(0).position;
        open = GetComponent<AudioSource>();
        ani=GetComponent<Animator>();
        canvas.alpha = 0.0f;
        canvascheck = false;
        ck = true;
        
        
    }
    public void SelfDestroy(float time)
    {
        transform.GetChild(2).GetChild(4).GetComponent<Outline>().enabled = false;
        
    }
    public void PlayerOpenSound()
    {
        open.Play();
        GameManager.instance.Score += 20;
        Destroy(gameObject, 3.0f);


    }
    
    private void Update()
    {
        
        if (Vector3.Distance(transform.position,GameManager.instance.Cam_Player_Position.position)<8.0f)
        {
            canvas.alpha = Mathf.Lerp(canvas.alpha, 1.0f, Time.deltaTime * 3);
        }
        else
        {
            canvas.alpha = Mathf.Lerp(canvas.alpha, 0.0f, Time.deltaTime * 3);
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
