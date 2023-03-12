using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class OpenDoor : MonoBehaviour
{
    public GameUIManager gameUI;
    Animation ani;
    public GameObject Door;
    public GameObject Light;
    public AudioSource OpenDoorSound;
    public bool State;
    public bool OneCount;
    public bool DisCount;
    public Vector3 position;//작동하는기계앞
    public Vector3 rotate;//작동하는기계를바라보는방향
    public CanvasGroup canvas;
    void Start()
    {
        State = true;
        ani = Door.GetComponent<Animation>();
        OneCount = true;
       canvas= transform.GetChild(1).GetChild(1).GetChild(0).GetChild(0).GetComponent<CanvasGroup>();
        canvas.alpha = 0;
        DisCount = false;
    }
    private void Update()
    {
        if(GameManager.instance.EnemyID.Count==0&OneCount==true)
        {
            State=false;
            OneCount=false;
            Light.SetActive(true);
        }
        if(Vector3.Distance(GameManager.instance.PlayerMove.gameObject.transform.position, transform.position)<15.0f)
        {
            
            DisCount = true;
        }
        else
        {
            DisCount = false;
        }
        if(DisCount==true)
        {
            canvas.transform.parent.transform.LookAt(GameManager.instance.Cam_Player_Position.position);
            canvas.transform.parent.transform.Rotate(0, -180, 0);
            //canvas.transform.LookAt(GameManager.instance.Cam_Player_Position.position);
            //canvas.transform.Rotate(0, -180, 0);
            canvas.alpha = Mathf.Lerp(canvas.alpha, 1, Time.deltaTime);
        }
        else
        {
            canvas.alpha = Mathf.Lerp(canvas.alpha, 0, Time.deltaTime);
        }






    }
    public void CantOpenDoor()
    {
        if(gameUI.PopupCheck==false)
        {
            gameUI.PopupCheck = true;
        }

        StartCoroutine(DelayText());
        gameUI.Popup.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "문을 여는데 실패하였습니다.";
        gameUI.Popup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "아직 적들이 남았습니다.";
    }

    IEnumerator DelayText()
    {
        gameUI.AnotherThing = true;
        yield return new WaitForSeconds(3.0f);
        gameUI.AnotherThing = false;
    }
    public void OpenAction()
    {
        ani.Play("BigDoorOpen");
        transform.GetChild(1).GetChild(1).GetComponent<Outline>().enabled = false;
        OpenDoorSound.Play();
        //State = true;
        gameObject.GetComponent<OpenDoor>().enabled = false;
        gameObject.GetComponent<Collider>().enabled = false;
        canvas.gameObject.SetActive(false);
        gameUI.MissionNumber = 1;
    }
}
