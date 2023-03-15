using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

public class EnemyManager : MonoBehaviour
{
    public int ID;
    public EnemyScriptTable Enemy_Data_Table;
    public Manager User_Manager;
    public CharManager Char_Manager;
    Animator animator;
    Enemy_Text EnemyText;
    public string Name;
    public int TYPE;
    public int Base_MaxHP;
    public int Base_ATK;
    public int Base_DEF;
    public int Base_Magic_Regi;

    public int Percent_ATK;
    public int Percent_DEF;
    public int Percent_MAXHP;

    public int Current_ATK;
    public int Current_DEF;
    public int Current_MAXHP;
    public int Current_Magic_Regi;
    public int Current_Hp;

    public int Shield_Point;
    int MagicRegi;
    int AttckCount;
    public bool Live;
    [SerializeField]
    Image Boss_Image;
    [SerializeField]
    Image Boss_BGImage;
    //보스스킬
    public List<Base_E_Skill> SKills ;
    



    [SerializeField]
    List<Transform> Skill_Effects;
    [SerializeField]
    List<Transform> Melee_HIT_Effects;
    [SerializeField]
    List<Transform> Spell_HIT_Effects;
    [SerializeField]
    List<Transform> Shield_Effects;
    [SerializeField]
    List<Transform> Buff_Effect;
    [SerializeField]
    public List<Transform> Skill_HIT_Effect;


    public SpellEffect Temp;
    public Transform Skill_Effect_Parrent;
    //public Transform Card_Effect_Parent;

    public List<SpellEffect> Spell_Effects_List;
    public List<SpellEffect> Skill_Effects_List;
    public Transform Hit_Effect_Parent;
    public Transform Shield_Effect_Parent;
    public Transform Buff_Effect_Parent;
    public Transform Skill_HIT_Effect_Parent;

    public List<TextMeshProUGUI> TextList;
    [SerializeField]
    Transform Contents_Parent;
    public List<int> TargetList;
    bool Loading;

    [SerializeField]
    Image HP_Bar;
    [SerializeField]
    Image SD_Bar;

    public int ShieldCoolDown;
    public int MAX_ShieldCoolDown;
    private void Awake()
    {
        Spell_Effects_List = new List<SpellEffect>();
        Skill_Effects_List = new List<SpellEffect>();
        EnemyText = transform.parent.GetComponent<Enemy_Text>();
        Char_Manager = User_Manager.transform.GetComponent<CharManager>();
        animator = GetComponent<Animator>();
        Melee_HIT_Effects = new List<Transform>();
        Spell_HIT_Effects = new List<Transform>();
        Shield_Effects = new List<Transform>();
        Buff_Effect = new List<Transform>();
        Skill_HIT_Effect = new List<Transform>();
        for (int i = 0; i < Hit_Effect_Parent.GetChild(0).childCount; i++)
        {
            Melee_HIT_Effects.Add(Hit_Effect_Parent.GetChild(0).GetChild(i).GetComponent<Transform>());
            Melee_HIT_Effects[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Hit_Effect_Parent.GetChild(1).childCount; i++)
        {
            Spell_HIT_Effects.Add(Hit_Effect_Parent.GetChild(1).GetChild(i).GetComponent<Transform>());
            Spell_HIT_Effects[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Shield_Effect_Parent.childCount; i++)
        {
            Shield_Effects.Add(Shield_Effect_Parent.GetChild(i).GetComponent<Transform>());
            Shield_Effects[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Buff_Effect_Parent.childCount; i++)
        {
            Buff_Effect.Add(Buff_Effect_Parent.GetChild(i).GetComponent<Transform>());
            Buff_Effect[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Skill_HIT_Effect_Parent.childCount; i++)
        {
            Skill_HIT_Effect.Add(Skill_HIT_Effect_Parent.GetChild(i).GetComponent<Transform>());
            Skill_HIT_Effect[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < Skill_Effect_Parrent.transform.transform.childCount; i++)
        {
            Skill_Effects.Add(Skill_Effect_Parrent.GetChild(i).GetComponent<Transform>());
            Skill_Effects[i].gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        Spell_Effects_List.Clear();
        Skill_Effects_List.Clear();
        Restart();
        Boss_Image.sprite = Enemy_Data_Table.Enemy[ID].Boss_Image;
        if (Enemy_Data_Table.Enemy[ID].Boss_material != null)
        {
            Boss_Image.material = Enemy_Data_Table.Enemy[ID].Boss_material;
        }
        Boss_BGImage.sprite = Enemy_Data_Table.Enemy[ID].Boss_BGImage;
    }

    public void Restart()//재시작함수
    {
        Live = true;
        Name = Enemy_Data_Table.Enemy[ID].Name;
        TYPE = Enemy_Data_Table.Enemy[ID].TYPE;
        Base_MaxHP = Enemy_Data_Table.Enemy[ID].Base_HP;
        Base_ATK = Enemy_Data_Table.Enemy[ID].Base_ATK;
        Base_DEF = Enemy_Data_Table.Enemy[ID].Base_DEF;
        Base_Magic_Regi = Enemy_Data_Table.Enemy[ID].Magic_Regi;
        SKills=new List<Base_E_Skill>();
        
        foreach (GameObject E in Enemy_Data_Table.Enemy[ID].Skills)
        {
            SKills.Add(E.GetComponent<Base_E_Skill>());
        }

        
        
        AttckCount = Enemy_Data_Table.Enemy[ID].AttackCount;
        CalculatorStatus();
        Current_Hp = Current_MAXHP;
        Status();
        Image_buff();//시작
        Loading = false;
        ResetBar();
        Shield_Broken();
        ShieldCoolDown = 0;
        MAX_ShieldCoolDown = 3;
    }
    public void ResetBar()//실드,체력바 갱신
    {
        SD_Bar.fillAmount = 0;
        HP_Bar.fillAmount = Current_Hp * 1.0f / Current_MAXHP; 
        foreach (Base_E_Skill E in SKills)
        {
            if(E.TYPESKILL==SKill_Enum.Shield)
            {
                SD_Bar.fillAmount = Shield_Point * 1.0f / E.Value;
                break;
            }
        }
        

    }
    public void StartEnemyTurn()//적의 턴함수
    {
        CalculatorStatus();
        ResetBar();
        StartCoroutine(EnemyTrunCoroutine());
    }
    public void TurnEnd()//적의 효과의 지속시간 감소
    {
        for (int i = Spell_Effects_List.Count - 1; i >= 0; i--)
        {
            if (Spell_Effects_List[i].UsingTurn())
            {
                Spell_Effects_List.Remove(Spell_Effects_List[i]);
            }
        }
        for (int i = Skill_Effects_List.Count - 1; i >= 0; i--)
        {
            if (Skill_Effects_List[i].UsingTurn())
            {
                Skill_Effects_List.Remove(Skill_Effects_List[i]);
            }
        }
        ResetBar();
        if (ShieldCoolDown >= 1)//실드쿨다운
        {
            ShieldCoolDown--;
        }
        Invoke("Image_buff", 0.1f);//디스트로이가 바로실행안되서 조금늦게해야함
    }
    public void AC_CAl()//공격횟수 재계산
    {
        int MAX = -100;
        int MIN = 100;
        foreach (SpellEffect SE in Spell_Effects_List)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                if (EV.TYPE == BUFF_TYPE.AtC)
                {
                    if (MAX < EV.Value & EV.Value >= 0)
                        MAX = EV.Value;
                    if (MIN > EV.Value & EV.Value < 0)
                    {
                        MIN = EV.Value;
                    }
                }
            }
        }
        foreach (SpellEffect SE in Skill_Effects_List)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                if (EV.TYPE == BUFF_TYPE.AtC)
                {
                    if (MAX < EV.Value & EV.Value >= 0)
                        MAX = EV.Value;
                    if (MIN > EV.Value & EV.Value < 0)
                    {
                        MIN = EV.Value;
                    }
                }
            }
        }
        AttckCount = Enemy_Data_Table.Enemy[ID].AttackCount;
        if (MAX != -100)
            AttckCount += MAX;
        if (MIN != 100)
        { 
            AttckCount += MIN;
        }
    }
    IEnumerator EnemyTrunCoroutine()//적의 턴을 진행하는 코루틴
    {
        //자기턴에 해야할일
        int Ran;
        AC_CAl();//공격횟수갱신
        for (int i = 0; i < AttckCount; i++)
        {
            Loading = true;
            bool c = false;
            if (SearchAttackTarget())
            {

                foreach (Base_E_Skill E in SKills)
                {
                    if (E.TYPESKILL == SKill_Enum.Attack)
                    {

                        Ran = Random.Range(1, 101);
                        if (Ran >= (100 - E.Percentage))
                        {
                            c = Skill_Function(E);
                            break;
                        }
                        break;
                    }
                }


                if (c == false)//스킬발동못해서 기본공격
                {
                    Attack_Fuction();
                }
                while (Loading)
                {
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                yield return new WaitForSeconds(1.0f);
            }
            else
            {
                break;
            }
        }
        EndEnemyTurn();//턴넘겨주기
    }
    public void EndEnemyTurn()//적의 턴을 종료하고 플레이어에게 넘겨주는 함수
    {
        CalculatorStatus();
        if (SearchAttackTarget())
        {
            User_Manager.SwapTurn();
        }
        else
            User_Manager.GameEnd(false);
    }
    public void EndAttack()//공격종료
    {
        Loading = false;
    }
    bool SearchAttackTarget()//공격할 대상 찾기
    {
        foreach(GameCard GC in Char_Manager.CombatChar)
        {
            if (GC.Live)
            {
                return true;
            }
        }
        Debug.Log("때릴수있는 상대방없음");
        EndAttack();
        return false;
    }
    //공격함수
    void Attack_Fuction()//공격함수
    {
        int Target = 0;
        while (true)
        {
            Target = Random.Range(0, Char_Manager.CombatChar.Count);
            if (Char_Manager.CombatChar[Target].Live)
            {
                //Debug.Log("때릴수있는상대확보");
                break;
            }
            //Debug.Log("때릴수있는상대재탐색");
        }
        StartCoroutine(Melee_Attack_Coroutine(Target, Skill_Effects[TYPE]));
    }
    IEnumerator Melee_Attack_Coroutine(int TargetNum, Transform Skill)//근접공격을 하는 함수
    {
        Vector3 Origin = Skill.position;
        Vector3 TargetPosi = Char_Manager.CombatChar[TargetNum].transform.position;
        Skill.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.2f);
        float time = 0;
        float boomTime = 0.25f;//터지는데까지시간
        while (time <= boomTime)
        {
            Skill.transform.position = Vector3.Lerp(Skill.transform.position, TargetPosi, time);
            time += Time.deltaTime;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        Skill.gameObject.SetActive(false);//도착
        Skill.position = Origin;
        Skill_HIT_Effect[TYPE].position = TargetPosi;
        Skill_HIT_Effect[TYPE].gameObject.SetActive(true);
        StartCoroutine(StopEffect(Skill_HIT_Effect[TYPE], 1.0f));
        Char_Manager.CombatChar[TargetNum].Melee_Damaged(Current_ATK);
    }
    bool Skill_Function(Base_E_Skill E)//스킬공격을 하는 함수
    {
        TargetList = new List<int>();
        int r = Random.Range(1, 10);
        switch(E.TYPESKILL)
        {
            case SKill_Enum.Attack:
                while (TargetList.Count == 0)
                {
                    r = Random.Range(0, Char_Manager.CombatChar.Count);
                    if (Char_Manager.CombatChar[r].Live)
                    {
                        TargetList.Add(r);
                        StartCoroutine(E.SkillCoroutine_(this, Shield_Effects[TYPE]));
                        return true;
                    }
                }
                break;
            case SKill_Enum.Shield:
                if (Shield_Point >= 1 | ShieldCoolDown >= 1)
                    return false;
                StartCoroutine(E.SkillCoroutine_(this, Shield_Effects[TYPE]));
                break;
            default:
                EndAttack();
                break;
        }
        return false;
    }
    public IEnumerator StopEffect(Transform effect, float time = 2.0f)
    {
        yield return new WaitForSeconds(time);
        effect.gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        EndAttack();
    }

    public void SkillDamaged(int posi, int Skill_Damage_Value)//해당번호로 스킬데미지적중
    {
        Char_Manager.CombatChar[posi].Spell_Damaged(Skill_Damage_Value);//psoi번호캐릭터에게 스킬데미지주기
    }

    public void Status()//체력바 이름갱신
    {
        TextList[0].text = "HP:" + Current_Hp;
        TextList[1].text = Name;
    }
    public void Image_buff()//걸린 버프/디버프갱신
    {
        //Debug.Log("이미지갱신시작");
        Color color_0 = Color.white;
        color_0.a = 0;
        Color color_1 = Color.white;
        color_1.a = 1;
        int C_Index = 0;
        foreach (SpellEffect SE in Skill_Effects_List)
        {
            Image temp = Contents_Parent.GetChild(C_Index++).GetComponent<Image>();
            temp.color = color_1;
            temp.sprite = CardData.instance.CharSkillFile.Find_Skill(SE.ID).Image;
        }
        foreach (SpellEffect SE in Spell_Effects_List)
        {
            Image temp = Contents_Parent.GetChild(C_Index++).GetComponent<Image>();
            temp.color = color_1;
            temp.sprite = CardData.instance.CardDataFile.Find_Spell_Card(SE.ID).Image;
        }
        for (int i = C_Index; i < Contents_Parent.childCount; i++)
        {
            Contents_Parent.GetChild(i).GetComponent<Image>().color = color_0;
        }
    }

    void CalculatorStatus()//스테이터스를 재계산
    {
        Current_ATK = Base_ATK;
        Current_DEF = Base_DEF;
        Current_MAXHP = Base_MaxHP;
        Current_Magic_Regi = Base_Magic_Regi;
        int TempA = 0;
        int TempD = 0;
        int TempHp = 0;
        Percent_ATK = 0;
        Percent_DEF = 0;
        Percent_MAXHP = 0;
        foreach (SpellEffect SE in Spell_Effects_List)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                switch (EV.TYPE)
                {
                    case BUFF_TYPE.ATK:
                        TempA += EV.Value;
                        break;
                    case BUFF_TYPE.DEF:
                        TempD += EV.Value;
                        break;
                    case BUFF_TYPE.MAXHP:
                        TempHp += EV.Value;
                        break;
                    case BUFF_TYPE.PATK:
                        Percent_ATK += EV.Value;
                        break;
                    case BUFF_TYPE.PDEF:
                        Percent_DEF += EV.Value;
                        break;
                    case BUFF_TYPE.PHP:
                        Percent_MAXHP += EV.Value;
                        break;
                    case BUFF_TYPE.MG:
                        if (Current_Magic_Regi < EV.Value)//가장높은 마법저항력만적용
                        {
                            Current_Magic_Regi = EV.Value;
                        }
                        break;
                    default:
                        break;

                }
            }
        }
        foreach (SpellEffect SE in Skill_Effects_List)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                switch (EV.TYPE)
                {
                    case BUFF_TYPE.ATK:
                        TempA += EV.Value;
                        break;
                    case BUFF_TYPE.DEF:
                        TempD += EV.Value;
                        break;
                    case BUFF_TYPE.MAXHP:
                        TempHp += EV.Value;
                        break;
                    case BUFF_TYPE.PATK:
                        Percent_ATK += EV.Value;
                        break;
                    case BUFF_TYPE.PDEF:
                        Percent_DEF += EV.Value;
                        break;
                    case BUFF_TYPE.PHP:
                        Percent_MAXHP += EV.Value;
                        break;
                    case BUFF_TYPE.MG:
                        if (Current_Magic_Regi < EV.Value)//가장높은 마법저항력만적용
                        {
                            Current_Magic_Regi = EV.Value;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        Current_ATK = (Base_ATK * ((100 + Percent_ATK)) / 100) + TempA;
        Current_DEF = (Base_DEF * ((100 + Percent_DEF)) / 100) + TempD;
        Current_MAXHP = (Base_MaxHP * ((100 + Percent_MAXHP)) / 100) + TempHp;
    }
    //=============================물리공격 캐릭터->에너미
    public void Melee_Attack(int Char_ATK, int CP, int CD, Attack_Effect_Type TYPE)//근접공격
    {
        if (Current_DEF > 0) //방어력음수일때 데미지늘어나는거방지
            Char_ATK -= Current_DEF;
        int CritiaclRandom = Random.Range(1, 101);
        if ((100 - CP) <= CritiaclRandom & CP > 0)
        {
            Char_ATK = (Char_ATK * (100 + CD)) / 100;
            if (Char_ATK > 0)
            {
                //Debug.Log("나온치명타값" + CritiaclRandom);
                //Debug.Log("넘어야하는확률" + (100-CP));
                //Debug.Log("치명타터짐");
                Damaged(Char_ATK, 3);
            }
        }
        else
        {
            if (Char_ATK > 0)
            {
                Damaged(Char_ATK, 1);
            }
        }
        Play_Hit_Animation(TYPE);
    }

    //=============================마법공격 캐릭터->에너미
    public void Magic_Attack(int Char_ATK, Attack_Effect_Type TYPE)
    {
        //Debug.Log("마법데미지"+Char_ATK);
        if (Current_Magic_Regi > 0)
        {
            Damaged(Char_ATK * (100 - Current_Magic_Regi) / 100, 2);
            //Debug.Log("마저 적용후" + Char_ATK * (100 - Current_Magic_Regi) / 100);
        }
        else
        {
            Damaged(Char_ATK, 2);
        }
        Play_Hit_Animation(TYPE, false); //마법히트애니메이션재생
    }
    //=============================스펠공격 플레이어->에너미
    public void Spell_Attack(int Char_ATK, Attack_Effect_Type TYPE)
    {

        if (Current_Magic_Regi > 0)
        {
            Damaged(Char_ATK * (100 - Current_Magic_Regi) / 100, 2);
        }
        else
        {
            Damaged(Char_ATK, 2);
        }
        Play_Hit_Animation(TYPE, false); //마법히트애니메이션재생
    }

    void Damaged(int Char_ATK, int Text_type)//데미지받는함수
    {
        if (Shield_Point > 0)
        {
            EnemyText.Setting(Char_ATK, Text_type);

            if (Char_ATK > Shield_Point)
            {
                Char_ATK -= Shield_Point;
                Shield_Broken();
                Current_Hp -= Char_ATK;
            }
            else if (Char_ATK == Shield_Point)
            {
                Shield_Broken();
            }
            else
            {
                Shield_Point -= Char_ATK;
            }
        }
        else
        {
            Current_Hp -= Char_ATK;
            EnemyText.Setting(Char_ATK, Text_type);
        }
        if (Current_Hp < 0)
        {
            Current_Hp = 0;
        }
    }
    void Shield_Broken()//실드파괴
    {
        Shield_Point = 0;
        Shield_Effects[TYPE].gameObject.SetActive(false);
        ShieldCoolDown = MAX_ShieldCoolDown;
    }

    void DieCheck()//애니메이션쪽에서 사용함
    {
        if (Current_Hp <= 0)
        {
            Live = false;
            User_Manager.GameEnd(true);
        }

    }
    public void Play_HitSpell_Animation(Attack_Effect_Type TYPE)
    {
        /*
        //마법 0 ~10
        //0~2 블루
        //3~5 레드
        //6~9 화이트
        //10 옐로우

        //물리 11~21
        //11 랜덤방향베기
        //12십자베기
        //13~14 아웃터??? 이해안됨

        //15~16 펀치//주먹캐릭터
        //17~18 스크래치//짐승형
        //19~21 베기
        
        switch (TYPE)
        {
            case 0:
                //a = Random.Range(1, 3);
                break;
            case 3:
                //a = Random.Range(3, 6);
                break;
            case 6:
                //a = Random.Range(6, 10);
                break;
            case 15:
                //a = Random.Range(15, 17);
                break;
            case 17:
                //a = Random.Range(17, 19);
                break;
            case 19:
                //a = Random.Range(19, 22);
                break;
            default:
                break;
        }
        */
        if (Shield_Point == 0)
            animator.SetTrigger("HIT");
        //Hit_Effects[a].Play();
        CalculatorStatus();
        ResetBar();
    }

    public void Play_Hit_Animation(Attack_Effect_Type TYPE, bool Melee = true)
    {
        StartCoroutine(Hit_EffectStart(TYPE, Melee));
        if (Shield_Point == 0)
            animator.SetTrigger("HIT");
        CalculatorStatus();
        ResetBar();
    }
    IEnumerator Hit_EffectStart(Attack_Effect_Type TYPE, bool Melee = true)
    {
        if (Melee)
        {
            Melee_HIT_Effects[(int)TYPE].gameObject.SetActive(true);
        }
        else
        {
            Spell_HIT_Effects[(int)TYPE].gameObject.SetActive(true);
        }
        yield return new WaitForSeconds(1.0f);
        if (Melee)
        {
            Melee_HIT_Effects[(int)TYPE].gameObject.SetActive(false);
        }
        else
        {
            Spell_HIT_Effects[(int)TYPE].gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        Skill_Effects_List.Clear();
        Spell_Effects_List.Clear();
        CalculatorStatus();
    }

}

//파기코드

/*
    public void UsingSpell(SpellCard spell)
    {


        if (spell.Turn >= 1)
        {

            //SpellEffect e;
            //e = Instantiate(Temp, Card_Effect_Parent);
            //e.Init(1, spell.CardNumber);
            //e.Apply_Spell_Effect(spell);
            Buff_Effect[spell.Value_Enemy_Effect_Num].gameObject.SetActive(true);
            StartCoroutine(TurnOff_GameObject(Buff_Effect[spell.Value_Enemy_Effect_Num].gameObject, 3.0f));

            if (spell.Value_Enemy_Damage > 0)
            {
                
             //   Magic_Attack(spell.Value_Enemy_Damage, spell.Value_Enemy_Damage_Effect);
            }

        }
        else
        {
            if (spell.Value_Enemy_Damage > 0)
            {
               // Magic_Attack(spell.Value_Enemy_Damage, spell.Value_Enemy_Damage_Effect);
            }
        }
        CalculatorStatus();
        
        ResetBar();
        Image_buff();
    }
    IEnumerator TurnOff_GameObject(GameObject OFF,float time)
    {
        yield return new WaitForSeconds(time);
        OFF.SetActive(false);
    }


    public void UsingSkill(CharSkillData spell)
    {



        SpellEffect e;
        //e = Instantiate(Temp, Card_Effect_Parent);
        //e.Init(0, spell.id);
        //e.Apply_Skill_Effect(spell);
        
        CalculatorStatus();
        ResetBar();
        Image_buff();
    }
    */