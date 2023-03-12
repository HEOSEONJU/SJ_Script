using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class StageUI : MonoBehaviour
{
    // Start is called before the first frame update

    


    public List<Sprite> Stage_Ready_Image;


    public GameObject Object;

    public Camera Mian;

    public GameObject StageButton;
    public List<Image> StageImage;



    public FailWinodw POPUP_1;


    public List<int> StageCount;
    int StaminaUse=1;


    private void Awake()
    {
        StaminaUse = 1;
        
        StageCount = new List<int>();
        for (int i=0;i<StageButton.transform.childCount;i++)
        {
            StageImage.Add(StageButton.transform.GetChild(i).GetComponent<Image>());
            StageCount.Add(i + 1);
        }

        
        
        

    }



    private void OnEnable()
    {
        ResetStageStatus();
    }

    public void ResetStageStatus()
    {

        
        for (int i = 0; i < StageImage.Count; i++)
        {
            int temp = StageCount[i];
            switch (FireBaseDB.instacne.Player_Data_instacne.StageClear[i])
            {
                case 0:
                    StageImage[i].sprite = Stage_Ready_Image[2];
                    
                    StageImage[i].transform.GetChild(0).gameObject.SetActive(true);
                    StageImage[i].transform.GetChild(1).gameObject.SetActive(false);
                    break;
                case 1:
                    StageImage[i].sprite = Stage_Ready_Image[1];
                    StageImage[i].GetComponent<Button>().onClick.AddListener(() => StageMove(temp));
                    StageImage[i].transform.GetChild(0).gameObject.SetActive(false);
                    StageImage[i].transform.GetChild(1).gameObject.SetActive(true);
                    StageImage[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (i + 1);
                    break;
                case 2:
                    StageImage[i].sprite = Stage_Ready_Image[0];
                    StageImage[i].GetComponent<Button>().onClick.AddListener(() => StageMove(temp));
                    StageImage[i].transform.GetChild(0).gameObject.SetActive(false);
                    StageImage[i].transform.GetChild(1).gameObject.SetActive(true);
                    StageImage[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + (i + 1);
                    break;
                default:
                    break;
            }
            
            

        }
    }

    public void StageMove(int a)
    {
        if (Stamin_Use_Fuction())
        {
            if (FireBaseDB.instacne.Player_Data_instacne.StageClear[a - 1] != 0)
            {
                FireBaseDB.instacne.Upload_Data(StoreTYPE.STAMINA);
                
                Mian.transform.GetComponent<ManagementMainUI>().Enemy_Manager.ID = a;

                Mian.enabled = false;

                Object.SetActive(true);
                gameObject.SetActive(false);
            }
        }
        
    }
    bool Stamin_Use_Fuction()
    {
        if(FireBaseDB.instacne.Player_Data_instacne.Stamina >= StaminaUse)
        {
            FireBaseDB.instacne.Player_Data_instacne.Stamina -= StaminaUse;
            return true;
        }
        else
        {
            POPUP_1.View_Text("스태미나가 모자랍니다");
            return false;
        }
    }
    public void Lock_View()
    {

        POPUP_1.View_Text("현재 스테이지는 잠겨있습니다");
    }

    



}
