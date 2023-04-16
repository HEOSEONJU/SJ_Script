using Hit_Type_NameSpace;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class Boar_Enemy : Enemy_Base_Status
{



    public Boar_Animation _boar_Animator;
    public Boar_AI _boar_AI;
    

    public bool IsInteracting;
    //public bool Attacking;

    public float _move_Speed;

    [SerializeField]
    Collider _collider;
    
    public Rigidbody _RD;

    public LayerMask Layer;
    public void Start()
    {

        init();
        _boar_Animator.Init();
        _boar_AI.Init();

    }

    public EHit_AnimationNumber AttackType;
    public float KnockPower;

    public override bool Damaged(float Damaged_Point, Transform Player, EAttack_Special AC)
    {
        
        if (AC == EAttack_Special.Air)
        {
            _boar_Animator._Animator.SetTrigger("Air_Hit");
        }
        else
        {
            _boar_Animator._Animator.SetTrigger("Hit");
        }
        
        HP -= Damaged_Point;
        if (_boar_AI.Current_Player == null)
        {
            _boar_AI.Current_Player = Player;
            Aggro_Time = 15;
        }
        if (HP <= 0)
        {
            
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
        Live = false;
        StartCoroutine(DIe());
        

    }
    IEnumerator DIe()
    {

        Ray ray = new Ray();
        ray.direction = Vector3.down;
        
        RaycastHit[] hits;

        while(true)
        {
            ray.origin = transform.position + new Vector3(0, 0.5f, 0);
            hits = Physics.RaycastAll(ray, 0.7f, Layer);
            //Debug.Log(hits.Length);
            if (hits.Length > 0) 
            {
                Debug.Log("탈출");
                break; 
            }
            yield return null;

        }

        _collider.isTrigger = true;
        _RD.useGravity = false;
        _RD.velocity = Vector3.zero;


        _boar_Animator.WeaponColliderDisable();
        _boar_Animator._Animator.CrossFade("DeathStart", 0.1f);
        Destroy(gameObject, 5.0f);

    }

    public override void Drop(int Per)
    {
        Debug.Log("드랍로그 어떻게 할지 정하기");
        Game_Master.instance.PM._manager_Inventory.Get_Item(Data.Drop.Return_Item(Per));
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
