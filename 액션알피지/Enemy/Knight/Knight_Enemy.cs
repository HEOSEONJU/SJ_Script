using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hit_Type_NameSpace;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;
using static UnityEngine.UI.CanvasScaler;

public class Knight_Enemy : Enemy_Base_Status
{

    
        
    public Knight_Animation Knight_Animator;
    public Knight_AI _Knight_AI;
    [SerializeField]
    Drop_Table_Script DTS;

    public bool IsInteracting;
    public bool Attacking;

    public float Move_Speed;
    public void Start()
    {

        init();
        Knight_Animator.Init();
        _Knight_AI.Init();
    }

    public Hit_AnimationNumber AttackType;
    public float KnockPower;

    public override bool Damaged(float Damaged_Point,Transform Player, int Type = 0)
    {
        HP -= Damaged_Point;
        if(_Knight_AI.Current_Player==null)
        {
            _Knight_AI.Current_Player= Player;
        }
        if (HP <= 0)
        {
            Live = false;
            Died();
            return true;
        }
        return false;
    }

    public override void Died()
    {
        //아이템드랍코드
        
        Drop(Random.Range(0, 100));
        Knight_Animator.WeaponColliderDisable();
        Knight_Animator._Animator.CrossFade("DeathStart", 0.1f);
        Destroy(gameObject, 5.0f);
        
    }


    public override void Drop(int Per)
    {
        Debug.Log("드랍로그 어떻게 할지 정하기");
        Game_Master.instance.Call_Player()._Manager_Inventory.Get_Item(Data.Drop.Return_Item(Per));
        Game_Master.instance.Call_Player()._Manager_Inventory.Get_Money(Data.Drop.Return_Gold());
        Game_Master.instance.Call_Player().Save_On_FireBase();
    }
    public List<int> PlayerObject_ID = new List<int>();

    public void Clear_ID_List()
    {
        PlayerObject_ID.Clear();
    }
    public void Player_to_Damaged(Collider other)
    {

        if (Attacking)
        {
            Player_Manager temp = other.GetComponent<Player_Manager>();
            if (temp != null)
            {
                foreach (int id in PlayerObject_ID)
                {
                    if (id == temp.ObjectID)
                    {
                        return;
                    }
                }



                temp.PlayerDamaged(100, transform.position - temp.transform.position, AttackType, KnockPower);
                PlayerObject_ID.Add(temp.ObjectID);


            }
        }



    }


}
