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

    public float Height = 0.5f;//���������ִ����
    public float Size = 0.1f;//�п��ִ� ī�� ũ��
    public StageUI Stage_UI;
    [Header("��������")]
    public PlayerInfos PlayerData;
    public Deck MYDeck;
    public CardHand Hand;
    public CharManager Char_Manager;
    public EnemyManager Enemy_Manager;

    [Header("��������")]
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

    float CardDelay = 0.5f;//ī��̱������
    int StartDrew = 5;
    public bool onMyCardArea;
    public TextMeshProUGUI AttackOrder;

    CardState state;


    [Header("Ȯ������")]
    public GameObject EnLargeObject;
    public Image Large_Main;
    public TextMeshProUGUI Large_Cost;
    public TextMeshProUGUI Large_Name;
    public TextMeshProUGUI Large_Exp;


    public FailWinodw POPUP_1;

    [SerializeField]
    GameObject Setting;

    [Header("��������")]
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

    


    [Header("����ī�� ���� ����")]
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
        AttackOrder.text = "�����غ�Ϸ�";
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
            AttackOrder.text = "�����غ�Ϸ�";
            Setting.SetActive(false);

            StartCoroutine(Char_Manager.MonsterCardReset());

            Enemy_Manager.Restart();
        }
        else
        {
            POPUP_1.View_Text("�ൿ�� ���� �׺��� �� �����ϴ�.");
        }

    }


    public void Enlarge(bool state)
    {
        EnLargeObject.SetActive(state);
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.Space))//ī������
        {
            CardAlignment();
        }
        if (Input.GetKeyDown(KeyCode.LeftShift))//ī�����
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
    IEnumerator SwapCorountine()//�ϳѰ��ֱ� �����ڷ�ƾ
    {
        Turn = !Turn;
        if (Turn)//���Ͻ���
        {
            LoadingTimer = 0;
            AttackOrder.text = "�����غ�Ϸ�";
            if (My_MAXCost <= 9)//�ִ��ڽ�Ʈ 9���϶�� 1����
            { My_MAXCost += 1; }
            My_Cost = My_MAXCost;//�ڽ�Ʈȸ��
            ViewCost();//�ڽ�Ʈ����
            Char_Manager.TurnEnd();// ������� ������ִ� ȿ���� ��1���Ҹ�
            Enemy_Manager.TurnEnd();
            DrewCount = Char_Manager.MAX_Drew_Count();
            LoadingTimer += DrewCount * 0.5f + 0.1f;//��ο�ż���ŭ ������
            for (int i = 0; i < DrewCount; i++)//��ο�����
            {
                if (MYDeck.Drew())//��ο찡���ϸ� ��ο��ϰ�������
                {
                    CardAlignment();
                }
            }
        }
        else//���� ���� ���ݽ��� 
        {   
            LoadingTimer = 100;
            Char_Manager.Renge_HP_Effect();//ü����������۵�

            for (int i = 0; i < Char_Manager.CombatChar.Count; i++)//����ִٸ� ����Ƚ�� ����
            {
                if (Char_Manager.CombatChar[i].Current_HP > 0)
                {
                    Char_Manager.CombatChar[i].ResetAttackCount();
                }
            }
            yield return new WaitForSeconds(1f);
            AttackOrder.text = "������";
            GameCard temp;
            for (int i = 0; i < Char_Manager.CombatChar.Count; i++)
            {
                temp = Char_Manager.MonsterCard.transform.GetChild(i).GetComponent<GameCard>();
                if (Enemy_Manager.Live == true)
                {
                    while (temp.AttackCount >= 1 && temp.Live)//����Ƚ���� �ټҸ��Ҷ����� ����
                    {
                        if (Enemy_Manager.Live == false)//���Ӱ����߻����
                        {
                            break;
                        }
                        //Debug.Log(temp.name + "�� ������ /" + (temp.AttackCount - 1) + "�� ����");
                        temp.animator.SetTrigger("Attack");
                        yield return new WaitForSeconds(1.5f);
                    }
                }
                else//���� ���� ���� ����ϸ� �ڷ�ƾ����
                {
                    yield break;
                }
            }
            AttackOrder.text = "�����";
            Enemy_Manager.StartEnemyTurn();
        }
    }










    public void Button_Swap()//�������� �Լ��θ���
    {
        if (LoadingTimer <= 0 & Enemy_Manager.Live)
        {
            LoadingTimer += 3.0f;
            SwapTurn();
        }
    }



    public void SwapTurn()//�ϳѰ��ִ��Լ�
    {



        StartCoroutine(SwapCorountine());



    }
    public void ViewCost()//�ڽ�Ʈ�̹�������
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






    IEnumerator CardAlignmentCoroutine()//ī�������߿��� ��������
    {

        LoadingTimer = CardDelay;
        yield return new WaitForSeconds(CardDelay);


    }

    public void CardAlignment()//���� ī������
    {
        List<PRS> orginCardRPSs = new List<PRS>();
        orginCardRPSs = RoundAlignment(Hand.myCardLeft, Hand.myCardRight, Hand.CurrentHand, Height, Vector3.one * Size);//ī��ũ������
        for (int i = 0; i < Hand.CurrentHand; i++)
        {
            var target = Hand.Cards[i];
            target.GetComponent<SpellCard>().originPosi = orginCardRPSs[i];
            target.GetComponent<SpellCard>().MoveTransForm(target.GetComponent<SpellCard>().originPosi, true, CardDelay);
            StartCoroutine(CardAlignmentCoroutine());
        }
    }

    List<PRS> RoundAlignment(Transform leftTr, Transform rightTr, int objCount, float height, Vector3 scale)//ī�� �����������������ִ��Լ�
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
    IEnumerator FirstDrew()//���ӽ��۽� 5���ο��ϴ��Լ�
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
    /// ���콺�� �����ϴ� �޼ҵ�
    //////////////////////////////////////////////////////////////////////////////////


    void CardDrag()//ī��巡���Լ�
    {
        if (!onMyCardArea)
        {
            Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            MousePos.z = -10f;
            SelectedCard.MoveTransForm(new PRS(MousePos, Quaternion.identity, SelectedCard.originPosi.scale), false);
        }
    }

    void DetectCardArea()//������������ üũ
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = -10f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("HandAreaCheck");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard(bool Enlarge, SpellCard Card)//ī�尮�ٴ�� Ȯ���ϴ��Լ�
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

    public void CardMouseOver(SpellCard Card)//ī�带���콺�� ���ٴ���۵��ϴ��Լ�
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

    public void DeckMouseOver(Canvas Count)//������ ���콺���÷� ���ż���Ȯ���ϴ��Լ�
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
    /// ����ȿ�������ϴ�
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
        switch (SelectedCard.CardType)//ī��Ÿ��üũ�ϰ� ������ ���̾����
        {
            case 0:
                //Debug.Log("�����԰���");
                layer = LayerMask.NameToLayer("ENEMY");
                break;
            case 1:
                //Debug.Log("������ī��÷�����");
                layer = LayerMask.NameToLayer("MYCHAR");
                break;
            case 2:
                //Debug.Log("������");
                layer = LayerMask.NameToLayer("MYAREA");
                break;
            case 3:
                //Debug.Log("�÷��̾��");
                layer = LayerMask.NameToLayer("MYAREA");
                break;
            case 4:
                layer = LayerMask.NameToLayer("MYCHAR");
                //�������׾��� ����ȿ������
                break;
            case 5:
                layer = LayerMask.NameToLayer("MYCHAR");
                //�������׾��� �÷��̾�����
                break;
            default:
                layer = 0;
                break;

        }
        Check = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
        //Debug.Log("�ݶ��̴�����=" + Check);

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
                    Debug.Log("Ȯ�εȷ���ĳ��Ʈ" + hits[i].collider.name);
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
                Reustl_Failed();//ī��ߵ������� �Ѱ��� �����ߴ¼��� ����
                yield break;
            }
        }
        if (Check)
        {
            switch (SelectedCard.CardType)
            {
                case 1:   //��ĳ���Ϳ�������
                    foreach (Effect effect in e)
                    {
                        effect.Effect_Solo_Function(this);
                    }
                    break;
                case 0://������
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
                case 2://���� �ڽ�ĳ����
                    foreach (Effect effect in e)
                    {
                        effect.Effect_Solo_Function(this);
                    }
                    break;
                case 3: //�÷��̾�� ����
                    foreach (Effect effect in e)
                    {
                        effect.Effect_Function(this);
                    }
                    break;
                case 4: //�÷��̾�� ���� ���� ĳ�����������


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
                Debug.Log("����");
                EnLargeObject.SetActive(false);
                CostUse(SelectedCard.Cost);
                Hand.UseCard(SelectedCard);
                Enlarge(false);
                CardAlignment();
                yield return new WaitForSeconds(DelayUseSpell);//����ī�带������ ī��ȿ���������ϴ�0.3�ʵ��� �巡�׹������� �ڵ岿���� ����
                break;
            case Card_Result.Duplication:
                POPUP_1.View_Text("�̹� ����� ȿ���Դϴ�");
                break;
            case Card_Result.Wrong_Target:
                POPUP_1.View_Text("�߸��� �������Դϴ�");
                break;
            case Card_Result.Char_Die:
                POPUP_1.View_Text("�̹� ����� �������Դϴ�");
                break;
            case Card_Result.Cost_lack:
                POPUP_1.View_Text("�ڽ�Ʈ�� �����մϴ�.");
                break;
            case Card_Result.CantCharge_Cost:
                POPUP_1.View_Text("�̹� ����� �� �ִ� �ڽ�Ʈ��  �ִ��Դϴ�");
                break;
            case Card_Result.CantDrew:
                POPUP_1.View_Text("���� �̹� ī�尡 �����ϴ�");
                break;
            case Card_Result.RequireMent:
                POPUP_1.View_Text("�ߵ� ������ ���� ���� �ʽ��ϴ�.");
                break;
        }
        state = CardState.Drag;
        ViewCost();
    }
    public void Reustl_Failed()//���а������ġ��
    {
        switch (_Result)
        {

            case Card_Result.Duplication:
                POPUP_1.View_Text("�̹� ����� ȿ���Դϴ�");

                break;
            case Card_Result.Wrong_Target:
                POPUP_1.View_Text("�߸��� �������Դϴ�");

                break;
            case Card_Result.Char_Die:
                POPUP_1.View_Text("�̹� ����� �������Դϴ�");

                break;
            case Card_Result.Cost_lack:
                POPUP_1.View_Text("�ڽ�Ʈ�� �����մϴ�.");

                break;
            case Card_Result.CantCharge_Cost:
                POPUP_1.View_Text("�̹� ����� �� �ִ� �ڽ�Ʈ��  �ִ��Դϴ�");
                break;
            case Card_Result.CantDrew:
                POPUP_1.View_Text("���� �̹� ī�尡 �����ϴ�");
                break;
            case Card_Result.RequireMent:
                POPUP_1.View_Text("�ߵ� ������ ���� ���� �ʽ��ϴ�.");
                break;




        }
        state = CardState.Drag;



        ViewCost();
    }
    public void InitCard_NUM(CardScriptable DataBox, int Rank, List<int> Init_List, int Special_num = 0)//ī�� �������
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
            //Debug.Log("�����ī�����");
            Init_List.Add(Special_num);
        }
    }
    public void CostUse(int UsingCost)//�ڽ�Ʈ�Ҹ�Ǵ� ��ġ Ȯ�ο�
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
        //if��ȸ�¸��ϰ��
        if (FireBaseDB.instacne.Player_Data_instacne.StageClear[Enemy_Manager.ID - 1] == 1)
        {
            Reward_Gold = 10000;
            Reward_Ticket = 10;
            Reward_Gem = 1000;
        }

        //2��°Ŭ�������
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


    [Header("�ʱ�ȭ��")]
    public Image FadeImage;
    private void OnDisable()
    {
        POPUP_END.transform.position = OriginPos;
        FadeImage.raycastTarget = false;
        Color color = FadeImage.color;
        color.a = 0;
        FadeImage.color = color;
    }
    [Header("���̵���")]
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
    public bool Use_MY_Effect(SpellCard Selected, bool Skill = false)//��ų������ü�������Ƿ� true��
    {

        if (Selected.Cost > My_Cost)//�ڽ�Ʈ���ڶ�
        {
            POPUP_1.View_Text("�ڽ�Ʈ�� �����մϴ�.");
            return false;
        }
        else if (My_MAXCost == 10 & Selected.Value_MAXCost > 0)
        {
            if (MYDeck.DeckCards.Count < 1)
            {
                POPUP_1.View_Text("���� �� �ִ� ī�尡 �����ϴ�");
                return false;
            }
        }
        else if ((My_Cost == My_MAXCost) & Selected.Value_Cost > 0)//�ڽ�Ʈȸ�����ϴ»�Ȳ
        {
            if (!(My_Cost - SelectedCard.Cost <= My_MAXCost))
            {
                POPUP_1.View_Text("�̹� ����� �� �ִ� �ڽ�Ʈ��  �ִ��Դϴ�");
                return false;
            }
        }
        else if ((Selected.Value_Create_Deck > 0) & (MYDeck.MaxCount <= MYDeck.DeckCards.Count))//���� 1�嵵��������¸ż����
        {
            POPUP_1.View_Text("���� �̹� �ִ��Դϴ�");
            return false;
        }
        else if ((MYDeck.DeckCards.Count < 1) & (Selected.Value_Drew > 0))
        {
            POPUP_1.View_Text("���� �� �ִ� ī�尡 �����ϴ�");
            return false;
        }
        else if (Hand.CurrentHand < Selected.Value_Hand_Less)
        {
            if (Selected.Value_Hand_Less < 10)
            {
                POPUP_1.View_Text("������ �ִ� �а����ڶ��ϴ�");

                return false;
            }
            //10�̻��̸� �����ι�����ȿ��
        }
        else if (Selected.Value_Hand_Less > 10 & MYDeck.DeckCards.Count < 1)
        {
            POPUP_1.View_Text("���� �� �ִ� ī�尡 �����ϴ�");
            return false;
        }

        //�Ǵ�^
        //��밡���ѻ��¶�� �ڽ�Ʈ���Ҹ��ϰ� ����
        //Debug.Log("���");
        CostUse(Selected.Cost);
        //�ƴ϶��
        //�������ظ���
        //�й���

        //��ο�
        //�ƽ��ڽ�Ʈȸ��
        //�ڽ�Ʈȸ��

        //������¼�����

        



        if (Selected.Value_Char_Damage > 0)
        {
            //Debug.Log("�������ޱ��۵�");
            Damaged(Selected.Value_Char_Damage, Selected.Value_Char_Effect_Num);


        }
        if (Selected.Value_Hand_Less > 10)
        {
            HandLess_All(Selected);
        }

        else if (Selected.Value_Hand_Less > 0)
        {
            //Debug.Log("�й����۵�");
            HandLess(Selected.Value_Hand_Less);
        }
        if (Selected.Value_Drew > 0)
        {
            //Debug.Log("��ο��۵�");
            ExtraDrew(Selected.Value_Drew);
        }
        if (Selected.Value_MAXCost > 0)
        {
            //Debug.Log("�ִ��ڽ�Ʈ�����۵�");
            Add_MAX_Cost(Selected.Value_MAXCost);
        }
        if (Selected.Value_Cost > 0)
        {
            //Debug.Log("�ڽ�Ʈȸ���۵�");
            Add_Cost(Selected.Value_Cost);
        }

        if (Selected.Value_Enemy_Damage > 0)
        {
            //�������ֱ�
            //Enemy_Manager.Magic_Attack(Selected.Value_Enemy_Damage, Selected.Value_Enemy_Damage_Effect);

        }


        if (Selected.Value_Create_Deck > 0)
        {
            //Debug.Log("������");
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
        CardScriptable DataBox = GameObject.Find("CardData").GetComponent<CardData>().CardDataFile; //����ī�嵥���͸�������
        int v = SV;//�Ѹ���ī�尹��
        int v_1 = SV1;//�������ϴ�1��ī�尹��
        int v_2 = SV2;//�������ϴ�2��ī�尹��
        int v_3 = SV3;//�������ϴ�3��ī�尹��
        int v_4 = SV4;//�������ϴ�4��ī����̵�
        int Case_num;//�������� ���۵� ī���Ƿ�ũ

        //Debug.Log(SV + "�Ѹ���ī��");
        //Debug.Log(MYDeck.MaxCount + "���ִ밹��");
        //Debug.Log("����4" + v_4);
        while (v >= 1)//����ī���� 1�̻��ϰ��
        {
            //Debug.Log("�����ҳ���ī�尹��"+v);

            Case_num = Random.Range(0, 4);
            //Debug.Log("���۵ɷ�ũ:"+Case_num);
            switch (Case_num)
            {
                case 0:
                    if (v_1 >= 1)//����1�������� 1�̻��̸� �����Ѱ����� 1�������� 1�����̰� 1��ī������
                    {
                        v_1--;
                        v--;
                        InitCard_NUM(DataBox, 1, Init_List);
                    }
                    break;
                case 1:
                    if (v_2 >= 1)//����2�������� 1�̻��̸� �����Ѱ����� 2�������� 1�����̰� 1��ī������
                    {
                        v_2--;
                        v--;
                        InitCard_NUM(DataBox, 2, Init_List);
                    }
                    break;
                case 2:
                    if (v_3 >= 1)//����3�������� 1�̻��̸� �����Ѱ����� 3�������� 1�����̰� 1��ī������
                    {
                        v_3--;
                        v--;
                        InitCard_NUM(DataBox, 3, Init_List);
                    }
                    break;
                case 3:
                    if (v >= 1 & v_4>=1)//����4�������� 1�̻��̸� �����Ѱ����� 4�������� 1�����̰� 1��ī������
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
///////////////////////////////////////////////////////////////////////////////////��ĳ���Ϳ�������
if (Check && SelectedCard.CardType == 1)//�� ĳ���Ϳ��� �ۿ��ϴ� ����
{

}

///////////////////////////////////////////////////////////////////////////////////�����԰�����
else if (Check && SelectedCard.CardType == 0)//������
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
///////////////////////////////////////////////////////////////////////////////////�������� ĳ���Ϳ�������
else if (Check && SelectedCard.CardType == 2)//����
{

    int Check_Live_USE = 0;//ȿ�����̹����������
    int LiveCount = 0;//����ִ� ĳ���ͼ�
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


    if (Check_Live_USE== LiveCount)//�̹� ��ι�������
    {
        _Result = Card_Result.Duplication;
    }
    else//�̹��������ĳ���ͺ��� ����ĳ���Ͱ����ٸ� =��������ʴ�ĳ���Ͱ��ִٴ¶�
    {






    }







}
///////////////////////////////////////////////////////////////////////////////////�÷��̾������
else if (Check && SelectedCard.CardType == 3)//�÷��̾��
{



    foreach (Effect effect in e)
    {
        effect.Effect_Function(this);

    }

    //Debug.Log("3��Ÿ��Ž��");
    //Result = Use_MY_Effect(SelectedCard);
}

///////////////////////////////////////////////////////////////////////////////////����ĳ����Ȯ������   ����ȿ��
else if (Check && SelectedCard.CardType == 4)
{


    int Check_Live_USE = 0;//ȿ�����̹����������

    for (int i = 0; i < Char_Manager.CombatChar.Count; i++)
    {
        if (Char_Manager.CombatChar[i].Live)
        {

            foreach (SpellEffect SE in Char_Manager.CombatChar[i].Spell_Effects)
            {
                if (SE.ID == SelectedCard.CardNumber)
                {
                    //Debug.Log("�̹��������");
                    Check_Live_USE++;

                }
            }

        }

    }
    //Debug.Log("������������ο�:" + Check_Live_USE);
    int LiveCount = 0;
    for (int i = 0; i < Char_Manager.CombatChar.Count; i++)//�������ִ�ĳ���ͼ�üũ
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
        //Debug.Log("Ȯ�εȷ���ĳ��Ʈ" + hits[i].collider.name);
        if (hits[i].transform.TryGetComponent<GameCard>(out var Temp_MY))
        {
            Target_Solo = Temp_MY;
            Temp_MY.Spell_Damaged(SelectedCard.Value_Char_Damage, SelectedCard.Value_Char_Effect_Num);
            LiveCheker = true;


        }
    }





    if (Check_Live_USE < LiveCount & LiveCheker & CostChecker)//�̹��������ĳ���ͺ��� ����ĳ���Ͱ����ٸ� =��������ʴ�ĳ���Ͱ��ִٴ¶�
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
                        //Debug.Log("�̹��������");
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
                Debug.Log("�������� ����");

            }
            else
            {
                POPUP_1.View_Text("�̹� ��� ����� ȿ���Դϴ�");
            }
        }
        else
        {
            POPUP_1.View_Text("�ڽ�Ʈ�� ���ڶ��ϴ�.");
        }


        //Debug.Log("�̸̹�ΰ������ȿ��");
    }



}
///////////////////////////////////////////////////////////////////////////////////����ĳ����Ȯ������   ����ȿ��
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
        //Debug.Log("Ȯ�εȷ���ĳ��Ʈ" + hits[i].collider.name);
        if (hits[i].transform.TryGetComponent<GameCard>(out var Temp_MY))
        {


            if (SelectedCard.Cost > My_Cost)//�ڽ�Ʈ���ڶ�
            {
                POPUP_1.View_Text("�ڽ�Ʈ�� �����մϴ�.");
                break;
            }
            else if (My_MAXCost == 10 & SelectedCard.Value_MAXCost > 0)
            {
                if (MYDeck.DeckCards.Count < 1)
                {
                    POPUP_1.View_Text("���� �� �ִ� ī�尡 �����ϴ�");
                    break;
                }
            }
            else if ((My_Cost == My_MAXCost) & SelectedCard.Value_Cost > 0)//�ڽ�Ʈȸ�����ϴ»�Ȳ
            {
                if (!(My_Cost - SelectedCard.Cost <= My_MAXCost))
                {
                    POPUP_1.View_Text("�̹� ����� �� �ִ� �ڽ�Ʈ��  �ִ��Դϴ�");
                    break;
                }
            }
            else if ((SelectedCard.Value_Create_Deck > 0) & (MYDeck.MaxCount <= MYDeck.DeckCards.Count))//���� 1�嵵��������¸ż����
            {
                POPUP_1.View_Text("���� �̹� �ִ��Դϴ�");
                break;
            }
            else if ((MYDeck.DeckCards.Count < 1) & (SelectedCard.Value_Drew > 0))
            {
                POPUP_1.View_Text("���� �� �ִ� ī�尡 �����ϴ�");
                break;
            }
            else if (Hand.CurrentHand < SelectedCard.Value_Hand_Less)
            {
                if (SelectedCard.Value_Hand_Less < 10)
                {
                    POPUP_1.View_Text("������ �ִ� �а����ڶ��ϴ�");

                    break;
                }
                //10�̻��̸� �����ι�����ȿ��
            }
            else if (SelectedCard.Value_Hand_Less > 10 & MYDeck.DeckCards.Count < 1)
            {
                POPUP_1.View_Text("���� �� �ִ� ī�尡 �����ϴ�");
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
    if (My_MAXCost == 10 & MYDeck.DeckCards.Count >= 1)//�ڽ�Ʈ�ִ� ����ī�尡1���̻�
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