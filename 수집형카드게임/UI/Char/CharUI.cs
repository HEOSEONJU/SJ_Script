using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharUI : MonoBehaviour
{
    
    public CharCardScriptTable CharData;
    public Char_Skill_ScriptTable SkillData;
    public int MYNUM;//자기몇번째인지
    public int CharId;
    public int CharLevel;
    public TextMeshProUGUI Name;
    public Image CharImage;
    public Image CharBGImage;
    public Image Rankimage;

    public Transform TextObject;
    public Transform SkilObject;
    public UIDataUpdate MYUI;
    int MAXLV;

    
    public int Select_Num;
    int need;
    int CurrentLevel_INDEX = 1;
    int Break_Lim = 1;

    public List<Transform> CharPosi;

    [SerializeField]
    List<Image> Skills_Sprite;
    [SerializeField]
    List<TextMeshProUGUI> Skills_Name;
    [SerializeField]
    List<TextMeshProUGUI> Skills_Exp;
    [SerializeField]
    GameObject Window;//확인창
    [SerializeField]
    TextMeshProUGUI NeedGoldText;
    [SerializeField]
    TextMeshProUGUI UP_ATK_TEXT;
    [SerializeField]
    TextMeshProUGUI UP_DEF_TEXT;
    [SerializeField]
    TextMeshProUGUI UP_HP_TEXT;
    FailWinodw PopUp_1;

    public Transform ScrollRect;
    public GameObject Prefab_Object;


    [SerializeField]
    UIDataUpdate Pre;
    [SerializeField]
    Image FadeImage;
    public void Awake()
    {
        PopUp_1 = GetComponent<FailWinodw>();
        Name = transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        CharImage = transform.GetChild(2).GetChild(2).GetComponent<Image>();
        CharBGImage = transform.GetChild(2).GetChild(1).GetComponent<Image>();
        Rankimage = transform.GetChild(2).GetChild(3).GetChild(1).GetComponent<Image>();
        MAXLV = 100;
        Select_Num = 0;

        for(int i=1; i<CharData.Monster.Count; i++)
        {
            var e=Instantiate(Prefab_Object,ScrollRect);
            SmallCharList temp = e.transform.GetComponent<SmallCharList>();
            temp.Setting();
            temp.MYID = i;
            e.transform.GetChild(0).GetComponent<Image>().sprite = CharData.Monster[i].Small_Image;
            e.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(()=> ChangeChar(e.transform.GetComponent<SmallCharList>().MYID));
            e.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = CharData.Monster[i].CardName;
            e.transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(() => PopUp_1.View_Text("해당 캐릭터는 잠금되어있습니다"));
            
        }


    }

    public void OnEnable()
    {
        CloseWindow();
        for(int i=0;i<ScrollRect.childCount;i++)
        {
            ScrollRect.GetChild(i).GetComponent<SmallCharList>().ResetState();
        }
        

    }




    public void OpenWindow()
    {

        StartCoroutine(Open_WindowCorountine());

        ResetText_Need_Gold();


    }
    public void CloseWindow()
    {
        FadeImage.enabled = false;
        Window.transform.localScale = Vector3.zero;

    }
    IEnumerator Open_WindowCorountine()
    {
        FadeImage.enabled = true;
        float time = 0;
        while(time<=1)
        {
            time+=Time.deltaTime*6;
            
            Window.transform.localScale = Vector3.one*time;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    void ResetText_Need_Gold()
    {
        
        int Level = 1;
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.MonsterCards.Count; i++)
        {
            if (FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].ID == CharId)
            {
                Break_Lim= FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].BreaK_Lim;
                CurrentLevel_INDEX = i;
                Level = FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].Level;
                if (Level >= 80)
                {
                    need = 500 * Level;
                }
                else if (Level >= 60)
                {
                    need = 400 * Level;
                }
                else if (Level >= 40)
                {
                    need = 300 * Level;
                }
                else if (Level >= 20)
                {
                    need = 200 * Level;
                }
                else
                {
                    need = 100 * Level;
                }
                break;

                
            }
        }
        
        
        NeedGoldText.text = "" + need;
        CharCardData temp = CharData.Monster[CharId];

        UP_HP_TEXT.text = ""+temp.UpHp;
        UP_ATK_TEXT.text = "" + temp.UpAtk;
        UP_DEF_TEXT.text = "" + temp.UpDef;

    }



    public void CloseCharUI()
    {

        Pre.UpdateData();

        for (int i = 0; i < CharPosi.Count; i++)
        {
            CharPosi[i].GetComponent<CharInfo>().Reset();
        }
        

        gameObject.SetActive(false);    
        

    }

    public void Setting(int ID,int Level,int num)
    {
        CharId = ID;
        
        CharLevel = Level;
        Select_Num = ID;
        MYNUM = num;
        Name.text = "" + CharData.Monster[CharId].CardName;
        CharImage.sprite = CharData.Monster[CharId].Image;
        
        Rankimage.sprite = CharData.Monster[CharId].RankImage;
        CharBGImage.sprite = CharData.Monster[CharId].BGImage;
        TextReset();

    }



    public void LevelUpChar()
    {

        if (need <= FireBaseDB.instacne.Player_Data_instacne.Gold & FireBaseDB.instacne.Player_Data_instacne.MonsterCards[CurrentLevel_INDEX].Level < Break_Lim * 20)
        {



            FireBaseDB.instacne.Player_Data_instacne.Gold -= need;
            FireBaseDB.instacne.Player_Data_instacne.MonsterCards[CurrentLevel_INDEX].Level++;
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
            FireBaseDB.instacne.Upload_Data(StoreTYPE.CHAR);


            MYUI.UpdateData();
            TextReset();
            ResetText_Need_Gold();
        }
        else if(need>= FireBaseDB.instacne.Player_Data_instacne.Gold)
        {
            PopUp_1.View_Text("돈이 모자랍니다.");
            //PopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "돈이 모자랍니다.";
            //Debug.Log("돈이모자랍니다");
            //돈이모자랍니다
        }
        else if(FireBaseDB.instacne.Player_Data_instacne.MonsterCards[CurrentLevel_INDEX].Level >= Break_Lim * 20)
        {
            PopUp_1.View_Text("현재 가능한 최대레벨에 도달햇습니다.");
            //PopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "현재 가능한 최대레벨에 도달햇습니다";
            //Debug.Log("현재 가능한 최대레벨에 도달햇습니다");
            //현재 가능한 최대레벨에 도달햇습니다
        }

    }


    void TextReset()
    {
        CharCardData temp = CharData.Monster[CharId];

        TextObject.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text= temp.CardName;//이름

        
        int LevelTemp =0;
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.MonsterCards.Count; i++)
        {
            if (FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].ID == CharId)
            {
                LevelTemp = FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].Level;
                TextObject.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>().text = "한계돌파 :" + FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].BreaK_Lim;
            }
        }
        TextObject.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = "Level:" + LevelTemp;//레벨
        LevelTemp -= 1;
        TextObject.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>().text = "HP:" + (temp.HP + (temp.UpHp * LevelTemp));//체력
        TextObject.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>().text = "ATK:" + (temp.ATK + (temp.UpAtk * LevelTemp));//공격
        TextObject.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>().text = "DEF" + (temp.DEF + (temp.UpDef * LevelTemp));//방어
        if (temp.AttackType >= 1)
            TextObject.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = "공격방식:물리";
        else
            TextObject.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>().text = "공격방식:마법";


        //스킬불러오기
        for (int i = 0; i <temp.Skill_ID.Count; i++)
        { Skills_Sprite[i].sprite = SkillData.Skill[temp.Skill_ID[i]].Image;
            Skills_Name[i].text = SkillData.Skill[temp.Skill_ID[i]].SkillName;
            Skills_Exp[i].text = SkillData.Skill[temp.Skill_ID[i]].exp;
        }
        





    }

    public void ChangeChar(int num)
    {

        Select_Num = num;
        //Data.MonsterCards[CharId - 1].Level = Data.UseMonsterCards[MYNUM].Level;

        bool ck = false;

        for(int i=0;i< FireBaseDB.instacne.Player_Data_instacne.MonsterCards.Count;i++)
        {
            if(FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].ID == Select_Num)
                ck = true;

        }


        if (ck == true)
        {
            CharId = CharData.Monster[Select_Num].id;

            for(int i=0;i< FireBaseDB.instacne.Player_Data_instacne.MonsterCards.Count;i++)
            {
                if(FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].ID==CharId)
                {
                    CharLevel = FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].Level;
                }
            }

            
            
            Name.text = "" + CharData.Monster[Select_Num].CardName;
            CharImage.sprite = CharData.Monster[Select_Num].Image;
            
            Rankimage.sprite = CharData.Monster[Select_Num].RankImage;
            CharBGImage.sprite = CharData.Monster[Select_Num].BGImage;
            TextReset();
        }
    }

    public void swapOk()
    {
        if (Select_Num != 0)
        {
            bool ck = false;
            for(int i=0;i<4;i++)
            {
                if(FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards[i]== CharData.Monster[Select_Num].id)
                    ck = true;
            }
            if (ck==false)
            {

                FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards[MYNUM] = CharData.Monster[Select_Num].id;
                

                for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.MonsterCards.Count; i++)
                {
                    if (FireBaseDB.instacne.Player_Data_instacne.MonsterCards[i].ID == CharId)
                    {
                        //Data.UseMonsterCards[MYNUM].Level = Data.MonsterCards[i].Level;
                    }
                }
                CharPosi[MYNUM].GetComponent<CharInfo>().Reset();
                FireBaseDB.instacne.Upload_Data(StoreTYPE.CHAR);
            }
            else
            {
                PopUp_1.View_Text("이미 배치되어있는 캐릭터입니다.");
                
            }
        }

    }
}
