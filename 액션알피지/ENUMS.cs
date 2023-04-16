using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ENUMS : MonoBehaviour
{

}




namespace Quests
{
    
    public enum EQuest_Type//����Ʈ Ÿ�� 
    {
        Hunt,//�������Ʈ
        Talk,//��ȭ������Ʈ
        Give    //��������Ʈ
    }


    [System.Serializable]
    public struct Quest_Info
    {
        public int INDEX;
        public int Progress;
        public int Point;
    }

    [System.Serializable]
    public struct Quest_Set
    {
        public int Quset_ID;
        public EQuest_Type TYPE;

    }
}
namespace Item_Enum
{
    public enum EItem_Slot_Type//�������� ���� �� �ִ� ����ĭ?
    {
        Weapon=1,
        Helmet=2,
        Breastplate=4,
        Glove=8,
        Use=16,
        ETC=32
    }

}
namespace Hit_Type_NameSpace
{
    public enum EHit_AnimationNumber
    {
        Weak, Nomal, Hard
    }
    public enum EAttack_Special
    {
        Ground,Air
    }
}
namespace Enemy
{
    public enum EMonster_Size
    {
        S_Size,
        M_Size,
        B_Size
    }
}
