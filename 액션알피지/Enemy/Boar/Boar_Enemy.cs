using Hit_Type_NameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar_Enemy : Enemy_Base_Status
{



    public Boar_Animation Boar_Animator;
    public Boar_AI _Boar_AI;
    

    public bool IsInteracting;
    //public bool Attacking;

    public float Move_Speed;
    public void Start()
    {

        init();
        Boar_Animator.Init();
        _Boar_AI.Init();
    }

    public Hit_AnimationNumber AttackType;
    public float KnockPower;

    public override bool Damaged(float Damaged_Point, Transform Player, int Type = 0)
    {
        HP -= Damaged_Point;
        if (_Boar_AI.Current_Player == null)
        {
            _Boar_AI.Current_Player = Player;
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
        Game_Master.instance.PM.PQB.Check_Monster_Kill(Data.Monster_ID);
        Drop(Random.Range(0, 100));

        Boar_Animator.WeaponColliderDisable();
        Boar_Animator._Animator.CrossFade("DeathStart", 0.1f);
        Destroy(gameObject, 5.0f);

    }


    public override void Drop(int Per)
    {
        Debug.Log("드랍로그 어떻게 할지 정하기");
        Game_Master.instance.PM._Manager_Inventory.Get_Item(Data.Drop.Return_Item(Per));
        Game_Master.instance.PM.Data.Current_Gold += Data.Drop.Return_Gold();
        Game_Master.instance.PM.Save_On_FireBase();
    }
    public List<int> PlayerObject_ID = new List<int>();

    public void Clear_ID_List()
    {
        PlayerObject_ID.Clear();
    }
    public void Player_to_Damaged(Collider other)
    {
        
        if (Boar_Animator._Animator.GetBool("Attacking"))
        {
            Player_Manager temp = other.gameObject.transform.root.GetComponent<Player_Manager>();
            if (temp != null)
            {
                foreach (int id in PlayerObject_ID)
                {
                    if (id == temp.ObjectID)
                    {
                        return;
                    }
                }
                temp.PlayerDamaged(ATK, transform.position - temp.transform.position, AttackType, KnockPower);
                PlayerObject_ID.Add(temp.ObjectID);


            }
        }

        

    }


}
