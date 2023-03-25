using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Item/Item_List")]
public class Item_List : ScriptableObject
{
    public List<item> Items;


    
    public List<item> Add_Item;
    
    public List<item> Delete_Item;



    public item Search_item(int INDEX)//이진탐색
    {
        
        if (INDEX== 0)
        {
            return null;
        }
        int Low = 0;
        int High = Items.Count - 1;
        int Mid;
        while (Low <= High)
        {
            Mid = (Low + High) / 2;
            if (Items[Mid].Item_Index == INDEX)
            {
                return Items[Mid];
            }
            else if (Items[Mid].Item_Index > INDEX)
            {
                High = Mid - 1;
            }
            else
            {
                Low = Mid + 1;
            }

        }
        return null;
    }

    public void Sort_item()//리스트를 이용한 아이템순서 정렬
    {
        Items = Items.OrderBy(x => x.Item_Index).ToList();
    }


    //찾을때 마다 정령이아닌 리스트에 삽입,삭제 시에만 정렬을 하여 정렬을 최소화
    public void Insert_New_Item(item temp)//리스트에 아이템 추가시 중복인지 확인후 추가후 정렬
    {
        if(Search_item(temp.Item_Index)!=null)
        {
            Debug.Log("이미존재하는아이템");
            return;
        }
        Items.Add(temp);
        
    }
    public void Delete_Old_item(item temp)//리스트에서 삭제시 리스트에 있는지 확인후 삭제
    {
        if (Search_item(temp.Item_Index) == null)
        {
            Debug.Log("리스트에 없는 아이템");
            return;
        }
        Items.Remove(temp);
        

    }
}
