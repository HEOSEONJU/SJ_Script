using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GetAnimation : MonoBehaviour
{
    [Header("Information")]

    
    [SerializeField]
    protected CardGetUI cardGetUI;

    [Header("Position")]    
    public List<Vector3> CardMovePosi;//ī����� ������ġ
    public List<SpellCard> CardObject;//������Ʈ ī�帮��Ʈ
    public Transform CenterPosi;//�߾� Ȯ�� ī�� ��ġ
    public Transform CardOutPosi;//���� ī�� �̵� ��ġ
    public List<Vector3> Card_Posi;//��ü������ 10���� ��ġ
    public FailWinodw POPUP_1;//�̱�� �������



    
    protected int CurrentCount;
    protected bool Action;//Ŭ�� �����ѻ��� üũ
    protected bool CheckButton;



    [Header("����")]
    public GameObject OK_Button;
    public GameObject END_Button;

    

    public float CardDelay = 0.3f;
    

    private void OnEnable()
    {
        CheckButton = true;
        Action = false;
        CurrentCount = FireBaseDB.instacne.Player_Data_instacne.UsePack;

        #region ī����ġ ũ�� �ʱ�ȭ
        
        foreach (SpellCard SC in CardObject)
        {
            SC.transform.localPosition = Vector3.zero;
            SC.transform.rotation = Quaternion.Euler(0, 180, 0);
            SC.gameObject.SetActive(false);
            SC.transform.localScale = Vector3.zero;
            SC.BackCardPosi();
        }
        #endregion
        #region ����ȸ����ŭ ī�尹�� Ȱ��ȭ
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)//����ī�尹����ŭ ī��Ȱ��ȭ
        {
            CardObject[i].gameObject.SetActive(true);
        }
        #endregion

        END_Button.SetActive(false);
        switch(FireBaseDB.instacne.Player_Data_instacne.UsePack)
        {
            case 1:
                OK_Button.SetActive(false);
                break;
            default:
                OK_Button.SetActive(true);
                break;
        }
        
        StartCoroutine(CardDrew());//ī��̱����
    }


    public void CardDrewFunction()
    {
        if (Action == false & CheckButton)
        {
            StartCoroutine(CardNextToMove());
        }
        
    }
    public void EndGet()//�̱�����
    {
        transform.parent.transform.parent.transform.GetComponent<RandomScene>().MoveScene();//���̵�
        StartCoroutine(CardResetPositionCorountine());
    }
    IEnumerator CardResetPositionCorountine()//ī����ġ ����� �ʱ�ȭ
    {
        yield return new WaitForSeconds(5.0f);

        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)//���Ѿ������ ī�� ����ġ�ΰ��� ���������
        {
            CardObject[i].transform.position = CenterPosi.position;
            CardObject[i].StartFlip();
        }

    }

    protected IEnumerator CardNextToMove()//ī�带������ �������ִ�ī��� ������ȭ��������̵�
    {

        Action = true;
        if (CurrentCount < 10 & (FireBaseDB.instacne.Player_Data_instacne.UsePack != 1))
        {
            var target = CardObject[CurrentCount];
            PRS Posi = new PRS(CardOutPosi.position, Quaternion.Euler(0, -180, 0), Vector3.one * 45f);
            target.originPosi = Posi;
            
            target.MoveTransForm(target.originPosi, true, 0.7f);
            yield return new WaitForSeconds(0.7f);

            target.originPosi = new PRS(CardOutPosi.position, Quaternion.Euler(0, -180, 0), Vector3.one * 15f);
            target.MoveTransForm(target.originPosi, false);
        }
        StartCoroutine(CardOpenCoroutine());//ȭ����̵��� ���ʿ��ִ� ī�� �߾ӿ� ��ġ

    }
    protected IEnumerator CardOpenCoroutine()//ī��1�徿���� �Ųٷΰ����̶� �ε��� 9������0������ ���ΰ����ߴٸ� ���κ����ִ°����ι�ġ 
    {
        CurrentCount -= 1;
        if (CurrentCount >= 0)
        {
            PRS Posi = new PRS(CenterPosi.position, Quaternion.Euler(0, -180, 0), Vector3.one * 45f);

            var target = CardObject[CurrentCount];
            target.originPosi = Posi;
            target.Change_Effect_Size(1);
            target.MoveTransForm(target.originPosi, true, 0.7f);
            yield return new WaitForSeconds(0.7f);
            CardObject[CurrentCount].StartFlip();
            Action = false;
            if (FireBaseDB.instacne.Player_Data_instacne.UsePack == 1)//1�ѱ�°Ŷ�� ��ü��������������
            {
                CheckButton = false;       
                END_Button.SetActive(true);
            }
        }
        else
        {
            PRS Posi;
            for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)
            {
                Posi = new PRS(Card_Posi[i], Quaternion.Euler(0, -180, 0), Vector3.one * 15f);
                var target = CardObject[i];
                target.Change_Effect_Size(0.5f);
                target.originPosi = Posi;
                target.MoveTransForm(target.originPosi, true, 0.7f);
            }
            StartCoroutine(Button_Delay(1.0f));
            CheckButton = false;
            Action = false;
        }
    }
    public void SkipButton()//1��Ŀ��� ��ü������
    {
        if (CheckButton & (!Action))
        {
            PRS Posi;
            for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)
            {
                if (CurrentCount <= i)
                {
                    Posi = new PRS(Card_Posi[i], Quaternion.Euler(0, -180, 0), Vector3.one * 15f);
                }
                else
                {
                    Posi = new PRS(Card_Posi[i], Quaternion.Euler(0, -180, 0), Vector3.one * 15f);
                    CardObject[i].StartFlip();
                }
                var target = CardObject[i];
                target.originPosi = Posi;
                target.Change_Effect_Size(0.5f);
                target.MoveTransForm(target.originPosi, true, 0.7f);
            }
            CheckButton = false;
            StartCoroutine(Button_Delay(1.0f));
        }
    }
    protected IEnumerator Button_Delay(float time)
    {
        yield return new WaitForSeconds(time);
        OK_Button.SetActive(false);
        END_Button.SetActive(true);
    }


    public void CardOpen()
    {
        CurrentCount -= 1;
        PRS Posi = new PRS(CenterPosi.position, Quaternion.Euler(0, -180, 0), Vector3.one * 45f);
        var target = CardObject[CurrentCount];
        target.originPosi = Posi;
        target.MoveTransForm(target.originPosi, true, 0.7f);
    }



    

    protected void CardAlignment(int num)//ī��������ġ��
    {
        PRS Posi = new PRS(CardMovePosi[num], Quaternion.Euler(0, -180, 0), Vector3.one * 15f);
        var target = CardObject[num];
        target.originPosi = Posi;
        target.MoveTransForm(target.originPosi, true, 0.7f);
    }

    protected void Card_Positin_Reset()
    {
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)
        {
            CardObject[i].transform.position = CenterPosi.position;
            CardObject[i].transform.localScale = Vector3.zero;
            CardObject[i].BackCardPosi();

        }

    }

    public virtual void Reset()//�ٽ� �̱�
    {
        return; 
    }
    protected virtual IEnumerator CardDrew()//ī�� �̹��� �����ϱ�
    {
        yield return null;
    }
}
