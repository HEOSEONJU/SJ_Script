using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public GameUIManager gameUI;
    public bool Active;
    public float time;
    public Transform MovePoint;
    public GameObject BossTrigger;
    void Start()
    {
        Active = false;
        time = 0;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (Active & other.gameObject.layer == 9)
        {
            time += Time.deltaTime;

            if(time>=2)
            {
                Color color = gameUI.Fadeimage.color;
                color.a+=Time.deltaTime/3f;
                gameUI.Fadeimage.color=color;
            }

            if(time>=5)
            {
                //캐릭터 보스존으로이동
                gameUI.MissionNumber = 2;
                enabled = false;
                GameManager.instance.PlayerMove.transform.position= MovePoint.position;
                GameManager.instance.PlayerMove.transform.rotation=Quaternion.Euler(0,0,0);
                GameManager.instance.PlayerMove.transform.GetComponent<Player_Manager>().TurnX = 0;
                BossTrigger.SetActive(true);

            }
        }
        else
            time = 0;
            
    }
    private void OnTriggerExit(Collider other)
    {
        StartCoroutine(FadeUn());

    }
    IEnumerator FadeUn()
    {

        Color color = gameUI.Fadeimage.color;
        while (color.a > 0)
        {
            color = gameUI.Fadeimage.color;
            color.a -= Time.deltaTime;

            gameUI.Fadeimage.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }

}
