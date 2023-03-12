using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class CharManager : MonoBehaviour
{
    Manager _Manager;
    public GameObject MonsterCard;
    public GameCard SelectedCard;
    public GameObject Infomation;
    public List<GameCard> CombatChar;
    public List<TextMeshProUGUI> TextList;
    [SerializeField]
    Transform Contents_Parent;


    //bool CanAttack;

    public int Skill_NUM;
    private void OnEnable()
    {
        Infomation.SetActive(false);
        //MonsterCardReset();
    }
    public void CharAwke()
    {
        _Manager = GetComponent<Manager>();

        //CanAttack = true;

        CombatChar = new List<GameCard>();
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards.Count; i++)
        {
            int temp = FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards[i];
            CombatChar.Add(MonsterCard.transform.GetChild(i).GetComponent<GameCard>());
        }
    }

    public void CharInit()
    {
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards.Count; i++)
        {
            
            MonsterCard.transform.GetChild(i).GetComponent<GameCard>().InputID(FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards[i]);
            
        }
    }
    
    public IEnumerator MonsterCardReset(float DelayTime=3.0f)
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
                //Debug.Log("찾고자하는스킬아이디" + _Manager.Target_Solo.Skill_ID[j]); 
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

    public void TurnEnd()
    {
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards.Count; i++)
        {


            MonsterCard.transform.GetChild(i).GetComponent<GameCard>().TURNUSE();
        }


    }

    public int MAX_Drew_Count()
    {
        int Count = 1;
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards.Count; i++)
        {


            if (Count < MonsterCard.transform.GetChild(i).GetComponent<GameCard>().MAX_DREW_Count())
                Count = MonsterCard.transform.GetChild(i).GetComponent<GameCard>().MAX_DREW_Count();
        }


        return Count;
    }

    public void Renge_HP_Effect()
    {
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards.Count; i++)
        {


            MonsterCard.transform.GetChild(i).GetComponent<GameCard>().REGEN_Active();
        }

    }


    void InformationCard(bool INFO, GameCard Card)
    {

        if (INFO & Card != null)
        {
            //Debug.Log("선택된아이디:" + Card.ID);
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











    public void CardMouseOver(GameCard Card)//카드확대
    {
        if (SelectedCard == null & !_Manager.STOP)
        {
            SelectedCard = Card;
            InformationCard(true, Card);
        }
    }
    public void CardMouseExit(GameCard Card)
    {
        InformationCard(false, Card);
        SelectedCard = null;
    }

    public void CardMouseDown()
    {

    }

    public void CardMouseUp()
    {


    }


}

