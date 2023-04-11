using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using UnityEngine.EventSystems;
using System.Reflection;


public class Quest_Function : MonoBehaviour
{
    Quest_NPC _NPC;
    [SerializeField]
    GameObject Init_Quest_Object;//생성할 퀘스트 오브젝트
    [SerializeField]
    Transform Init_Posi;





    [SerializeField]
    int Current_INDEX;


    [SerializeField]
    TextMeshProUGUI Title_Text;
    [SerializeField]
    TextMeshProUGUI Description_Text;
    [SerializeField]
    TextMeshProUGUI Complte_Text;

    [SerializeField]
    GameObject OK_Button;
    [SerializeField]
    GameObject Clear_Button;


    [SerializeField]
    VerticalLayoutGroup VLG;
    public void Init(Quest_NPC NPC, List<int> Quest_List)
    {
        _NPC = NPC;
        Title_Text.text = "";
        Description_Text.text = "";
        Complte_Text.text = "";
        for (int i = 0; i < Quest_List.Count; i++)
        {
            var e = Instantiate(Init_Quest_Object, Init_Posi);
            e.name = Quest_List[i].ToString();
            Quest_Process T = Game_Master.instance.QLO.Search_Quest(Quest_List[i]);
            e.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = T.Quest_Title;
            int temp = i;
            e.transform.GetComponentInChildren<Button>().onClick.AddListener(delegate { View_Quest(temp); });

        }
    }
    public void Reset_Page()
    {
        VLG.spacing = 0;

        Title_Text.alpha = 0;
        Description_Text.alpha = 0;
        Complte_Text.alpha = 0;
        OK_Button.SetActive(false);
        Clear_Button.SetActive(false);
    }
    public void View_Quest(int Selected_INDEX)
    {

        Current_INDEX = _NPC.Have_Quest_ID[Selected_INDEX];
        Quest_Process QB = Game_Master.instance.QLO.Search_Quest(Current_INDEX);
        if (Game_Master.instance.PM.Data.Complted_Quest.FindIndex(x => x == Current_INDEX) != -1)//이미 클리어한 퀘스트
        {
            Title_Text.text = QB.Quest_Title;
            Description_Text.text = QB.Quest_Description;
            Complte_Text.text = "이미 클리어한 퀘스트 입니다.";
            OK_Button.SetActive(false);
            Clear_Button.SetActive(false);
            Title_Text.alpha = 1;
            Description_Text.alpha = 1;
            Complte_Text.alpha = 1;
            Invoke("Invoke_Chagne_spacing", Time.deltaTime * 5);//UI오토리사이즈 때문에 딜레이를 줘야함
            return;
        }
        int INDEX = Game_Master.Instance.PM.PQB.QBL.FindIndex(x => x.Quest_ID == Current_INDEX);
        if (INDEX == -1)//이미받은 퀘스트 가 아님
        {

            if (!QB.Can)//선행조건 비 만족
            {
                Title_Text.text = QB.Quest_Title;
                Description_Text.text = QB.Quest_Description;
                Complte_Text.text = "선행퀘스트가 있는 퀘스트입니다..";
                OK_Button.SetActive(false);
                Clear_Button.SetActive(false);
                Title_Text.alpha = 1;
                Description_Text.alpha = 1;
                Complte_Text.alpha = 1;
                return;
            }



            Title_Text.text = QB.Quest_Title;
            Description_Text.text = QB.Quest_Description;
            Complte_Text.text = "보상 :" + QB.View_Reward_List();


            OK_Button.SetActive(true);
            Clear_Button.SetActive(false);
        }



        else//받은 퀘스트 정보 있음
        {
            Quest_Process QH = Game_Master.instance.PM.PQB.QBL[INDEX];
            Title_Text.text = QH.Quest_Title;



            if (QH.Progress < QH.MaxProgress)
            {
                Description_Text.text = QH.List_Process[QH.Progress].Quest_Description;
                Complte_Text.text = "다음 대상을 찾아가기";
            }
            else
            {
                Description_Text.text = "완료버툰을 누르고 하단의 보상을 받자";
                Complte_Text.text = "보상 :" + QB.View_Reward_List();
            }
            OK_Button.SetActive(false);
            Clear_Button.SetActive(true);
        }
        Title_Text.alpha = 1;
        Description_Text.alpha = 1;
        Complte_Text.alpha = 1;
        Invoke("Invoke_Chagne_spacing", Time.deltaTime * 5);



    }
    void Invoke_Chagne_spacing()
    {
        VLG.spacing = 20;
    }
    public void Accept()
    {
        Debug.Log(Current_INDEX);
        if (Current_INDEX == 0)
        {
            return;
        }

        Quest_Process QB = Game_Master.instance.QLO.Search_Quest(Current_INDEX);

        if (QB != null && QB.Accept())
        {
            Game_Master.instance.PM.PQB.Accept_Quest(QB);
            Reset_Page();
            return;
        }
        Debug.Log("수락실패");

    }

    public void Clear_Quest()
    {
        int INDEX = Game_Master.Instance.PM.PQB.QBL.FindIndex(x => x.Quest_ID == Current_INDEX);
        if (INDEX == -1)//받은적없는 퀘스트
        {
            return;
        }

        if (Game_Master.Instance.PM.PQB.QBL[INDEX].Reward())
        {
            Game_Master.Instance.PM.PQB.QBL.RemoveAt(INDEX);
            Game_Master.instance.PM.Data.Complted_Quest.Add(Current_INDEX);
            Game_Master.instance.PM.Save_On_FireBase();
            Reset_Page();
        }
    }
    public void Take_Item()
    {
        _NPC.Text.text = Game_Master.Instance.PM.PQB.Check_Give_Item();
        
    }

}

