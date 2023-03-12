using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Make_Card_Effect : Base_Effect
{
    [SerializeField]
    int Rank1_Card;
    [SerializeField]
    int Rank2_Card;
    [SerializeField]
    int Rank3_Card;
    [SerializeField]
    int Special_ID;
    [SerializeField]
    int Special_Card;

    public override void Effect_Function(Manager _Manager)
    {
        int Make_Count=Rank1_Card+Rank2_Card+Rank3_Card+Special_Card;
        int Rank1 = Rank1_Card;
        int Rank2 = Rank2_Card;
        int Rank3 = Rank3_Card;
        int Rank_S = Special_ID;
        
        CardScriptable DataBox = GameObject.Find("CardData").GetComponent<CardData>().CardDataFile; //����ī�嵥���͸�������
        List<int> Init_List = new List<int>();
        List<int> Special_Init_List = new List<int>();
        while (Make_Count >= 1)//����ī���� 1�̻��ϰ��
        {
            //Debug.Log("�����ҳ���ī�尹��"+v);

            int Case_num = Random.Range(0, 4);
            //Debug.Log("���۵ɷ�ũ:"+Case_num);
            switch (Case_num)
            {
                case 0:
                    if (Rank1 >= 1)//����1�������� 1�̻��̸� �����Ѱ����� 1�������� 1�����̰� 1��ī������
                    {
                        Rank1--;
                        Make_Count--;
                        _Manager.InitCard_NUM(DataBox, 1, Init_List);
                    }
                    break;
                case 1:
                    if (Rank2 >= 1)//����2�������� 1�̻��̸� �����Ѱ����� 2�������� 1�����̰� 1��ī������
                    {
                        Rank2--;
                        Make_Count--;
                        _Manager.InitCard_NUM(DataBox, 2, Init_List);
                    }
                    break;
                case 2:
                    if (Rank3 >= 1)//����3�������� 1�̻��̸� �����Ѱ����� 3�������� 1�����̰� 1��ī������
                    {
                        Rank3--;
                        Make_Count--;
                        _Manager.InitCard_NUM(DataBox, 3, Init_List);
                    }
                    break;
                case 3:
                    if (Make_Count >= 1 & Rank_S >= 1)//����4�������� 1�̻��̸� �����Ѱ����� 4�������� 1�����̰� 1��ī������
                    {
                        
                        _Manager.InitCard_NUM(DataBox, 4, Special_Init_List, Rank_S);
                        Make_Count--;

                    }
                    break;
            }


        }
        _Manager.MYDeck.Play_Particle();
        int c = 0;
        for (int j = 0; j < Init_List.Count; j++)
        {
            if (_Manager.MYDeck.DeckCards.Count < _Manager.MYDeck.MaxCount)
            {

                if (j == Init_List.Count - 1)
                {
                    //_Manager.MYDeck.Stop_Particle(j);
                }
                c++;
                _Manager.MYDeck.Function_Stop_Particle(j);
                _Manager.MYDeck.Extra_InitCard(Init_List[j], j);
                _Manager.DelayUseSpell += 0.1f;
            }
            else
            {
                _Manager.MYDeck.Function_Stop_Particle(j);
                c++;
                _Manager.MYDeck.ShuffleDeck();
                return;
            }
        }
        for (int j = 0; j < Special_Init_List.Count; j++)
        {
            if (_Manager.MYDeck.DeckCards.Count < _Manager.MYDeck.MaxCount)
            {

                if (j == Special_Init_List.Count - 1)
                    
                    //_Manager.MYDeck.Stop_Particle(j);
                    
                

                _Manager.MYDeck.Function_Stop_Particle(j);
                _Manager.MYDeck.Special_InitCard(Special_Init_List[j], c++);
                _Manager.DelayUseSpell += 0.1f;
            }
            else
            {
                _Manager.MYDeck.Function_Stop_Particle(j);

                _Manager.MYDeck.ShuffleDeck();
                return;
            }
        }


        _Manager.MYDeck.ShuffleDeck();
        return;

    }
    public override void RequireMent(Manager _Manager)
    {
        int Make_Count = Rank1_Card + Rank2_Card + Rank3_Card + Special_Card;
        if(_Manager.MYDeck.Count+Make_Count>40)
        {
            _Manager._Result = Card_Result.RequireMent;//���ʰ�
            Require = false;
            return;
        }
        Require = true;

    }
}
