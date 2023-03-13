using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEditor.Progress;

public class GameCard : MonoBehaviour
{
    [Header("게임참조")]
    //public CharCardScriptTable CardDataBase;
    //public Char_Skill_ScriptTable SkillDataBase;
    public CharManager Char_manager;

    public EnemyManager Enemy_Manager;

    [Header("카드정보")]
    public int ID;
    public bool Live;


    public Image BG;
    public Image MainImage;


    public string Name;
    public int Level;
    public int Atk;
    public int UPAtk;
    public int Def;
    public int UPDef;
    public int Hp;
    public int UPHp;


    public List<int> Skill_ID;
    public int Type;
    public int AttackType;
    public List<int> AttackEffect;
    public int AttackCount = 1;


    public int Current_ATK;
    public int Current_DEF;
    public int Current_HP;
    public int Current_MAXHP;

    public int Effect_Magic_Regi;

    public int Percent_ATK;
    public int Percent_DEF;
    public int Percent_MAXHP;

    public int Effect_ATK;
    public int Effect_DEF;
    public int Effect_HP;
    public int Effect_CP;
    public int Effect_CD;



    public SpellEffect Temp;
    
    public List<SpellEffect> Spell_Effects;
    public List<SpellEffect> Skill_Effects;

    int Based_CriticalDamage;
    int Based_CriticalPercent;

    public int Current_CriticalDamage;
    public int Current_CriticalPercent;


    public Animator animator;
    public List<Animator> Text_animator;

    public List<TextMeshProUGUI> text_mesh;
    int num;

    Vector3 OriginiPosi;

    [SerializeField]
    List<Material> Shine;


    [SerializeField]
    Transform Buff_Effect_Parnet;
    [SerializeField]
    List<Transform> Buff_Effect;


    [SerializeField]
    Transform Hit_MY_Spell_Parent;
    [SerializeField]
    List<Transform> Hit_MY_Spell;
    [SerializeField]
    Image HP_BAR;

    enum Text_Array { Damaged_T, HEAL_T, ATK_T, DEF_T, ATK_Count_T, DamaedSpell_T, CP_T, CD_T };
    private void Awake()
    {
        animator = GetComponent<Animator>();
        OriginiPosi = transform.position;
        Buff_Effect = new List<Transform>();
        Hit_MY_Spell = new List<Transform>();
        for (int i= 0; i < Buff_Effect_Parnet.childCount; i++)
        {
            Buff_Effect.Add(Buff_Effect_Parnet.GetChild(i).transform);
        }
        for (int i = 0; i < Hit_MY_Spell_Parent.childCount; i++)
        {
            Hit_MY_Spell.Add(Hit_MY_Spell_Parent.GetChild(i).transform);
        }   
    }
    public void InputID(int INDEX) //INDEX를받고 생성
    {
        Spell_Effects = new List<SpellEffect>();
        Skill_Effects = new List<SpellEffect>();
        Skill_ID = new List<int>();

        MainImage.color = Color.white;
        ID = INDEX;
        Live = true;
        CharCardData Temp = CardData.instance.CharDataFile.Find_Char(INDEX);
        ID = Temp.id;
        Name = Temp.CardName;
        Level = FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(ID).Level;
        Atk = Temp.ATK;
        UPAtk = Temp.UpAtk;
        Def = Temp.DEF;
        UPDef = Temp.UpDef;
        Hp = Temp.HP;
        UPHp = Temp.UpHp;
        AttackType = Temp.AttackType;
        AttackEffect = Temp.AttackEffectNum;
        Based_CriticalPercent = Temp.CritiaclPercent;
        Based_CriticalDamage = Temp.CritiaclDamage;
        Setting_Skill_Text();
        BG.sprite = Temp.BGImage;
        MainImage.sprite = Temp.Image;
        switch (Temp.Rank)
        {
            case 3:
                MainImage.material = Shine[0];
                break;
            case 2:
                MainImage.material = Shine[1];
                break;
            case 1:
                MainImage.material = Shine[2];
                break;
            default:
                break;
        }
        Effect_ATK = 0;
        Effect_DEF = 0;
        Effect_HP = 0;
        CalculatorStatus();
        Current_HP = Current_MAXHP;
        num = 0;
        CalculatorStatus();
    }
    public void CalculatorStatus()//스테이터스 계산
    {
        Effect_ATK = Effect_DEF = Effect_HP = Effect_Magic_Regi = Effect_CD = Effect_CP = Percent_ATK = Percent_DEF = Percent_MAXHP = 0;

        foreach(SpellEffect SE in Spell_Effects )
        {
            foreach(Effect_Value EV in SE.Effect_Type_Value)
            {
                switch (EV.TYPE)
                {
                    case BUFF_TYPE.ATK:
                        Effect_ATK += EV.Value;
                        break;
                    case BUFF_TYPE.DEF:
                        Effect_DEF += EV.Value;
                        break;
                    case BUFF_TYPE.MAXHP:
                        Effect_HP += EV.Value;
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
                    case BUFF_TYPE.CP:
                        Effect_CP += EV.Value;
                        break;
                    case BUFF_TYPE.CD:
                        Effect_CD += EV.Value;
                        break;
                    case BUFF_TYPE.MG:
                        if (Effect_Magic_Regi < EV.Value)//가장높은 마법저항력만적용
                        {
                            Effect_Magic_Regi = EV.Value;
                        }
                        break;
                    default:
                        break;
                }
            }   
        }
        Current_ATK = (int)(Mathf.Floor((Atk + (UPAtk * Level)) * ((100f + Percent_ATK) / 100))) + Effect_ATK;
        Current_DEF = (int)(Mathf.Floor((Def + (UPDef * Level)) * ((100f + Percent_DEF) / 100)))+ Effect_DEF;
        Current_MAXHP = (int)(Mathf.Floor((Hp + (UPHp * Level)) * ((100f + Percent_MAXHP) / 100))) + Effect_HP;
        Current_CriticalDamage = Based_CriticalDamage + Effect_CD;
        Current_CriticalPercent = Based_CriticalPercent + Effect_CP;
        if (Current_HP > Current_MAXHP)
        {
            Current_HP = Current_MAXHP;
        }
    }

    private void Update_Bar()//체력바갱신
    {
        HP_BAR.fillAmount = Current_HP*1.0f / Current_MAXHP;
    }
    
    void Setting_Skill_Text()//스킬 ID값설정하기
    {
        for(int i=0;i< CardDataBase.Monster[ID].Skill_ID.Count;i++)
        {
            Skill_ID.Add(CardDataBase.Monster[ID].Skill_ID[i]);   
        }
    }


    public void Melee_Damaged(int Enemy_Attack)//물리데미지 방어력에 영향받는 데미지 받는 함수
    {
        //Debug.Log("들어올피해량" + Enemy_Attack);
        //Debug.Log("현재방어력" + Current_DEF);
        if (Current_DEF > 0)
            Enemy_Attack -= Current_DEF;
        animator.SetTrigger("HIT");
        //Debug.Log("들어올피해량"+Enemy_Attack);
        if (Enemy_Attack > 0)
        {
            //Debug.Log(Enemy_Attack+"만큼데미지입음");
            Use_Text(Text_Array.Damaged_T, Enemy_Attack);
            Current_HP -= Enemy_Attack;
            Update_Bar();
        }
        else
        {
            Use_Text(Text_Array.Damaged_T, 0);
        }
    }
    public void Spell_Damaged(int Enemy_Attack,int TYPE=50)///스펠데미지 마법방어력에 영향받는 데미지 받는 함수
    {
        animator.SetTrigger("HIT");
        if(TYPE!=50)
        {
            Hit_MY_Spell[TYPE].gameObject.SetActive(true);
            StartCoroutine(TurnOff_GameObject(Hit_MY_Spell[TYPE].gameObject,2.0f));
        }

        if (Effect_Magic_Regi > 0)
        {
            Enemy_Attack = Enemy_Attack*(100 - Effect_Magic_Regi) / 100;
        }
        Use_Text(Text_Array.DamaedSpell_T, Enemy_Attack);
        if (Enemy_Attack > 0)
        {
            Current_HP -= Enemy_Attack;
        }
        else
        {
            Use_Text(Text_Array.DamaedSpell_T, 0);
        }
        Update_Bar();
    }
    public void DieCheck()//죽었는지 체크 히트애니메이션 종료 후 자동 호출
    {
        //Debug.Log("죽음체크");
        if (Current_HP <= 0)
        {
            Live = false;
            Current_HP = 0;
            AttackCount = 0;
            Update_Bar();
            MainImage.color = Color.gray;
            MainImage.material = null;
            int L = 0;
            for(int i=0;i<Char_manager.CombatChar.Count;i++)
            {
                if (Char_manager.CombatChar[i].Live)
                    L++;
            }
            if(L==0)
            {
                Enemy_Manager.User_Manager.GameEnd(false);
            }
        }
    }

    public void Heal(int Heal_Value)//회복 기능
    {
        if (Current_MAXHP <= (Current_HP + Heal_Value))//최대치를넘는경우 넘지않는한에대해서 회복
        {
            Current_HP = Current_MAXHP;
        }
        else
        {
            Current_HP += Heal_Value;
        }
        Update_Bar();//체력바갱신함수 회복이나 최대체력오를때 사용
        Active_Effect(10);
    }
    public void Active_Effect(int INDEX)//인덱스에 맞는 이펙트를 3초간활성화
    {
        Buff_Effect[INDEX].gameObject.SetActive(true);
        StartCoroutine(TurnOff_GameObject(Buff_Effect[INDEX].gameObject, 3.0f));
    }
    
    void Attack()//어택애니메이션작동시 작동
    {
        AttackCount -= 1;
        //int num = Random.Range(0, AttackEffect.Count);
        if (AttackType >= 1)
            Enemy_Manager.Melee_Attack(Current_ATK, Current_CriticalPercent, Current_CriticalDamage, Attack_Effect_Type.Fire);
        else
            Enemy_Manager.Magic_Attack(Current_ATK, Attack_Effect_Type.Fire);
    }


    public void ResetAttackCount()//공격횟수 계산
    {
        int MAX = 1;
        int MIN = 0;
        foreach (SpellEffect SE in Spell_Effects)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                if (EV.TYPE == BUFF_TYPE.AtC)
                {
                    if (MAX < EV.Value)
                    {

                        MAX = EV.Value;
                    }
                    if (MIN > EV.Value & EV.Value < 0)
                    {
                        MIN = EV.Value;
                    }
                }
            }
        }
        foreach (SpellEffect SE in Skill_Effects)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                if (EV.TYPE == BUFF_TYPE.AtC)
                {
                    if (MAX < EV.Value)
                    {

                        MAX = EV.Value;
                    }
                    if (MIN > EV.Value & EV.Value < 0)
                    {
                        MIN = EV.Value;
                    }

                }

            }
        }
        AttackCount = 1;
        if (MAX>1)
            AttackCount = MAX;
        if (MIN < 0)
        {
            AttackCount -= MIN;
        }
        
    }
    public void TURNUSE()//적용된 스킬,스펠 지속시간 소모
    {
        for (int i= Spell_Effects.Count-1;i>=0;i--)
        {

            if (Spell_Effects[i].UsingTurn())
            {
                Spell_Effects.Remove(Spell_Effects[i]);
            }
        }
        for (int i = Skill_Effects.Count - 1; i >= 0; i--)
        {

            if (Skill_Effects[i].UsingTurn())
            {
                Skill_Effects.Remove(Skill_Effects[i]);
            }
        }
        CalculatorStatus();
    }

    public int MAX_DREW_Count()//추가드로우횟수 계산
    {
        int count = 1;
        foreach (SpellEffect SE in Spell_Effects)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                if (EV.TYPE == BUFF_TYPE.DC)
                {
                    if (count < EV.Value)
                    {
                        count = EV.Value;
                    }
                }
            }
        }
        foreach (SpellEffect SE in Skill_Effects)
        {
            foreach (Effect_Value EV in SE.Effect_Type_Value)
            {
                if (EV.TYPE == BUFF_TYPE.DC)
                {
                    if (count < EV.Value)
                    {
                        count = EV.Value;
                    }
                }
            }
        }
        return count;
    }


    public void REGEN_Active()//체력재생효과
    {
        if (Live)
        {
            int REGEN = 0;
            foreach (SpellEffect SE in Spell_Effects)
            {
                foreach (Effect_Value EV in SE.Effect_Type_Value)
                {
                    if (EV.TYPE == BUFF_TYPE.Regen)
                    {
                        REGEN += EV.Value;
                    }
                }
            }
            foreach (SpellEffect SE in Skill_Effects)
            {
                foreach (Effect_Value EV in SE.Effect_Type_Value)
                {
                    if (EV.TYPE == BUFF_TYPE.Regen)
                    {
                        REGEN += EV.Value;
                    }
                }
            }
            if (REGEN > 0)
            {
                Buff_Effect[Buff_Effect.Count - 1].gameObject.SetActive(true);
                StartCoroutine(TurnOff_GameObject(Buff_Effect[Buff_Effect.Count - 1].gameObject, 2.0f));
                Current_HP += REGEN;
                Use_Text(Text_Array.HEAL_T, REGEN);
            }
            else if (REGEN < 0)
            {
                Current_HP += REGEN;
                Use_Text(Text_Array.Damaged_T, REGEN);
            }
            Update_Bar();
            CalculatorStatus();
        }
    }
    IEnumerator TurnOff_GameObject(GameObject OFF, float time)
    {
        yield return new WaitForSeconds(time);
        OFF.SetActive(false);
    }
    void Use_Text(Text_Array Case, int Value)//효과받았을때 수치 출력
    {
        Text_animator[num].SetTrigger("Damage");

        switch (Case)
        {
            case Text_Array.Damaged_T:
                text_mesh[num].color = Color.red;
                text_mesh[num].text = "-" + Value;
                break;
            case Text_Array.HEAL_T:
                text_mesh[num].color = Color.green;
                text_mesh[num].text = "+" + Value;
                break;
            case Text_Array.ATK_T:
                text_mesh[num].color = Color.yellow;
                if (Value > 0)
                    text_mesh[num].text = "+" + Value;
                else
                    text_mesh[num].text = "" + Value;
                break;
            case Text_Array.DEF_T:
                text_mesh[num].color = Color.cyan;
                if (Value > 0)
                    text_mesh[num].text = "+" + Value;
                else
                    text_mesh[num].text = "" + Value;
                break;
            case Text_Array.ATK_Count_T:
                text_mesh[num].color = Color.gray;
                if (Value > 0)
                    text_mesh[num].text = "+" + Value;
                else
                    text_mesh[num].text = "" + Value;
                break;
            case Text_Array.DamaedSpell_T:
                text_mesh[num].color = Color.blue;
                text_mesh[num].text = "-" + Value;
                break;
            case Text_Array.CP_T:
                Color color_CP = new Color();//분홍색
                color_CP.r = 1;
                color_CP.g = 0;
                color_CP.b = 1;
                color_CP.a = 1;
                text_mesh[num].color = color_CP;
                text_mesh[num].text = "-" + Value;
                break;
            case Text_Array.CD_T:
                Color color_CD = new Color();//주황색
                color_CD.r = 1;
                color_CD.g = 127 / 255f;
                color_CD.b = 0;
                color_CD.a = 1;
                text_mesh[num].color = color_CD;
                text_mesh[num].text = "-" + Value;
                break;
            default:
                text_mesh[num].color = Color.black;
                text_mesh[num].text = "+" + Value;
                break;
        }
        num += 1;
        if (text_mesh.Count >= num)
        {
            num = 0;
        }
    }//데미지 받았을때 텍
    void Use_Text(Text_Array Case_1, Text_Array Case_2, int Value, int Value_1)
    {
        StartCoroutine(Double_TextCorountine(Case_1, Case_2, Value, Value_1));
    }
    IEnumerator Double_TextCorountine(Text_Array Case_1, Text_Array Case_2, int value, int value_1)//두종류의 텍스트
    {
        Use_Text(Case_1, value);
        yield return new WaitForSeconds(0.3f);
        Use_Text(Case_2, value_1);
    }


    private void OnDisable()
    {
        CalculatorStatus();
        transform.position = OriginiPosi;
    }
    private void OnMouseOver()
    {
        Char_manager.CardMouseOver(this);
        CalculatorStatus();
    }
    private void OnMouseExit()
    {

        Char_manager.CardMouseExit(this);

    }

}
