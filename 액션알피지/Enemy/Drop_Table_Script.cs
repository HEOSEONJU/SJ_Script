using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item/Drop_Table")]
public class Drop_Table_Script : ScriptableObject
{
    
    public Drop_Table_List Drop_Talbe;
    public int Gold;

    public int Return_Gold() { return Gold; }
    public Item_Data Return_Item(int Per)
    {
        float Current_Remain = Drop_Talbe.Cal_remain();
        if (Per < Current_Remain || Drop_Talbe.Table_List.Count==0)
        {
            return null;
        }

        Item_Data TEMP= new Item_Data();
        for (int i = 0; Drop_Talbe.Table_List.Count - 1 > i; i++)
        {
            Current_Remain += Drop_Talbe.Table_List[i].percentage;
            if (Per <= Current_Remain)
            {
                TEMP.Insert_Data(Game_Master.instance.ILO.Search_item(Drop_Talbe.Table_List[i].Drop_Item_INDEX),1);
                
                return TEMP;
            }

        }

        TEMP.Insert_Data(Game_Master.instance.ILO.Search_item(Drop_Talbe.Table_List[Drop_Talbe.Table_List.Count - 1].Drop_Item_INDEX),1);
        
        return TEMP;
        


    }

}

[System.Serializable]
public class Drop_Table_List

{
    [System.Serializable]
    public class Drop_Table_class
    {
        public int Drop_Item_INDEX;        
        public int percentage;

    }

    public List<Drop_Table_class> Table_List;
    

    public int  Cal_remain()
    {
        int remain_percentage = 100;
        foreach (var item in Table_List) 
        {
            remain_percentage -= item.percentage;
        }
        return remain_percentage;
    }


}