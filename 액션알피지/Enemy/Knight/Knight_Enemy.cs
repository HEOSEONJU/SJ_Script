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


    public bool IsInteracting;
    //public bool Attacking;

    public float Move_Speed;
    public void Start()
    {

        init();

        _Knight_AI.Init();
    }

    public EHit_AnimationNumber AttackType;
    public float KnockPower;

    public override bool Damaged(float Damaged_Point,Transform Player, EAttack_Special AC)
    {
        HP -= Damaged_Point;
        if(_Knight_AI.Current_Player==null)
        {
            _Knight_AI.Current_Player= Player;
            Aggro_Time = 15;
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
        Game_Master.instance.PM._playerQuestBox.Check_Monster_Kill(Data.Monster_ID);
        Drop(Random.Range(0, 100));
        
        Knight_Animator.WeaponColliderDisable();
        Knight_Animator._Animator.CrossFade("DeathStart", 0.1f);
        Destroy(gameObject, 5.0f);
        
    }


    public override void Drop(int Per)
    {
        Debug.Log("드랍로그 어떻게 할지 정하기");
        Game_Master.instance.PM._manager_Inventory.Get_Item(Data.Drop.Return_Item(Per));
        Game_Master.instance.PM.Data.Current_Gold+=Data.Drop.Return_Gold();
        Game_Master.instance.PM.Save_On_FireBase();
    }
    public List<int> PlayerObject_ID = new List<int>();

    public void Clear_ID_List()
    {
        PlayerObject_ID.Clear();
    }
    public void Player_to_Damaged(Collider other)
    {
        if (Knight_Animator._Animator.GetBool("Attacking"))
        {
            Player_Manager temp = other.gameObject.transform.root.GetComponent<Player_Manager>();
            if (temp != null)
            {
                foreach (int id in PlayerObject_ID)
                {
                    if (id == temp._objectID)
                    {
                        return;
                    }
                }
                temp.PlayerDamaged(ATK, transform.position - temp.transform.position, AttackType, KnockPower);
                PlayerObject_ID.Add(temp._objectID);


            }
        }



    }


}
