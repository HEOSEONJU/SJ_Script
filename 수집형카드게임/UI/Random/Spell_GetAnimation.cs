using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.InteropServices;
using System.Linq;

public class Spell_GetAnimation : GetAnimation
{
    [SerializeField]
    List<Image> Bg;
    [SerializeField]
    List<Image> Main;
    [SerializeField]
    List<TextMeshProUGUI> Cost_Text;
    [SerializeField]
    List<TextMeshProUGUI> Card_Name;
    [SerializeField]
    List<TextMeshProUGUI> Exp;
    

    

    protected override IEnumerator CardDrew()//��í���� �������� ī�������Է�
    {
        Action = true;
        for (int i = 0; i <FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)//���� ������ŭ ī���̹����Է�
        {
            Bg[i].sprite = CardData.instance.CardDataFile.cards[FireBaseDB.instacne.Player_Data_instacne.GetPack[i]].BG_Image;//�����̹���
            Main[i].sprite= CardData.instance.CardDataFile.cards[FireBaseDB.instacne.Player_Data_instacne.GetPack[i]].Image;//�����̹���
            Cost_Text[i].text = CardData.instance.CardDataFile.cards[FireBaseDB.instacne.Player_Data_instacne.GetPack[i]].cost.ToString();//�ڽ�Ʈ����
            Card_Name[i].text = CardData.instance.CardDataFile.cards[FireBaseDB.instacne.Player_Data_instacne.GetPack[i]].CardName;//�̸�����
            Exp[i].text = CardData.instance.CardDataFile.cards[FireBaseDB.instacne.Player_Data_instacne.GetPack[i]].exp;//������
            #region ī�� 3��ũ�� ��ƼŬ����Ʈ����
            switch (CardData.instance.CardDataFile.cards[FireBaseDB.instacne.Player_Data_instacne.GetPack[i]].Rank)//��ũ��3�̸� ��ƼŬȰ��ȭ
            {
                case 3:
                    CardObject[i].Reset_Particle();
                    break;
                default:
                    CardObject[i].Stop_Particle();
                    break;
            }
            #endregion
        }
        #region ī�� �̱� 1�� 10�� �����ϰ� ����

        switch (FireBaseDB.instacne.Player_Data_instacne.UsePack)//1�� 10�� ���ⱸ���ؼ� �����ֱ�
        {
            case 1:
                StartCoroutine(CardNextToMove());
                yield return new WaitForSeconds(CardDelay);
                break;
            default:
                for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)//ī����������������� ��ġ
                {
                    CardAlignment(i);
                    yield return new WaitForSeconds(CardDelay);
                }
                break;
        }
        #endregion
        Action = false;
        
    }

    public override void Reset()//�ٽ� �̱�
    {
        if (CheckButton == false)
        {
            if (FireBaseDB.instacne.Player_Data_instacne.SpellCardPack >= FireBaseDB.instacne.Player_Data_instacne.UsePack)
            {
                
                if (FireBaseDB.instacne.Player_Data_instacne.UsePack != 1)
                {
                    OK_Button.SetActive(true);
                }
                END_Button.SetActive(false);
                CheckButton = true;

                Card_Positin_Reset();



                FireBaseDB.instacne.Player_Data_instacne.GetPack.Clear();



                for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)
                {
                    int Gold = 0;
                    

                    int num= cardGetUI.Random_Result(Random.Range(1, 100), false, out Gold);
                    if ((FireBaseDB.instacne.Player_Data_instacne.DeckCards.Count(Element => Element == num) + FireBaseDB.instacne.Player_Data_instacne.HaveCard.Count(Element => Element == num))<3)
                    {
                        FireBaseDB.instacne.Player_Data_instacne.HaveCard.Add(num);
                    }
                    else//3���̻��ΰ��
                    {
                        FireBaseDB.instacne.Player_Data_instacne.Gold += Gold;
                    }
                    FireBaseDB.instacne.Player_Data_instacne.GetPack.Add(num);
                }
                
                CurrentCount = FireBaseDB.instacne.Player_Data_instacne.UsePack;
                cardGetUI.ResetPackText();
                StartCoroutine(CardDrew());//�ٽû̱�
                CheckButton = true;
                FireBaseDB.instacne.Upload_Data(StoreTYPE.PACK);
                FireBaseDB.instacne.Upload_Data(StoreTYPE.HAVE);
                FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
            }
            else
            {
                POPUP_1.View_Text("���� Ƽ���� ���ڶ��ϴ�.");
            }
        }
    }
}

