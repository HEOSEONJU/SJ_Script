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

    public Deck MYDeck;//�÷��̾��� ��
    public CardHand Hand;//�÷��̾��� ��
    public CharManager Char_Manager;//��Ʋ�� ������ ĳ���� �Ŵ���
    public EnemyManager Enemy_Manager;//��Ʋ����� �Ŵ���

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
    enum CardState { Not, Over, Drag }//�ƹ��͵��������ϴ»���, Ȯ�븸 ������ ����,�巡�ױ��������ѻ���;
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
    public void Restart()//������ �ʱ� ��Ȳ���� ������ ������ϴ� ���
    {
        if (state == CardState.Drag)//�巡�װ����ѻ��´� ���Ͽ� �ൿ�� �� ���ִ»���
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
        else//�ൿ�� �� �� ���� �ܰ� �����,������
        {
            POPUP_1.View_Text("�ൿ�� ���� �׺��� �� �����ϴ�.");
        }

    }


    public void Enlarge(bool state)//ī�� Ȯ���ϴ� ���
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

        if (LoadingTimer >= 0)//�ε�Ÿ�̸Ӱ��ִµ��ȿ��� ������������
        {
            LoadingTimer -= Time.deltaTime;
            state = CardState.Not;
        }
        else
        {
            state = CardState.Drag;
        }
    }
    IEnumerator SwapCorountine()//���ൿ�� �����ϰ� ĳ���͵��� ��� ���������� ��뿡�� ���� �Ѱ��ִ� ���
                                //���� ������ ��밡 ������ ���� �Ѱ��ٶ� �� ���۽� �۵��ؾ��� ��� �۵�
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

    void DetectCardArea()//�巡���� ī�尡 ������ üũ�ϴ� �Լ�
    {
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        MousePos.z = -10f;
        RaycastHit2D[] hits = Physics2D.RaycastAll(MousePos, Vector3.forward);
        int layer = LayerMask.NameToLayer("HandAreaCheck");
        onMyCardArea = Array.Exists(hits, x => x.collider.gameObject.layer == layer);
    }

    void EnlargeCard(bool Enlarge, SpellCard Card)//������ ī���� ������ Ȯ��ī�忡 �����ϴ� �Լ�
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

    public void CardMouseExit(SpellCard Card) //ī�忡�� ���콺�� ������ �۵��ϴ� �Լ�
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

    public void DeckMouseExit(Canvas Count) //������  ���콺�� ������ �۵��ϴ� �Լ�
    {
        Count.gameObject.SetActive(false);
    }

    public void CardMouseDown()//�巡���� ������ Ȯ���ϴ� �Լ�
    {
        if (state == CardState.Not)
            return;
        Drag = true;
    }

    public void CardMouseUp()//�巡���� ���Ḧ Ȯ���ϴ� �Լ�
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
    IEnumerator Card_Effect()//�������� ����ī�尡 �ùٸ� �ߵ���ġ���� �ߵ������� �����Ͽ����� Ȯ�� ��
                             //�ߵ� ������ ��� �����Ͽ��ٸ� ����ī�尡 ���� Effect�� �ߵ� ��Ű�� �ڷ�ƾ
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
    public void InitCard_NUM(CardScriptable DataBox, int Rank, List<int> Init_List, int Special_num = 0)//�÷��̾ ���� �����ص� ī�带 �����ϴ±��
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
    public void CostUse(int UsingCost)//�ڽ�Ʈ �Ҹ� ����ϴ� �Լ�
    {
        My_Cost -= UsingCost;
        ViewCost();


    }
    public void StopManager()//������ �ߴ��ϴ� �Լ�
    {
        state = CardState.Not;
        Loading = false;
    }

    public void OpenSetting()//����â�� ���� �Լ�
    {
        Setting.SetActive(true);
    }
    public void CloseSetting()//����â�� �ݴ� �Լ�
    {
        Setting.SetActive(false);
    }

    public void GiveUpGame()//�׺����
    {
        Setting.SetActive(false);
        GameEnd(false);
    }


    public void GameEnd(bool result)//�������Ḧ �����ϴ� �Լ�
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







            
            StageClear_Function(Enemy_Manager.ID - 1);
            

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
    public void StageClear_Function(int i)//���� ���������� �ر��ϴ� �Լ�
    {

        FireBaseDB.instacne.Player_Data_instacne.StageClear[i] = 2;
        if (i < FireBaseDB.instacne.Player_Data_instacne.StageClear.Count - 1)
        {
            if (FireBaseDB.instacne.Player_Data_instacne.StageClear[i + 1] == 0)
            {
                FireBaseDB.instacne.Player_Data_instacne.StageClear[i + 1] = 1;
                FireBaseDB.instacne.Upload_Data(StoreTYPE.STAGE);
            }
        }

    }
    public void Reward()//���ӿ��� �̰��� �� ������ó���ϴ� �Լ�
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
    public void MoveMainScene()//���ӽ����������� �������� ����â���� ���ư��� �Լ�
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

