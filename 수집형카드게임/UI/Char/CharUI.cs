using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharUI : MonoBehaviour
{

    //public CharCardScriptTable CharData;
    //public Char_Skill_ScriptTable SkillData;
    public int MYNUM;//�ڱ���°����
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
    GameObject Window;//Ȯ��â
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


    TextMeshProUGUI Break_Text;
    TextMeshProUGUI Level_Text;
    TextMeshProUGUI HP_Text;
    TextMeshProUGUI ATK_Text;
    TextMeshProUGUI DEF_Text;
    TextMeshProUGUI ATK_TYPE_Text;

    public void Awake()
    {
        PopUp_1 = GetComponent<FailWinodw>();
        Name = transform.GetChild(2).GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        CharImage = transform.GetChild(2).GetChild(2).GetComponent<Image>();
        CharBGImage = transform.GetChild(2).GetChild(1).GetComponent<Image>();
        Rankimage = transform.GetChild(2).GetChild(3).GetChild(1).GetComponent<Image>();
        MAXLV = 100;
        Select_Num = 0;

        for (int i = 1; i < CardData.instance.CharDataFile.Monster.Count; i++)
        {
            var e = Instantiate(Prefab_Object, ScrollRect);
            SmallCharList temp = e.transform.GetComponent<SmallCharList>();
            temp.Setting(i);
            e.transform.GetChild(0).GetComponent<Image>().sprite = CardData.instance.CharDataFile.Monster[i].Small_Image;
            e.transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() => ChangeChar(e.transform.GetComponent<SmallCharList>().MYID));
            e.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = CardData.instance.CharDataFile.Monster[i].CardName;
            e.transform.GetChild(2).GetChild(0).GetComponent<Button>().onClick.AddListener(() => PopUp_1.View_Text("�ش� ĳ���ʹ� ��ݵǾ��ֽ��ϴ�"));

        }
        Break_Text = TextObject.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        Level_Text = TextObject.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>();
        HP_Text = TextObject.GetChild(3).GetChild(0).GetComponent<TextMeshProUGUI>();
        ATK_Text = TextObject.GetChild(4).GetChild(0).GetComponent<TextMeshProUGUI>();
        DEF_Text = TextObject.GetChild(5).GetChild(0).GetComponent<TextMeshProUGUI>();
        ATK_TYPE_Text = TextObject.GetChild(6).GetChild(0).GetComponent<TextMeshProUGUI>();


    }

    public void OnEnable()
    {
        CloseWindow();
        for (int i = 0; i < ScrollRect.childCount; i++)
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
        while (time <= 1)
        {
            time += Time.deltaTime * 6;
            Window.transform.localScale = Vector3.one * time;
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }


    void ResetText_Need_Gold()//�䱸 ��差 ����
    {
        int Level = 1;
        MonsterInfo Temp = FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(CharId);
        Break_Lim = Temp.BreaK_Lim;
        Level = Temp.Level;
        CurrentLevel_INDEX = Temp.ID;
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
        NeedGoldText.text = "" + need;
        CharCardData temp = CardData.instance.CharDataFile.Find_Char(CharId);
        UP_HP_TEXT.text = "" + temp.UpHp;
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

    public void Setting(int ID, int Level, int num)//�ʱ����
    {
        CharId = ID;

        CharLevel = Level;
        Select_Num = ID;
        MYNUM = num;
        CharCardData Temp = CardData.instance.CharDataFile.Find_Char(CharId);
        Name.text = "" + Temp.CardName;
        CharImage.sprite = Temp.Image;
        Rankimage.sprite = Temp.RankImage;
        CharBGImage.sprite = Temp.BGImage;
        TextReset();

    }



    public void LevelUpChar()
    {
        MonsterInfo TEMP = FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(CurrentLevel_INDEX);
        if (need <= FireBaseDB.instacne.Player_Data_instacne.Gold & TEMP.Level < Break_Lim * 20)
        {
            FireBaseDB.instacne.Player_Data_instacne.Gold -= need;
            TEMP.Level++;
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
            FireBaseDB.instacne.Upload_Data(StoreTYPE.CHAR);


            MYUI.UpdateData();
            TextReset();
            ResetText_Need_Gold();
        }
        else if (need >= FireBaseDB.instacne.Player_Data_instacne.Gold)
        {
            PopUp_1.View_Text("���� ���ڶ��ϴ�.");
            //PopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "���� ���ڶ��ϴ�.";
            //Debug.Log("���̸��ڶ��ϴ�");
            //���̸��ڶ��ϴ�
        }
        else if (TEMP.Level >= Break_Lim * 20)
        {
            PopUp_1.View_Text("���� ������ �ִ뷹���� �����޽��ϴ�.");
            //PopUp.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "���� ������ �ִ뷹���� �����޽��ϴ�";
            //Debug.Log("���� ������ �ִ뷹���� �����޽��ϴ�");
            //���� ������ �ִ뷹���� �����޽��ϴ�
        }

    }



    


    void TextReset()//���̴� �ؽ�Ʈ ����
    {
        CharCardData temp = CardData.instance.CharDataFile.Find_Char(CharId);

        TextObject.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = temp.CardName;//�̸�


        int LevelTemp = 0;
        MonsterInfo TEMP = FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(CharId);
        Break_Text.text = "�Ѱ赹�� :" + TEMP.BreaK_Lim;
        Level_Text.text = "Level:" + TEMP.Level;//����
        
        
        LevelTemp = TEMP.Level-1;
        HP_Text.text = "HP:" + (temp.HP + (temp.UpHp * LevelTemp));//ü��
        ATK_Text.text = "ATK:" + (temp.ATK + (temp.UpAtk * LevelTemp));//����
        DEF_Text.text = "DEF" + (temp.DEF + (temp.UpDef * LevelTemp));//���
        if (temp.AttackType >= 1)
            ATK_TYPE_Text.text = "���ݹ��:����";
        else
            ATK_TYPE_Text.text = "���ݹ��:����";


        //��ų�ҷ�����
        for (int i = 0; i < temp.Skill_ID.Count; i++)
        {
            CharSkillData TEMP_SKill = CardData.instance.CharSkillFile.Find_Skill(temp.Skill_ID[i]);
            Skills_Sprite[i].sprite = TEMP_SKill.Image;
            Skills_Name[i].text = TEMP_SKill.SkillName;
            Skills_Exp[i].text = TEMP_SKill.exp;
        }






    }

    public void ChangeChar(int num)//���� Ŭ�������� ��ȭ�� ĳ���� ����
    {

        Select_Num = num;
        if (FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(Select_Num) == null)
        {
            return;
        }

        CharCardData TMEP = CardData.instance.CharDataFile.Find_Char(Select_Num);
        CharId = TMEP.id;
        CharLevel = FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(CharId).Level;
        Name.text = "" + TMEP.CardName;
        CharImage.sprite = TMEP.Image;

        Rankimage.sprite = TMEP.RankImage;
        CharBGImage.sprite = TMEP.BGImage;
        TextReset();

    }

    public void swapOk()//ĳ���� ��ġ�ϱ�
    {
        if (Select_Num == 0)//0 ���̵����Ͷ� ����
        {
            return;
        }

        CharCardData TEMP = CardData.instance.CharDataFile.Find_Char(Select_Num);
        foreach(int INDEX in FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards)
        {
            if(INDEX==TEMP.id)
            {
                PopUp_1.View_Text("�̹� ��ġ�Ǿ��ִ� ĳ�����Դϴ�.");
                return;
            }
        }
        FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards[MYNUM] = TEMP.id;
        CharPosi[MYNUM].GetComponent<CharInfo>().Reset();
        FireBaseDB.instacne.Upload_Data(StoreTYPE.CHAR);




    }
}
