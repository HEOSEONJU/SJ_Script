using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class UIShop : MonoBehaviour
{
    public PlayerInfos Data;
    public UIDataUpdate my;


    public GameObject POPUP;
    public TextMeshProUGUI CostText;
    public TextMeshProUGUI GetText;

    public FailWinodw POPUP_1;

    int cost;
    int gold;
    [SerializeField]
    Image FadeImage;
    private void Start()
    {
        my = GetComponent<UIDataUpdate>();
    }

    private void OnEnable()
    {
        CloseWindow();
        //POPUP.SetActive(false);
        cost = 0;
        gold= 0;
    }

    public void AddGem(int num)
    {
        FireBaseDB.instacne.Player_Data_instacne.Gem += num;
        FireBaseDB.instacne.Upload_Data(StoreTYPE.GEM);//현금구매미구현이라 코스트없음
        my.UpdateData();
    }



    
    public void CloseWindow()
    {
        FadeImage.enabled = false;
        POPUP.transform.localScale = Vector3.zero;

    }
    IEnumerator Open_WindowCorountine()
    {
        FadeImage.enabled = true;
        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime * 6;

            POPUP.transform.localScale = Vector3.one * time;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }











    public void OpnePopup(int Value)
    {
        StartCoroutine(Open_WindowCorountine());
        cost = Value;

        switch (cost)
        {
            case 8000:
                gold = 120000;
                break;
            case 4000:
                gold = 55000;
                break;
            case 2000:
                gold = 27000;
                break;
            case 1000:
                gold = 13000;
                break;
            case 500:
                gold = 6000;
                break;
            case 100:
                gold = 1000;
                break;
        }

        CostText.text = cost + "";
        GetText.text = gold + "";
    }
    public void ClosePopup()
    {
        CloseWindow();
    }





    public void AddGold()
    {
        if (FireBaseDB.instacne.Player_Data_instacne.Gem >= cost)
        {

            FireBaseDB.instacne.Player_Data_instacne.Gem -= cost;
            FireBaseDB.instacne.Player_Data_instacne.Gold += gold;

            my.UpdateData();
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GEM);
            ClosePopup();
        }
        else
        {
            POPUP_1.View_Text("젬이 부족합니다.");
            
            ClosePopup();
        }
    }
    public void AddStamina(int num)
    {
        FireBaseDB.instacne.Player_Data_instacne.Stamina += num;
        FireBaseDB.instacne.Upload_Data(StoreTYPE.STAMINA);//현금구매미구현이라 코스트없음
        my.UpdateData();
    }



}
