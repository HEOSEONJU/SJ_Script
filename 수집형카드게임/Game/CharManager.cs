using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Linq;

public class CharManager : MonoBehaviour
{
    Manager _Manager;
    public GameObject MonsterCard;
    public GameObject Infomation;//확대해서 보여주는 오브젝트
    public GameCard SelectedCard;//선택된 캐릭터
    public List<GameCard> CombatChar;//배틀에 참가한 캐릭터들
    public List<TextMeshProUGUI> TextList;
    [SerializeField]
    Transform Contents_Parent;
    public int Skill_NUM;
    private void OnEnable()
    {
        Infomation.SetActive(false);
    }
    public void CharAwke()//배틀 시작시 플레이어데이터에서 배틀에 참가한 캐릭터 정보를 받아오는기능
    {
        _Manager = GetComponent<Manager>();
        CombatChar = new List<GameCard>();
        foreach(int INDEX in FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards)
        {
            CombatChar.Add(MonsterCard.transform.GetChild(INDEX).GetComponent<GameCard>());
        }
    }
    public void CharInit()//플레이어데이터에서 배틀에 참가한 캐릭터 정보를 받아오는기능
    {
        foreach (int INDEX in FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards)
        {
            MonsterCard.transform.GetChild(INDEX).GetComponent<GameCard>().InputID(FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards[INDEX]);
        }
    }
    public IEnumerator MonsterCardReset(float DelayTime=3.0f)//배틀에 참가한 캐릭터 리셋하는 기능
    {
        yield return new WaitForSeconds(DelayTime);
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards.Count; i++)
        {
            _Manager.Target_Solo = MonsterCard.transform.GetChild(i).GetComponent<GameCard>();   
            for (int j=0;j< _Manager.Target_Solo.Skill_ID.Count;j++)
            {
                if (_Manager.Target_Solo.Skill_ID[j] == 0)
                {
                    continue;
                }
                CharSkillData T = CardData.instance.CharSkillFile.Find_Skill(_Manager.Target_Solo.Skill_ID[j]);
                Skill_NUM = T.id;
                Effect[] e = T.Effects.GetComponents<Effect>();
                foreach (Effect effect in e)
                {
                    effect.RequireMent(_Manager);
                    if (effect.Require == false)
                    {
                        continue;
                    }
                    effect.Effect_Enemy_Function(_Manager);
                    effect.Effect_Solo_Function(_Manager);
                    effect.Effect_Function(_Manager);
                    effect.Damage_Enemy_Function(_Manager.Enemy_Manager);
                }
                switch(T.CardType)
                { 
                    case 3:
                        yield return new WaitForSeconds(0.7f);
                        break;
                }
                _Manager.LoadingTimer += 0.5f;
                yield return new WaitForSeconds(0.2f);
            }
            _Manager.LoadingTimer = 0;
            _Manager.Enemy_Manager.Image_buff();
        }
    }

    public void TurnEnd()//해당 각 캐릭터의 효과의 지속시간 감소
    {
        foreach (int INDEX in FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards)
        {
            MonsterCard.transform.GetChild(INDEX).GetComponent<GameCard>().TURNUSE();
        }
    }
    public int MAX_Drew_Count()//캐릭터들이 가진 최대 드로우횟수를 계산하는 함수
    {
        List<int> Temp= new List<int>();
        foreach (int INDEX in FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards)
        {
            Temp.Add(MonsterCard.transform.GetChild(INDEX).GetComponent<GameCard>().MAX_DREW_Count());   
        }
        return Temp.Max();
    }
    public void Renge_HP_Effect()//캐릭터들이 체력자동기능을 작동하는 함수
    {
        foreach (int INDEX in FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards)
        {
            MonsterCard.transform.GetChild(INDEX).GetComponent<GameCard>().REGEN_Active();
        }
    }
    void InformationCard(bool INFO, GameCard Card)//선택된 캐릭터를 확대하는 함수
    {
        if (INFO & Card != null)
        {
            Infomation.SetActive(true);
            Card.CalculatorStatus();
            TextList[0].text = "ATK:" + Card.Current_ATK;
            TextList[1].text = "DEF:" + Card.Current_DEF;
            TextList[2].text = "HP:" + Card.Current_HP;
            TextList[3].text = "MR:" + Card.Effect_Magic_Regi + "%";
            TextList[4].text = "CP:" + Card.Current_CriticalPercent + "%";
            TextList[5].text = "CD:" + (100 + Card.Current_CriticalDamage) + "%";
            Color color_0 = Color.white;
            color_0.a = 0;
            Color color_1 = Color.white;
            color_1.a = 1;
            int C_Index=0;
            foreach (SpellEffect SE in Card.Skill_Effects)
            {
                Image temp = Contents_Parent.GetChild(C_Index++).GetComponent<Image>();
                temp.color = color_1;
                temp.sprite = CardData.instance.CharSkillFile.Skill[SE.ID].Image;
            }
            foreach (SpellEffect SE in Card.Spell_Effects)
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
        else
        {
            Infomation.SetActive(false);
        }
    }
    public void CardMouseOver(GameCard Card)//마우스로 갖다된 카드확대
    {
        if (SelectedCard == null & !_Manager.STOP)
        {
            SelectedCard = Card;
            InformationCard(true, Card);
        }
    }
    public void CardMouseExit(GameCard Card)//마우스로 갖다된 카드종료
    {
        InformationCard(false, Card);
        SelectedCard = null;
    }
}

