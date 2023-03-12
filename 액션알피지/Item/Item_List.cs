using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


[CreateAssetMenu(menuName = "Item/Item_List")]
public class Item_List : ScriptableObject
{
    public List<item> Items;



    public item Search_item(int INDEX)//����Ž��
    {
        if(INDEX== 0)
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

    public void Sort_item()//����Ʈ�� �̿��� �����ۼ��� ����
    {
        Items = Items.OrderBy(x => x.Item_Index).ToList();
    }


    //ã���� ���� �����̾ƴ� ����Ʈ�� ����,���� �ÿ��� ������ �Ͽ� ������ �ּ�ȭ
    public void Insert_New_Item(item temp)//����Ʈ�� ������ �߰��� �ߺ����� Ȯ���� �߰��� ����
    {
        if(Search_item(temp.Item_Index)!=null)
        {
            Debug.Log("�̹������ϴ¾�����");
            return;
        }
        Items.Add(temp);
        Sort_item();
    }
    public void Delete_Old_item(item temp)//����Ʈ���� ������ ����Ʈ�� �ִ��� Ȯ���� ����
    {
        if (Search_item(temp.Item_Index) == null)
        {
            Debug.Log("����Ʈ�� ���� ������");
            return;
        }
        Items.Remove(temp);
        Sort_item();

    }
}
