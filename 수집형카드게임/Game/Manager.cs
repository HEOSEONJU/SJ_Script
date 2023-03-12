using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using TMPro;
using System;

using Random = UnityEngine.Random;
using System.Text.RegularExpressions;

public class Manager : MonoBehaviour
{
    public static Manager instance;

    public float Height = 0.5f;//원형패의최대높이
    public float Size = 0.1f;//패에있는 카드 크기
    public StageUI Stage_UI;
    [Header("게임참조")]
    public PlayerInfos PlayerData;
    public Deck MYDeck;
    public CardHand Hand;
    public CharManager Char_Manager;
    public EnemyManager Enemy_Manager;

    [Header("게임정보")]
    bool Loading;
    public bool Drag;
    float Delaytime5 = 0.5f;
    public float DelayUseSpell = 0.3f;
    [SerializeField]
    bool Turn;


    public Transform Cost_Trans;
    public Transform MAX_Cost_Trans;
    public int My_Cost;
    public int My_MAXCost;
    public int DrewCount = 1;







    public bool STOP;

    float CardDelay = 0.5f;//카드뽑기딜레이
    int StartDrew = 5;
    public bool onMyCardArea;
    public TextMeshProUGUI AttackOrder;

    CardState state;


    [Header("확대참조")]
    public GameObject EnLargeObject;
    public Image Large_Main;
    public TextMeshProUGUI Large_Cost;
    public TextMeshProUGUI Large_Name;
    public TextMeshProUGUI Large_Exp;


    public FailWinodw POPUP_1;

    [SerializeField]
    GameObject Setting;

    [Header("보상참조")]
    [SerializeField]
    PlayerInfos Player_Reward;
    [SerializeField]
    GameObject POPUP_END;
    [SerializeField]
    GameObject POPUP_Victroy;
    [SerializeField]
    GameObject POPUP_Game_over;
    [SerializeField]
    List<TextMeshProUGUI> Reward_Text;
    [SerializeField]
    TextMeshProUGUI Level_Text;
    [SerializeField]
    TextMeshProUGUI EXP_Text;

    Vector3 OriginPos;

    public float LoadingTimer = 0;

    


    [Header("스펠카드 사용시 참조")]
    public Card_Result _Result;
    public GameCard Target_Solo;

    public SpellCard SelectedCard;
    enum CardState { Not, Over, Drag }
    private void Awake()
    {
        Char_Manager.CharAwke();
        OriginPos = POPUP_END.transform.position;
    }

    void OnEnable()
    {
        POPUP_END.SetActive(false);
        My_Cost = My_MAXCost = 1;
        DrewCount = 1;
        Loading = true;
        Hand.Init();
        MYDeck.manager = this;
        MYDeck.Init();
        Drag = false;
        Char_Manager.CharInit();
        StartCoroutine(FirstDrew());
        StartCoroutine(Char_Manager.MonsterCardReset());
        Turn = true;
        ViewCost();
        STOP = false;
        EnLargeObject.SetActive(false);
        AttackOrder.text = "공격준비완료";
        Setting.SetActive(false);
    }



    public void Restart()
    {
        if (state == CardState.Drag)
        {
            CloseSetting();
            MYDeck.Reset_Deck_Hand();
            DrewCount = 1;
            Loading = true;
            Hand.Init();
            MYDeck.manager = this;
            MYDeck.Init();
            Drag = false;
            Char_Manager.CharInit();
            StartCoroutine(FirstDrew());

            Turn = true;
            ViewCost();
            STOP = false;
            Enlarge(false);
            AttackOrder.text = "공격준비완료";
            Setting.SetActive(false);

            StartCoroutine(Char_Manager.MonsterCardReset());

            Enemy_Manager.Restart();
        }
        else
        {
            POPUP_1.View_Text("행동중 에는 항복할 수 없습니다.");
        }

    }


    public void Enlarge(bool state)
    {
        EnLargeObject.SetActive(state);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))//카드정리
        {
            CardAlignment();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))//카드셔플
        {
            MYDeck.ShuffleDeck();
        }


        DetectCardArea();
        if (Drag)
        {
            CardDrag();
        }

        if (LoadingTimer >= 0)
        {
            LoadingTimer -= Time.deltaTime;
            state = CardState.Not;
        }
        else
        {
            state = CardState.Drag;
        }
    }
    IEnumerator SwapCorountine()//턴넘겨주기 진행코루틴
    {
        Turn = !Turn;
        if (Turn)//내턴시작
        {
            LoadingTimer = 0;
            AttackOrder.text = "공격준비완료";
            if (My_MAXCost <= 9)//최대코스트 9이하라면 1증가
            { My_MAXCost += 1; }
            My_Cost = My_MAXCost;//코스트회복
            ViewCost();//코스트갱신
            Char_Manager.TurnEnd();// 턴종료시 적용되있는 효과들 턴1씩소모
            Enemy_Manager.TurnEnd();
            DrewCount = Char_Manager.MAX_Drew_Count();
            LoadingTimer += DrewCount * 0.5f + 0.1f;//드로우매수만큼 딜레이
            for (int i = 0; i < DrewCount; i++)//드로우정리
            {
                if (MYDeck.Drew())//드로우가능하면 드로우하고패정리
                {
                    CardAlignment();
                }
            }
        }
        else//내턴 종료 공격시작 
        {   
            LoadingTimer = 100;
            Char_Manager.Renge_HP_Effect();//체력재생버프작동

            for (int i = 0; i < Char_Manager.CombatChar.Count; i++)//살아있다면 공격횟수 재계산
            {
                if (Char_Manager.CombatChar[i].Current_HP > 0)
                {
                    Char_Manager.CombatChar[i].ResetAttackCount();
                }
            }
            yield return new WaitForSeconds(1f);
            AttackOrder.text = "공격중";
            GameCard temp;
            for (int i = 0; i < Char_Manager.CombatChar.Count; i++)
            {
                temp = Char_Manager.MonsterCard.transform.GetChild(i).GetComponent<GameCard>();
                if (Enemy_Manager.Live == true)
                {
                    while (temp.AttackCount >= 1 && temp.Live)//공격횟수가 다소모할때까지 공격
                    {
                        if (Enemy_Manager.Live == false)//연속공격중사망시
                        {
                            break;
                        }
                        //Debug.Log(temp.name + "가 공격중 /" + (temp.AttackCount - 1) + "번 남음");
                        temp.animator.SetTrigger("Attack");
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                else//공격 도중 적이 사망하면 코루틴종료
                {
                    yield break;
                }
            }
            AttackOrder.text = "상대턴";
            Enemy_Manager.StartEnemyTurn();
        }
    }










    public void Button_Swap()//버툰으로 함수부르기
    {
        if (LoadingTimer <= 0 & Enemy_Manager.Live)
        {
            LoadingTimer += 3.0f;
            SwapTurn();
        }
    }



    public void SwapTurn()//턴넘겨주는함수
    {



        StartCoroutine(SwapCorountine());



    }
    public void ViewCost()//코스트이미지갱신
    {
        for (int i = 0; i < Cost_Trans.childCount; i++)
        {
            if (i < My_Cost)
            {
                Cost_Trans.GetChild(i).GetComponent<Image>().enabled = true;
            }
            else
            {
                Cost_Trans.GetChild(i).GetComponent<Image>().enabled = false;
            }
        }
        for (int i = 0; i < MAX_Cost_Trans.childCount; i++)
        {
            if (i < My_MAXCost)
            {
                MAX_Cost_Trans.GetChild(i).GetComponent<Image>().enabled = true;
            }
            else
            {
                MAX_Cost_Trans.GetChild(i).GetComponent<Image>().enabled = false;
            }
        }
    }






    IEnumerator CardAlignmentCoroutine()//카드정리중에는 못움직임
    {

        LoadingTimer = CardDelay;
        yield return new WaitForSeconds(CardDelay);


    }

    public void CardAlignment()//손패 카드정리
    {
        List<PRS> orginCardRPSs = new List<PRS>();
        orginCardRPSs = RoundAlignment(Hand.myCardLeft, Hand.myCardRight, Hand.CurrentHand, Height, Vector3.one * Size);//카드크기조절
        for (int i = 0; i < Hand.CurrentHand; i++)
        {
            var target = Hand.Cards[i];
            target.GetComponent<SpellCard>().originPosi = orginCardRPSs[i];
            target.GetComponent<SpellCard>().MoveTransForm(target.GetComponent<SpellCard>().originPosi, true, CardDelay);
            StartCoroutine(CardAlignmentCoroutine());
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)//카드 원형으로정리도와주는함수
    {
        float[] objLerps = new float[objCount];
        List<PRS> results = new List<PRS>(objCount);
        switch (objCount)
        {
            case 1: objLerps = new float[] { 0.5f }; break;
            case 2: objLerps = new float[] { 0.35f, 0.65f }; break;
            case 3: objLerps = new float[] { 0.25f, 0.5f, 0.75f }; break;
            default:
                float interval = 1f / (objCount - 1);
                for (int i = 0; i < objCount; i++)
                {
                    objLerps[i] = interval * i;
                }
                break;
        }
        for (int i = 0; i < objCount; i++)
        {
            var targetPos = Vector3.Lerp(leftTr.position, rightTr.position, objLerps[i]);
            var targetRot = Quaternion.identity;
            if (objCount >= 4)
            {
                float curve = Mathf.Sqrt(Mathf.Pow(height, 2) - Mathf.Pow(objLerps[i] - 0.5f, 2));
                if (i == 0 | i == objCount - 1)
                {
                    curve += 0.2f;
                }
                targetPos.y += curve;
                targetRot = Quaternion.Slerp(leftTr.rotation, rightTr.rotation, objLerps[i]);
            }
            results.Add(new PRS(targetPos, targetRot, scale));
        }

        return results;
    }
    IEnumerator FirstDrew()//게임시작시 5장드로우하는함수
    {
        for (int i = 0; i < StartDrew; i++)
        {
            yield return new WaitForSeconds(Delaytime5);
            if (MYDeck.Drew())
            {
                CardAlignment();
            }
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 마우스에 반응하는 메소드
    //////////////////////////////////////////////////////////////////////////////////


    void CardDrag()//카드드래그함수
    {
        if (!onMyCardArea)
        {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePos.z = -10f;
            SelectedCard.MoveTransForm(new PRS(MousePos, Quaternion.identity, SelectedCard.originPosi.scale), false);
        }
    }

    void DetectCardArea()//손패지역인지 체크
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = -10f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("HandAreaCheck");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard(bool Enlarge, SpellCard Card)//카드갖다대면 확대하는함수
    {
        if (Enlarge)
        {
            this.Enlarge(true);
            int Temp_ID = Card.CardNumber;
            if (Card.Special)
            {
                Large_Main.sprite = MYDeck.CardDataBase.Special_cards[Temp_ID].Image;
                Large_Main.material = null;
                Large_Cost.text = MYDeck.CardDataBase.Special_cards[Temp_ID].cost + "";
                Large_Name.text = MYDeck.CardDataBase.Special_cards[Temp_ID].CardName + "";
                Large_Exp.text = MYDeck.CardDataBase.Special_cards[Temp_ID].exp + "";
            }
            else
            {
                Large_Main.sprite = MYDeck.CardDataBase.cards[Temp_ID].Image;
                Large_Main.material = null;
                Large_Cost.text = MYDeck.CardDataBase.cards[Temp_ID].cost + "";
                Large_Name.text = MYDeck.CardDataBase.cards[Temp_ID].CardName + "";
                Large_Exp.text = MYDeck.CardDataBase.cards[Temp_ID].exp + "";
            }
        }
        else
        {
            EnLargeObject.SetActive(false);
        }
    }

    public void CardMouseOver(SpellCard Card)//카드를마우스에 갖다대면작동하는함수
    {
        if (state == CardState.Not)
            return;
        if (Drag == false)
        {
            SelectedCard = Card;
            EnlargeCard(true, Card);
        }
    }

    public void CardMouseExit(SpellCard Card)
    {
        EnlargeCard(false, Card);
    }

    public void DeckMouseOver(Canvas Count)//덱위에 마우스를올려 덱매수를확인하는함수
    {

        if (Drag == false)
        {
            Count.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "" + MYDeck.transform.GetChild(0).transform.childCount;
            Count.gameObject.SetActive(true);
        }
    }

    public void DeckMouseExit(Canvas Count)
    {
        Count.gameObject.SetActive(false);
    }

    public void CardMouseDown()
    {
        if (state == CardState.Not)
            return;
        Drag = true;
    }

    public void CardMouseUp()
    {
        if (state == CardState.Not)
            return;
        Drag = false;
        if (SelectedCard != null)
        {
            state = CardState.Not;
            SelectedCard.MoveTransForm(SelectedCard.originPosi, false);
            StartCoroutine(Card_Effect());
        }
    }

    //////////////////////////////////////////////////////////////////////////////////
    /// <summary>
    /// 스펠효과적용하는
    //////////////////////////////////////////////////////////////////////////////////
    IEnumerator Card_Effect()
    {

        bool Check;
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = -10f;
        DelayUseSpell = 0.3f;


        RaycastHit2D[] hits = Physics2D.RaycastAll(MousePos, Vector3.forward);
        int layer;
        _Result = Card_Result.Reset;
        switch (SelectedCard.CardType)//카드타입체크하고 반응할 레이어고르기
        {
            case 0:
                //Debug.Log("적에게공격");
                layer = LayerMask.NameToLayer("ENEMY");
                break;
            case 1:
                //Debug.Log("나에게카드올려놓기");
                layer = LayerMask.NameToLayer("MYCHAR");
                break;
            case 2:
                //Debug.Log("광역기");
                layer = LayerMask.NameToLayer("MYAREA");
                break;
            case 3:
                //Debug.Log("플레이어에게");
                layer = LayerMask.NameToLayer("MYAREA");
                break;
            case 4:
                layer = LayerMask.NameToLayer("MYCHAR");
                //단일한테쓰고 광역효과적용
                break;
            case 5:
                layer = LayerMask.NameToLayer("MYCHAR");
                //단일한테쓰고 플레이어적용
                break;
            default:
                layer = 0;
                break;

        }
        Check = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        //Debug.Log("콜라이더적중=" + Check);

        if (Check == false)
        {
            _Result = Card_Result.Wrong_Target;
            Reustl_Failed();
            yield break;
        }
        else
        {
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].transform.TryGetComponent<GameCard>(out var Temp_MY))
                {
                    Debug.Log("확인된레이캐스트" + hits[i].collider.name);
                    Target_Solo = Temp_MY;
                    break;
                }
            }
            LoadingTimer += 0.5f;
        }
        if (My_Cost < SelectedCard.Cost)
        {
            _Result = Card_Result.Cost_lack;
            Reustl_Failed();
            yield break;
        }

        Effect[] e = CardData.instance.CardDataFile.Find_Spell_Card(SelectedCard.CardNumber).Effects.GetComponents<Effect>();
        foreach (Effect effect in e)
        {
            effect.RequireMent(this);
            if (effect.Require == false)
            {
                Reustl_Failed();//카드발동조건을 한개라도 못맞추는순간 종료
                yield break;
            }
        }
        if (Check)
        {
            switch (SelectedCard.CardType)
            {
                case 1:   //내캐릭터에게적용
                    foreach (Effect effect in e)
                    {
                        effect.Effect_Solo_Function(this);
                    }
                    break;
                case 0://적에게
                    for (int i = 0; i < hits.Length; i++)
                    {
                        if (hits[i].transform.TryGetComponent<EnemyManager>(out var Temp_Enemy))
                        {

                            foreach (Effect effect in e)
                            {
                                effect.Effect_Enemy_Function(this);
                                effect.Damage_Enemy_Function(Temp_Enemy);
                            }
                            break;
                        }
                    }
                    break;
                case 2://광역 자신캐릭터
                    foreach (Effect effect in e)
                    {
                        effect.Effect_Solo_Function(this);
                    }
                    break;
                case 3: //플레이어에게 적용
                    foreach (Effect effect in e)
                    {
                        effect.Effect_Function(this);
                    }
                    break;
                case 4: //플레이어에게 적용 단일 캐릭터희생적용


                    foreach (Effect effect in e)
                    {
                        effect.Effect_Solo_Function(this);
                    }
                    break;
                case 5:
                    foreach (Effect effect in e)
                    {
                        effect.Effect_Function(this);
                    }
                    break;
            }

        }

        switch (_Result)
        {
            case Card_Result.Success:
                Debug.Log("성공");
                EnLargeObject.SetActive(false);
                CostUse(SelectedCard.Cost);
                Hand.UseCard(SelectedCard);
                Enlarge(false);
                CardAlignment();
                yield return new WaitForSeconds(DelayUseSpell);//스펠카드를쓰고나면 카드효과를진행하는0.3초동안 드래그방지를해 코드꼬임을 방지
                break;
            case Card_Result.Duplication:
                POPUP_1.View_Text("이미 적용된 효과입니다");
                break;
            case Card_Result.Wrong_Target:
                POPUP_1.View_Text("잘못된 적용대상입니다");
                break;
            case Card_Result.Char_Die:
                POPUP_1.View_Text("이미 사망한 적용대상입니다");
                break;
            case Card_Result.Cost_lack:
                POPUP_1.View_Text("코스트가 부족합니다.");
                break;
            case Card_Result.CantCharge_Cost:
                POPUP_1.View_Text("이미 사용할 수 있는 코스트가  최대입니다");
                break;
            case Card_Result.CantDrew:
                POPUP_1.View_Text("덱에 이미 카드가 없습니다");
                break;
            case Card_Result.RequireMent:
                POPUP_1.View_Text("발동 조건이 만족 하지 않습니다.");
                break;
        }
        state = CardState.Drag;
        ViewCost();
    }
    public void Reustl_Failed()//실패결과스위치문
    {
        switch (_Result)
        {

            case Card_Result.Duplication:
                POPUP_1.View_Text("이미 적용된 효과입니다");

                break;
            case Card_Result.Wrong_Target:
                POPUP_1.View_Text("잘못된 적용대상입니다");

                break;
            case Card_Result.Char_Die:
                POPUP_1.View_Text("이미 사망한 적용대상입니다");

                break;
            case Card_Result.Cost_lack:
                POPUP_1.View_Text("코스트가 부족합니다.");

                break;
            case Card_Result.CantCharge_Cost:
                POPUP_1.View_Text("이미 사용할 수 있는 코스트가  최대입니다");
                break;
            case Card_Result.CantDrew:
                POPUP_1.View_Text("덱에 이미 카드가 없습니다");
                break;
            case Card_Result.RequireMent:
                POPUP_1.View_Text("발동 조건이 만족 하지 않습니다.");
                break;




        }
        state = CardState.Drag;



        ViewCost();
    }
    public void InitCard_NUM(CardScriptable DataBox, int Rank, List<int> Init_List, int Special_num = 0)//카드 생성기능
    {
        int Ran = 1;

        if (Rank <= 3)
        {
            while (true)
            {
                Ran = Random.Range(1, DataBox.cards.Count);
                for (int i = 0; i < DataBox.Ban_List.Count; i++)
                {
                    if (DataBox.Ban_List[i] == Ran)
                    {
                        continue;
                    }
                }
                if (DataBox.cards[Ran].Rank == Rank)
                    break;
            }
            Init_List.Add(Ran);
        }
        else
        {
            //Debug.Log("스폐셜카드생성");
            Init_List.Add(Special_num);
        }
    }
    public void CostUse(int UsingCost)//코스트소모되는 위치 확인용
    {
        My_Cost -= UsingCost;
        ViewCost();


    }
    public void StopManager()
    {
        state = CardState.Not;
        Loading = false;
    }

    public void OpenSetting()
    {
        Setting.SetActive(true);
    }
    public void CloseSetting()
    {
        Setting.SetActive(false);
    }

    public void GiveUpGame()
    {
        Setting.SetActive(false);
        GameEnd(false);
    }


    public void GameEnd(bool result)
    {
        int Live_Check = 0;

        for (int i = 0; i < Char_Manager.CombatChar.Count; i++)
        {
            if (Char_Manager.CombatChar[i].Live)
            {
                Live_Check++;
            }
        }
        if (Live_Check == 0)
        {
            result = false;
        }








        POPUP_END.SetActive(true);
        if (result)
        {

            Reward();

            POPUP_Victroy.SetActive(true);
            POPUP_Game_over.SetActive(false);








            PlayerData.StageClear_Function(Enemy_Manager.ID - 1);
            

        }
        else
        {


            POPUP_Victroy.SetActive(false);
            POPUP_Game_over.SetActive(true);
            Reward_Text[0].text = "" + 0;
            Reward_Text[1].text = "" + 0;
            Reward_Text[2].text = "" + 0;

        }
        LoadingTimer += 100000;
        
        Level_Text.text = "" + FireBaseDB.instacne.Player_Data_instacne.Level;
        EXP_Text.text = "" + FireBaseDB.instacne.Player_Data_instacne + "/" + 200;
        EXP_Text.transform.parent.GetChild(0).GetComponent<Slider>().value = (FireBaseDB.instacne.Player_Data_instacne.Exp * 1f) / 200;
        

    }

    public void Reward()
    {
        int Reward_Gold;
        int Reward_Ticket;
        int Reward_Gem;
        //if초회승리일경우
        if (FireBaseDB.instacne.Player_Data_instacne.StageClear[Enemy_Manager.ID - 1] == 1)
        {
            Reward_Gold = 10000;
            Reward_Ticket = 10;
            Reward_Gem = 1000;
        }

        //2번째클리어부터
        else
        {
            Reward_Gold = Random.Range(0, 100);
            if (Reward_Gold > 70)
            {
                Reward_Gold = 1500 + Enemy_Manager.ID * 100;
            }
            else if (Reward_Gold > 30)
            {
                Reward_Gold = 1000 + Enemy_Manager.ID * 50;
            }
            else
            {
                Reward_Gold = 500 + Enemy_Manager.ID * 50;
            }


            Reward_Ticket = Random.Range(0, 100);
            if (Reward_Ticket > 80)
            {
                Reward_Ticket = (Enemy_Manager.ID / 4) + 1;
            }
            else
            {
                Reward_Ticket = 0;
            }


            Reward_Gem = Random.Range(0, 100);
            if (Reward_Gem > 90)
            {
                Reward_Gem = 50;
            }
            else
            {
                Reward_Gem = 0;
            }
        }






        Reward_Text[0].text = "" + Reward_Gold;
        Reward_Text[1].text = "" + Reward_Ticket;
        Reward_Text[2].text = "" + Reward_Gem;

        FireBaseDB.instacne.Player_Data_instacne.Gold += Reward_Gold;
        FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += Reward_Ticket;
        FireBaseDB.instacne.Player_Data_instacne.Gem += Reward_Gem;

        FireBaseDB.instacne.Player_Data_instacne.Exp += (10 + Enemy_Manager.ID);
        FireBaseDB.instacne.Download_Data(StoreTYPE.GOLD);

        FireBaseDB.instacne.Download_Data(StoreTYPE.PACK);

        FireBaseDB.instacne.Download_Data(StoreTYPE.GEM);
        FireBaseDB.instacne.Download_Data(StoreTYPE.EXP);




    }


    [Header("초기화용")]
    public Image FadeImage;
    private void OnDisable()
    {
        POPUP_END.transform.position = OriginPos;
        FadeImage.raycastTarget = false;
        Color color = FadeImage.color;
        color.a = 0;
        FadeImage.color = color;
    }
    [Header("씬이동용")]
    public Camera MainCamera;
    public GameObject GameScene;
    public void MoveMainScene()
    {
        MainCamera.enabled = true;
        GameObject.Find("Main Camera").GetComponent<ManagementMainUI>().UpdateDate();
        GameScene.SetActive(false);

    }
}
public enum Card_Result
{
    Success,
    Cost_lack,
    Duplication,
    Char_Die,
    Wrong_Target,
    CantCharge_Cost,
    CantDrew,
    RequireMent,
    Reset

}


/*
    public bool Use_MY_Effect(SpellCard Selected, bool Skill = false)//스킬쓸땐객체가없으므로 true로
    {

        if (Selected.Cost > My_Cost)//코스트모자람
        {
            POPUP_1.View_Text("코스트가 부족합니다.");
            return false;
        }
        else if (My_MAXCost == 10 & Selected.Value_MAXCost > 0)
        {
            if (MYDeck.DeckCards.Count < 1)
            {
                POPUP_1.View_Text("뽑을 수 있는 카드가 없습니다");
                return false;
            }
        }
        else if ((My_Cost == My_MAXCost) & Selected.Value_Cost > 0)//코스트회복못하는상황
        {
            if (!(My_Cost - SelectedCard.Cost <= My_MAXCost))
            {
                POPUP_1.View_Text("이미 사용할 수 있는 코스트가  최대입니다");
                return false;
            }
        }
        else if ((Selected.Value_Create_Deck > 0) & (MYDeck.MaxCount <= MYDeck.DeckCards.Count))//덱에 1장도만들수없는매수라면
        {
            POPUP_1.View_Text("덱이 이미 최대입니다");
            return false;
        }
        else if ((MYDeck.DeckCards.Count < 1) & (Selected.Value_Drew > 0))
        {
            POPUP_1.View_Text("뽑을 수 있는 카드가 없습니다");
            return false;
        }
        else if (Hand.CurrentHand < Selected.Value_Hand_Less)
        {
            if (Selected.Value_Hand_Less < 10)
            {
                POPUP_1.View_Text("버릴수 있는 패가모자랍니다");

                return false;
            }
            //10이상이면 패전부버리는효과
        }
        else if (Selected.Value_Hand_Less > 10 & MYDeck.DeckCards.Count < 1)
        {
            POPUP_1.View_Text("뽑을 수 있는 카드가 없습니다");
            return false;
        }

        //판단^
        //사용가능한상태라면 코스트를소모하고 적용
        //Debug.Log("사용");
        CostUse(Selected.Cost);
        //아니라면
        //내가피해를봄
        //패버림

        //드로우
        //맥스코스트회복
        //코스트회복

        //덱만드는순으로

        



        if (Selected.Value_Char_Damage > 0)
        {
            //Debug.Log("데미지받기작동");
            Damaged(Selected.Value_Char_Damage, Selected.Value_Char_Effect_Num);


        }
        if (Selected.Value_Hand_Less > 10)
        {
            HandLess_All(Selected);
        }

        else if (Selected.Value_Hand_Less > 0)
        {
            //Debug.Log("패버림작동");
            HandLess(Selected.Value_Hand_Less);
        }
        if (Selected.Value_Drew > 0)
        {
            //Debug.Log("드로우작동");
            ExtraDrew(Selected.Value_Drew);
        }
        if (Selected.Value_MAXCost > 0)
        {
            //Debug.Log("최대코스트증가작동");
            Add_MAX_Cost(Selected.Value_MAXCost);
        }
        if (Selected.Value_Cost > 0)
        {
            //Debug.Log("코스트회복작동");
            Add_Cost(Selected.Value_Cost);
        }

        if (Selected.Value_Enemy_Damage > 0)
        {
            //데미지주기
            //Enemy_Manager.Magic_Attack(Selected.Value_Enemy_Damage, Selected.Value_Enemy_Damage_Effect);

        }


        if (Selected.Value_Create_Deck > 0)
        {
            //Debug.Log("덱생성");
            //CreateDeck(Selected.Value_Create_Deck, Selected.Value_Create_Deck1, Selected.Value_Create_Deck2, Selected.Value_Create_Deck3, Selected.Value_Create_Deck4);
            
        }

    
            
            
        
        else
        {
            DelayUseSpell = 0.5f;
        }

        if (Skill == false)
        {
            Hand.UseCard(Selected);
        }










        CardAlignment();
        return true;
    }

    


    bool CreateDeck(int SV, int SV1, int SV2, int SV3, int SV4)
    {
        List<int> Init_List = new List<int>();
        List<int> Special_Init_List = new List<int>();
        CardScriptable DataBox = GameObject.Find("CardData").GetComponent<CardData>().CardDataFile; //스펠카드데이터를가져옴
        int v = SV;//총만들카드갯수
        int v_1 = SV1;//만들어야하는1성카드갯수
        int v_2 = SV2;//만들어야하는2성카드갯수
        int v_3 = SV3;//만들어야하는3성카드갯수
        int v_4 = SV4;//만들어야하는4성카드아이디
        int Case_num;//랜덤으로 제작될 카드의랭크

        //Debug.Log(SV + "총만들카드");
        //Debug.Log(MYDeck.MaxCount + "덱최대갯수");
        //Debug.Log("스펠4" + v_4);
        while (v >= 1)//만들카수가 1이상일경우
        {
            //Debug.Log("제작할남은카드갯수"+v);

            Case_num = Random.Range(0, 4);
            //Debug.Log("제작될랭크:"+Case_num);
            switch (Case_num)
            {
                case 0:
                    if (v_1 >= 1)//만들1성갯수가 1이상이면 만들총갯수와 1성갯수를 1씩줄이고 1성카드제작
                    {
                        v_1--;
                        v--;
                        InitCard_NUM(DataBox, 1, Init_List);
                    }
                    break;
                case 1:
                    if (v_2 >= 1)//만들2성갯수가 1이상이면 만들총갯수와 2성갯수를 1씩줄이고 1성카드제작
                    {
                        v_2--;
                        v--;
                        InitCard_NUM(DataBox, 2, Init_List);
                    }
                    break;
                case 2:
                    if (v_3 >= 1)//만들3성갯수가 1이상이면 만들총갯수와 3성갯수를 1씩줄이고 1성카드제작
                    {
                        v_3--;
                        v--;
                        InitCard_NUM(DataBox, 3, Init_List);
                    }
                    break;
                case 3:
                    if (v >= 1 & v_4>=1)//만들4성갯수가 1이상이면 만들총갯수와 4성갯수를 1씩줄이고 1성카드제작
                    {
                        
                        InitCard_NUM(DataBox, 4, Special_Init_List, v_4);
                        v--;

                    }
                    break;
            }


        }
        MYDeck.Play_Particle();
        int c = 0;
        for (int j = 0; j < Init_List.Count; j++)
        {
            if (MYDeck.DeckCards.Count < MYDeck.MaxCount)
            {

                if (j == Init_List.Count - 1)
                {
                   MYDeck.Function_Stop_Particle(j);
                   ///MYDeck.Stop_Particle(j);

                }
                c++;

                MYDeck.Function_Stop_Particle(j);
                MYDeck.Extra_InitCard(Init_List[j], j);
                DelayUseSpell += 0.1f;
            }
            else
            {
                MYDeck.Function_Stop_Particle(j);
                c++;
                MYDeck.ShuffleDeck();
                return true;
            }
        }
        for (int j = 0; j < Special_Init_List.Count; j++)
        {
            if (MYDeck.DeckCards.Count < MYDeck.MaxCount)
            {

                if (j == Special_Init_List.Count - 1)
                {
                    //MYDeck.Stop_Particle(j);
                    MYDeck.Function_Stop_Particle(j);
                }

                MYDeck.Function_Stop_Particle(j);
                MYDeck.Special_InitCard(Special_Init_List[j], c++);
                DelayUseSpell += 0.1f;
            }
            else
            {
                MYDeck.Function_Stop_Particle(j);

                MYDeck.ShuffleDeck();
                return true;
            }
        }


        MYDeck.ShuffleDeck();
        return true;


    }
    */

/*
///////////////////////////////////////////////////////////////////////////////////내캐릭터에게적용
if (Check && SelectedCard.CardType == 1)//내 캐릭터에게 작용하는 스펠
{

}

///////////////////////////////////////////////////////////////////////////////////적에게게적용
else if (Check && SelectedCard.CardType == 0)//적에게
{
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform.TryGetComponent<EnemyManager>(out var Temp_Enemy))
            {

                foreach (Effect effect in e)
                {
                    effect.Effect_Enemy_Function(this);
                    effect.Damage_Enemy_Function(Temp_Enemy);
                }
                break;
            }
        }

}
///////////////////////////////////////////////////////////////////////////////////광역으로 캐릭터에게적용
else if (Check && SelectedCard.CardType == 2)//광역
{

    int Check_Live_USE = 0;//효과를이미적용받을수
    int LiveCount = 0;//살아있는 캐릭터수
    for (int i = 0; i < Char_Manager.CombatChar.Count; i++)
    {
        if (Char_Manager.CombatChar[i].Live)
        {
            LiveCount++;
            foreach (SpellEffect SE in Char_Manager.CombatChar[i].Spell_Effects)
            {
                if (SE.ID == SelectedCard.CardNumber)
                {

                    Check_Live_USE++;
                }
            }

        }

    }


    if (Check_Live_USE== LiveCount)//이미 모두받은상태
    {
        _Result = Card_Result.Duplication;
    }
    else//이미적용받은캐릭터보다 생존캐릭터가많다면 =적용받지않는캐릭터가있다는뜻
    {






    }







}
///////////////////////////////////////////////////////////////////////////////////플레이어에게적용
else if (Check && SelectedCard.CardType == 3)//플레이어에게
{



    foreach (Effect effect in e)
    {
        effect.Effect_Function(this);

    }

    //Debug.Log("3번타입탐지");
    //Result = Use_MY_Effect(SelectedCard);
}

///////////////////////////////////////////////////////////////////////////////////단일캐릭터확정피해   광역효과
else if (Check && SelectedCard.CardType == 4)
{


    int Check_Live_USE = 0;//효과를이미적용받을수

    for (int i = 0; i < Char_Manager.CombatChar.Count; i++)
    {
        if (Char_Manager.CombatChar[i].Live)
        {

            foreach (SpellEffect SE in Char_Manager.CombatChar[i].Spell_Effects)
            {
                if (SE.ID == SelectedCard.CardNumber)
                {
                    //Debug.Log("이미적용받음");
                    Check_Live_USE++;

                }
            }

        }

    }
    //Debug.Log("적용받지않은인원:" + Check_Live_USE);
    int LiveCount = 0;
    for (int i = 0; i < Char_Manager.CombatChar.Count; i++)//생존해있느캐릭터수체크
    {
        if (Char_Manager.CombatChar[i].Live)
        {
            LiveCount++;
        }
    }

    bool CostChecker = false;
    if (My_Cost >= SelectedCard.Cost)
    {
        CostChecker = true;
    }


    bool LiveCheker = false;


    for (int i = 0; i < hits.Length; i++)
    {
        if (CostChecker == false | (Check_Live_USE == LiveCount))
        {
            break;
        }
        //Debug.Log("확인된레이캐스트" + hits[i].collider.name);
        if (hits[i].transform.TryGetComponent<GameCard>(out var Temp_MY))
        {
            Target_Solo = Temp_MY;
            Temp_MY.Spell_Damaged(SelectedCard.Value_Char_Damage, SelectedCard.Value_Char_Effect_Num);
            LiveCheker = true;


        }
    }





    if (Check_Live_USE < LiveCount & LiveCheker & CostChecker)//이미적용받은캐릭터보다 생존캐릭터가많다면 =적용받지않는캐릭터가있다는뜻
    {

        for (int i = 0; i < Char_Manager.CombatChar.Count; i++)
        {
            if (Char_Manager.CombatChar[i].Live)
            {
                bool check = false;
                foreach (SpellEffect SE in Char_Manager.CombatChar[i].Spell_Effects)
                {
                    if (SE.ID == SelectedCard.CardNumber)
                    {
                        //Debug.Log("이미적용받음");
                        check = true;

                    }
                }


                if (!check)

                {

                    Char_Manager.CombatChar[i].UsingSpell(SelectedCard);
                    //Char_Manager.CombatChar[i].ResetAttackCount();
                }
            }

        }
        if (SelectedCard.Value_Enemy_Damage > 0)
        {
            //Enemy_Manager.Magic_Attack(SelectedCard.Value_Enemy_Damage, SelectedCard.Value_Enemy_Damage_Effect);
        }


        My_Cost -= SelectedCard.Cost;

        Hand.UseCard(SelectedCard);
        EnLargeObject.SetActive(false);
        CardAlignment();
        Result = true;

    }
    else
    {
        if (CostChecker == true)
        {
            if (SelectedCard.Value_Enemy_Damage > 0)
            {
                //Enemy_Manager.Magic_Attack(SelectedCard.Value_Enemy_Damage, SelectedCard.Value_Enemy_Damage_Effect);
                Hand.UseCard(SelectedCard);
                EnLargeObject.SetActive(false);
                CardAlignment();
                Result = true;
                Debug.Log("데미지만 적용");

            }
            else
            {
                POPUP_1.View_Text("이미 모두 적용된 효과입니다");
            }
        }
        else
        {
            POPUP_1.View_Text("코스트가 모자랍니다.");
        }


        //Debug.Log("이미모두가적용된효과");
    }



}
///////////////////////////////////////////////////////////////////////////////////단일캐릭터확정피해   광역효과
else if (Check && SelectedCard.CardType == 5)
{

    bool CostChecker = false;
    if (My_Cost >= SelectedCard.Cost)
    {
        CostChecker = true;
    }





    for (int i = 0; i < hits.Length; i++)
    {
        if (CostChecker == false)
        {
            break;
        }
        //Debug.Log("확인된레이캐스트" + hits[i].collider.name);
        if (hits[i].transform.TryGetComponent<GameCard>(out var Temp_MY))
        {


            if (SelectedCard.Cost > My_Cost)//코스트모자람
            {
                POPUP_1.View_Text("코스트가 부족합니다.");
                break;
            }
            else if (My_MAXCost == 10 & SelectedCard.Value_MAXCost > 0)
            {
                if (MYDeck.DeckCards.Count < 1)
                {
                    POPUP_1.View_Text("뽑을 수 있는 카드가 없습니다");
                    break;
                }
            }
            else if ((My_Cost == My_MAXCost) & SelectedCard.Value_Cost > 0)//코스트회복못하는상황
            {
                if (!(My_Cost - SelectedCard.Cost <= My_MAXCost))
                {
                    POPUP_1.View_Text("이미 사용할 수 있는 코스트가  최대입니다");
                    break;
                }
            }
            else if ((SelectedCard.Value_Create_Deck > 0) & (MYDeck.MaxCount <= MYDeck.DeckCards.Count))//덱에 1장도만들수없는매수라면
            {
                POPUP_1.View_Text("덱이 이미 최대입니다");
                break;
            }
            else if ((MYDeck.DeckCards.Count < 1) & (SelectedCard.Value_Drew > 0))
            {
                POPUP_1.View_Text("뽑을 수 있는 카드가 없습니다");
                break;
            }
            else if (Hand.CurrentHand < SelectedCard.Value_Hand_Less)
            {
                if (SelectedCard.Value_Hand_Less < 10)
                {
                    POPUP_1.View_Text("버릴수 있는 패가모자랍니다");

                    break;
                }
                //10이상이면 패전부버리는효과
            }
            else if (SelectedCard.Value_Hand_Less > 10 & MYDeck.DeckCards.Count < 1)
            {
                POPUP_1.View_Text("뽑을 수 있는 카드가 없습니다");
                break;
            }










            Temp_MY.Spell_Damaged(SelectedCard.Value_Char_Damage, SelectedCard.Value_Char_Effect_Num);
            SelectedCard.Value_Char_Damage = 0;
            Result = Use_MY_Effect(SelectedCard);
            break;

        }
    }



}
*/

/*
void Damaged(int Value_Damaged, int Effect)
{
    int Ran = Random.Range(0, Char_Manager.CombatChar.Count);

    while (true)
    {
        if (Char_Manager.CombatChar[Ran].Live == true)
        {
            break;
        }
        else
            Ran = Random.Range(0, 4);
    }
    Char_Manager.CombatChar[Ran].Spell_Damaged(Value_Damaged, Effect);





}


void ExtraDrew(int Value_Drew)
{
  //  ExtraDrew2(Value_Drew);
}
void ExtraDrew2(int Value_Drew)
{
    for (int i = 0; i < Value_Drew; i++)
    {
        if (MYDeck.Drew())
        {
            CardAlignment();
        }
        else
        {
            break;
        }

    }

    CardAlignment();

}
void HandLess(int Less)
{
    if (Less >= Hand.CurrentHand)
    {
        Less = Hand.CurrentHand;
    }

    for (int i = 0; i < Less; i++)
    {
        int Ran = Random.Range(0, Hand.CurrentHand);
        Hand.ThrowCard(Hand.Cards[Ran]);

    }
    CardAlignment();

}
void HandLess_All(SpellCard spell)
{
    if (spell.Value_Hand_Less >= Hand.CurrentHand)
    {
        spell.Value_Hand_Less = Hand.CurrentHand;
    }


    for (int i = 0; i < spell.Value_Hand_Less; i++)
    {
        int Ran = Random.Range(0, Hand.CurrentHand);
        Hand.ThrowCard(Hand.Cards[Ran]);
        spell.Value_Drew++;

    }



    CardAlignment();

}

void Add_Cost(int COST)
{


    My_Cost += COST;
    if (My_Cost >= My_MAXCost)
    {
        My_Cost = My_MAXCost;
    }
    ViewCost();





}
void Add_MAX_Cost(int MAXCost)
{
    if (My_MAXCost == 10 & MYDeck.DeckCards.Count >= 1)//코스트최대 덱에카드가1장이상
    {

        int Drew_Value = MAXCost;
        //ExtraDrew2(Drew_Value);






    }
    else
    {


        My_MAXCost += MAXCost;
        if (My_MAXCost > 10)
        {
            My_MAXCost = 10;
        }
        ViewCost();



    }
    CardAlignment();

}
*/