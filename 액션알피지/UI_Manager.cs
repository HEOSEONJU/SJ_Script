using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;

public class UI_Manager : MonoBehaviour
{

    Vector3 Mouse_Position;
    public bool Windows
    {
        get { return Windows_Object.gameObject.activeSelf; }
    }

    public bool Draging;
    [SerializeField]
    Transform Windows_Object;
    [SerializeField]
    UI_Information INFO;

    public TextMeshProUGUI Status_Text_Level;
    public TextMeshProUGUI Status_Text_ATK;
    public TextMeshProUGUI Status_Text_DEF;
    public TextMeshProUGUI Status_Text_HP;
    public TextMeshProUGUI Status_Text_CRP;
    public TextMeshProUGUI Status_Text_CRT;
    public TextMeshProUGUI Gold_Text;


    [SerializeField]
    CanvasGroup Canvas_Group;

    
     LinkedList<GameObject> Open_Object;
    public int Count_Open_UI
    {
        get { return Open_Object.Count; }
    }


    public void Init()
    {

        DontDestroyOnLoad(this.gameObject);
        

        Inventory_Slot[] temp = GetComponentsInChildren<Inventory_Slot>();
        int INDEX = 0;
        foreach (Inventory_Slot slot in temp)
        {
            slot.Setting_Slot( INDEX++);
        }
        Windows_Object.gameObject.SetActive(false);
        Open_Object=new LinkedList<GameObject>();
    }

    public bool Use_Heal_item(Use_Item temp)
    {
        return Game_Master.instance.PM._Status.Heal(temp.Point);
        
    }
    public void View_Hp_text()
    {
        Status_Text_HP.text = Game_Master.instance.PM._Status.Current_HP.ToString() + "/" + Game_Master.instance.PM._Status.MAX_HP.ToString();
    }

    public void Reseting_Status()
    {
        Game_Master.instance.PM._Status.Cal_All();
        Status_Text_Level.text = Game_Master.instance.PM._Status.Level.ToString();
        Status_Text_ATK.text = Game_Master.instance.PM._Status.ATK_Point.ToString();
        Status_Text_DEF.text = Game_Master.instance.PM._Status.DEF_Point.ToString();
        View_Hp_text();
        Status_Text_CRP.text = Game_Master.instance.PM._Status.CRP_Point.ToString() + "%";
        Status_Text_CRT.text = Game_Master.instance.PM._Status.CRT_Point.ToString() + "%";

    }
    public void Open_UI(GameObject GO)
    {
        
        if (GO.activeSelf == false)
        {
            GO.SetActive(true);
            Open_Object.AddLast(GO);
        }
    }
    public void Close_UI(GameObject GO)
    {
        
        if (GO.activeSelf == true)
        {
            GO.SetActive(false);
            Open_Object.Remove(GO);
        }
    }

    public void Close_All_UI()
    {
        
        while (Open_Object.Count>0)
        {
            
            Open_Object.First().SetActive(false);
            Open_Object.Remove(Open_Object.First());

        }
    }

    public void Open_Small_Windows(Item_Data Item_temp = null)//확대창
    {
        
        if (Item_temp == null)
        {
            return;
        }
        if (Item_temp.Base_item != null)
            Renewal_Text(Item_temp);
        Open_UI(Windows_Object.gameObject);
    }
    public void Close_Small_Windows()//확대창
    {
        Close_UI(Windows_Object.gameObject);
    }
    public void Renewal_Text(Item_Data Item_temp)
    {

        INFO.Name.text = Item_temp.Base_item.Item_Name;

        switch (Item_temp.Base_item.Type)
        {
            case Type.Use:
                INFO.Upgarde.text = "";
                INFO.Type.text = "Use";
                Use_Item temp_U = (Use_Item)Item_temp.Base_item;
                INFO.Header[0].text = "회복양 :";
                INFO.Point[0].text = temp_U.Point.ToString();
                INFO.Header[1].text = "개수:";
                INFO.Point[1].text = Item_temp.count.ToString();
                for (int i = 2; i < INFO.Header.Count; i++)
                {
                    INFO.Header[i].text = "";
                    INFO.Point[i].text = "";
                }
                break;

            case Type.Equip:

                Equip_Item EI = (Equip_Item)Item_temp.Base_item;
                switch (EI.EST)
                {

                    case Equip_Slot_Type.Weapon:
                        INFO.Type.text = "무기";
                        Weapon_Item temp_W = (Weapon_Item)Item_temp.Base_item;
                        INFO.Upgarde.text = "+" + Item_temp.Upgrade;
                        INFO.Header[0].text = "공격력 :";
                        INFO.Point[0].text = (temp_W.Attack_Point + (temp_W.UP_Attack_Point * Item_temp.Upgrade)).ToString();
                        INFO.Header[1].text = "치명타확률 :";
                        INFO.Point[1].text = (temp_W.CRP + (temp_W.UP_CRP * Item_temp.Upgrade)).ToString() + "%";
                        INFO.Header[2].text = "치명타데미지 :";
                        INFO.Point[2].text = (temp_W.CRT + (temp_W.UP_CRT * Item_temp.Upgrade)).ToString() + "%";
                        for (int i = 3; i < INFO.Header.Count; i++)
                        {
                            INFO.Header[i].text = "";
                            INFO.Point[i].text = "";
                        }

                        break;
                    default:
                        Armor_Item temp_A = (Armor_Item)Item_temp.Base_item;
                        switch (temp_A.EST)
                        {
                            case Equip_Slot_Type.Breastplate:
                                INFO.Type.text = "갑옷";
                                break;
                            case Equip_Slot_Type.Glove:
                                INFO.Type.text = "장갑";
                                break;
                            case Equip_Slot_Type.Helmet:
                                INFO.Type.text = "투구";
                                break;

                        }
                        INFO.Upgarde.text = "+" + Item_temp.Upgrade;
                        INFO.Header[0].text = "공격력 :";
                        INFO.Point[0].text = (temp_A.Attack_Point + (temp_A.UP_Attack_Point * Item_temp.Upgrade)).ToString();
                        INFO.Header[1].text = "방어력 :";
                        INFO.Point[1].text = (temp_A.Armor_Point + (temp_A.UP_Armor_Point * Item_temp.Upgrade)).ToString();
                        INFO.Header[2].text = "체력  :";
                        INFO.Point[2].text = (temp_A.HP_Point + (temp_A.UP_HP_Point * Item_temp.Upgrade)).ToString();
                        for (int i = 3; i < INFO.Header.Count; i++)
                        {
                            INFO.Header[i].text = "";
                            INFO.Point[i].text = "";
                        }
                        break;
                }
                break;
        }

    }

    public void Renewal_Gold_Text()
    {
        Gold_Text.text = Game_Master.instance.PM.Data.Current_Gold.ToString();
    }



}
