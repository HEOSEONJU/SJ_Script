using Item_Enum;
using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Item_Data
{
    
    public item Base_item;
    
    public int count;
    
    public int Upgrade;
    
    
    public int INDEX;

    public void Insert_Data(item Temp,int NUM=0)
    {
        if(Temp!= null) 
        {
            if(Temp.Type== EItem_Slot_Type.Use)
            {
                count = NUM;
            }
            Base_item = Temp;
            INDEX = Temp.Item_Index;

        }
        else
        {
            Base_item = null;
            INDEX = 0;
        }
        
    }
    public void Delete_Data()
    {
        Base_item = null;
        INDEX = 0;
        Upgrade = 0;
        count = 0;
    }

    public void Insert_Data(Item_Data Temp)
    {
        if (Temp != null)
        {
            Base_item = Temp.Base_item;
            INDEX = Temp.Base_item.Item_Index;
            Upgrade= Temp.Upgrade;
            count = Temp.count;

        }
        else
        {
            
            INDEX = 0;
            Upgrade = 0;
            count =0;
        }

    }

    public bool UseItem(int C=1)
    {

        if (count >= C)
        {
            count-=C;
            if(count == 0)
            {
                Delete_Data();

            }
            return true;
        }
        else
        {
            return false;
        }
    }
    public int Merge_count()
    {
        int temp = count;
        count = 0;
        return temp;
    }

    public void Upgrade_Value()
    {
        Upgrade += 1;
    }
}
