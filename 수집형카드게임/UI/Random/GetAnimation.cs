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
    public List<Vector3> CardMovePosi;//카드왼쪽 정렬위치
    public List<SpellCard> CardObject;//오브젝트 카드리스트
    public Transform CenterPosi;//중앙 확대 카드 위치
    public Transform CardOutPosi;//우측 카드 이동 위치
    public List<Vector3> Card_Posi;//전체공개시 10장의 위치
    public FailWinodw POPUP_1;//뽑기권 부족출력



    
    protected int CurrentCount;
    protected bool Action;//클릭 가능한상태 체크
    protected bool CheckButton;



    [Header("버툰")]
    public GameObject OK_Button;
    public GameObject END_Button;

    

    public float CardDelay = 0.3f;
    

    private void OnEnable()
    {
        CheckButton = true;
        Action = false;
        CurrentCount = FireBaseDB.instacne.Player_Data_instacne.UsePack;

        #region 카드위치 크기 초기화
        
        foreach (SpellCard SC in CardObject)
        {
            SC.transform.localPosition = Vector3.zero;
            SC.transform.rotation = Quaternion.Euler(0, 180, 0);
            SC.gameObject.SetActive(false);
            SC.transform.localScale = Vector3.zero;
            SC.BackCardPosi();
        }
        #endregion
        #region 뽑은회수만큼 카드갯수 활성화
        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)//뽑은카드갯수만큼 카드활성화
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
        
        StartCoroutine(CardDrew());//카드뽑기시작
    }


    public void CardDrewFunction()
    {
        if (Action == false & CheckButton)
        {
            StartCoroutine(CardNextToMove());
        }
        
    }
    public void EndGet()//뽑기종료
    {
        transform.parent.transform.parent.transform.GetComponent<RandomScene>().MoveScene();//씬이동
        StartCoroutine(CardResetPositionCorountine());
    }
    IEnumerator CardResetPositionCorountine()//카드위치 가운데로 초기화
    {
        yield return new WaitForSeconds(5.0f);

        for (int i = 0; i < FireBaseDB.instacne.Player_Data_instacne.UsePack; i++)//씬넘어가기전에 카드 제위치로가고 뒤집어놓기
        {
            CardObject[i].transform.position = CenterPosi.position;
            CardObject[i].StartFlip();
        }

    }

    protected IEnumerator CardNextToMove()//카드를뽑을때 기존에있던카드는 오른쪽화면밖으로이동
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
        StartCoroutine(CardOpenCoroutine());//화면밖이동후 왼쪽에있던 카드 중앙에 배치

    }
    protected IEnumerator CardOpenCoroutine()//카드1장씩개방 거꾸로개방이라 인덱스 9번부터0번까지 전부개방했다면 전부보여주는것으로배치 
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
            if (FireBaseDB.instacne.Player_Data_instacne.UsePack == 1)//1팩까는거라면 전체펴지기하지않음
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
    public void SkipButton()//1장식에서 전체공개로
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



    

    protected void CardAlignment(int num)//카드정렬위치로
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

    public virtual void Reset()//다시 뽑기
    {
        return; 
    }
    protected virtual IEnumerator CardDrew()//카드 이미지 갱신하기
    {
        yield return null;
    }
}
