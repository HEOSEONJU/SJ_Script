using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CardPack : MonoBehaviour
{
    // Start is called before the first frame update
    
    UIDataUpdate UIData;
    FailWinodw POPUP_1;

    public GameObject GEM_PoPUp;
    public GameObject GOLD_PoPUp;
    public TextMeshProUGUI GEM_ValueText;
    public TextMeshProUGUI GOLD_ValueText;
    public TextMeshProUGUI Gem_GETText;
    public TextMeshProUGUI GOLD_GETText;
    int value;
    int cost;
    public List<GameObject> Chest_Gems;
    public List<GameObject> Chest_Golds;
    

    public bool check;

    [SerializeField]
    Image FadeImage;
    void Awake()
    {
        POPUP_1 = GetComponent<FailWinodw>();
        UIData = GetComponent<UIDataUpdate>();
        

        CloseWindow();
    }

    private void OnEnable()
    {
        check = false;
        value = 0;
        cost= 0;
        FadeImage.enabled = false;
    }


    public void CloseWindow()
    {
        FadeImage.enabled = false;
        GEM_PoPUp.transform.localScale = Vector3.zero;
        GOLD_PoPUp.transform.localScale = Vector3.zero;

    }
    IEnumerator Open_WindowCorountine()
    {
        FadeImage.enabled = true;
        float time = 0;
        while (time <= 1)
        {
            time += Time.deltaTime * 6;
            if(check)

                GOLD_PoPUp.transform.localScale = Vector3.one * time;
            else
                GEM_PoPUp.transform.localScale = Vector3.one * time;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }




    public void SwapShop()
    {
        check = !check;
        if(check)
        {
            for (int i = 0; i < Chest_Gems.Count; i++)
            {
                Chest_Golds[i].SetActive(true);
                Chest_Gems[i].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < Chest_Gems.Count; i++)
            {
                Chest_Golds[i].SetActive(false);
                Chest_Gems[i].SetActive(true);
            }

        }
    }


    public void Open_GEM_PoPUp(int a)
    {
        StartCoroutine(Open_WindowCorountine());
        value = a;

        switch (value)
        {
            case 1:
                cost = 100;
                break;
            case 6:
                cost = 500;
                break;
            case 13:
                cost = 1000;
                break;
            case 27:
                cost = 2000;
                break;
            case 55:
                cost = 4000;
                break;
            default:
                cost = 1000;
                break;
        }
        GEM_ValueText.text = cost + "";
        Gem_GETText.text = a.ToString();
        
    }
    public void Close_GEM_PoPUp()
    {
        CloseWindow();
    }


    public void CardPackGetGemUse()
    {

        

        if(FireBaseDB.instacne.Player_Data_instacne.Gem>=cost)
        {
            FireBaseDB.instacne.Player_Data_instacne.Gem -= cost;
            switch(cost)
            {
                case 100:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 1;
                    break;
                case 500:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 6;
                    break;
                case 1000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 13;
                    break;
                case 2000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 27;
                    break;
                case 4000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 55;
                    break;
                    
            }
            UIData.UpdateData();
            FireBaseDB.instacne.Upload_Data(StoreTYPE.PACK);
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GEM);
        }
        else
        {
            POPUP_1.View_Text("티켓을 사기위한 젬이 부족합니다.");
        }
        Close_GEM_PoPUp();
    }


    public void Open_GOLD_PoPUp(int a)
    {
        StartCoroutine(Open_WindowCorountine());
        value = a;

        
        switch (value)
        {
            case 1:
                cost = 1000;
                break;
            case 6:
                cost = 5000;
                break;
            case 13:
                cost = 10000;
                break;
            case 27:
                cost = 20000;
                break;
            case 55:
                cost = 40000;
                break;
            default:
                cost = 0;
                break;
        }

        GOLD_ValueText.text = cost + "";
        GOLD_GETText.text = a.ToString();
        
    }
    public void Close_GOLD_PoPUp()
    {
        CloseWindow();
    }






    public void CardPackGetGoldUse( )
    {

        if (FireBaseDB.instacne.Player_Data_instacne.Gold >= cost)
        {
            FireBaseDB.instacne.Player_Data_instacne.Gold -= cost;
            switch (cost)
            {
                case 1000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 1;
                    break;
                case 5000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 6;
                    break;
                case 10000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 13;
                    break;
                case 20000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 27;
                    break;
                case 40000:
                    FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += 55;
                    break;
                default:
                    break;

            }

            UIData.UpdateData();
            FireBaseDB.instacne.Upload_Data(StoreTYPE.PACK);
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
        }
        else
        {
            POPUP_1.View_Text("티켓을 사기위한 골드가 부족합니다.");
            
        }
        Close_GOLD_PoPUp();
    }
}

