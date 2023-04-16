
using UnityEngine;
using Item_Enum;
[System.Serializable]
public class Quest_Give : Quest_Basic
{

    public int Item_ID;
    [SerializeField]
    int Item_Count;



    public override float Progress_float(int point)
    {
        return ((point * 1f) / Item_Count) * 100f;
    }
    public override bool Complete_Check(int point = 0)
    {
        //Debug.Log(Current_Hunt + "/" + Hunt_Count);
        if (point == Item_Count)
        {
            return true;
        }

        return false;
    }

    public override bool Give_Check(int Item_ID)
    {
        
        switch( Game_Master.instance.ILO.Search_item(Item_ID).Type)
        {
            case EItem_Slot_Type.ETC:
                break;
            case EItem_Slot_Type.Use:
                int Max_count = 0;
                foreach (Item_Data ID in Game_Master.instance.PM.Data.Items.FindAll(x => x.INDEX == Item_ID))
                {
                    while(ID.count>0)
                    {
                        ID.UseItem();
                        Max_count++;
                        if (Max_count >= Item_Count)
                        {
                            Item_Count = Max_count;
                            return true;
                        }
                    }
                }
                

                break;
            default:
                int INDEX = Game_Master.instance.PM.Data.Items.FindIndex(x => x.INDEX == Item_ID);
                if (INDEX != -1)
                {
                    Game_Master.instance.PM.Data.Items[INDEX].Delete_Data();
                    Item_Count = 1;

                    return true;
                }
                break;


        }
        return false;
    }

    
}
