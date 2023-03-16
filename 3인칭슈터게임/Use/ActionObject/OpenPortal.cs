using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenPortal : MonoBehaviour
{
    public GameUIManager gameUI;
    
    public GameObject Teleport;
    
    public AudioSource OpenPortalSound;
    public bool State;
    public Transform ActivePosi;
    public Vector3 position;//작동하는기계앞
    public Vector3 rotate;//작동하는기계를바라보는방향
    public CanvasGroup canvas;
    // Start is called before the first frame update
    void Start()
    {
        State = true;
        position = ActivePosi.position;
            
    }

    // Update is called once per frame
    void Update()
    {

        if (Vector3.Distance(GameManager.instance.PlayerMove.gameObject.transform.position, transform.position) < 15.0f)
        {
            canvas.transform.parent.transform.LookAt(GameManager.instance.Cam_Player_Position.position);
            canvas.alpha = Mathf.Lerp(canvas.alpha, 1, Time.deltaTime);
        }
        else
        {
            canvas.alpha = Mathf.Lerp(canvas.alpha, 0, Time.deltaTime);
        }
    }
    public void OpenPortalFunction()
    {

        if (gameUI.BoomCheck)
        {
            State = false;




            transform.GetChild(0).GetComponent<Outline>().enabled = false;
            //OpenPortalSound.Play();


            canvas.gameObject.SetActive(false);
            for (int i = 0; i < Teleport.transform.GetChild(2).GetChild(0).childCount; i++)
            {
                Teleport.transform.GetChild(2).GetChild(0).GetChild(i).GetComponent<ParticleSystem>().Play();



            }
            Teleport.GetComponent<Portal>().Active = true;
            
        }
        else
        {
            CantTeleport();
        }
        
    }

    public void CantTeleport()
    {
        if(gameUI.PopupCheck==false)
        {
            gameUI.PopupCheck = true;
        }

        StartCoroutine(DelayText());
        gameUI.Popup.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "실패";
        gameUI.Popup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "작동하기위한 장비를 획득하십시오.";
    }
    IEnumerator DelayText()
    {
        gameUI.AnotherThing = true;
        yield return new WaitForSeconds(3.0f);
        gameUI.AnotherThing = false;
    }
}
