using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class ManagementMainUI : MonoBehaviour
{
    [Header("Info")]
    public PlayerInfos Data;
    public string Name;
    public int Level;
    public int Exp;
    int MaxExp = 200;
    public Image ExpGage;
    public int Stamina;
    public int Gem;
    public int Gold;

    public EnemyManager Enemy_Manager;




    


    [Header("UIObjcet")]
    public GameObject Setting;

    public GameObject Shop;

    public GameObject GemShop;

    public GameObject GoldShop;

    public GameObject Misson;

    public GameObject Reward;

    public GameObject Deck;

    public GameObject Summon;

    public GameObject Character;

    public GameObject Stage;

    [Header("TextUI")]
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI StaminaText;
    public TextMeshProUGUI GemText;
    public TextMeshProUGUI GoldText;
    

    
    
    [SerializeField]
    GameObject RewardObject;
    [SerializeField]
    TextMeshProUGUI Level_Text;
    [SerializeField]
    TextMeshProUGUI Add_Gold_Text;
    [SerializeField]
    TextMeshProUGUI Add_Ticket_Text;
    [SerializeField]
    TextMeshProUGUI Add_Gem_Text;

    [SerializeField]
    Transform CenterPosi;
    [SerializeField]
    Transform SidePosi;
    


    
    public void Init()
    {
        UpdateDate();

        Add_Gold_Text.text = "" + Data.LGold;
        Add_Ticket_Text.text = "" + Data.LCardPack;
        Add_Gem_Text.text = "" + Data.LGem;
    }

    // Update is called once per frame

    public void UpdateDate()
    {
        if(Data.LevelUp())
        {
            RewardObject.SetActive(true);
            Level_Text.text = "" + FireBaseDB.instacne.Player_Data_instacne.Level;
            StartCoroutine(LevelUpReward());
        }
        Name= FireBaseDB.instacne.Player_Data_instacne.Name;
        Level= FireBaseDB.instacne.Player_Data_instacne.Level;
        Exp= FireBaseDB.instacne.Player_Data_instacne.Exp;
        Stamina= FireBaseDB.instacne.Player_Data_instacne.Stamina;
        Gem= FireBaseDB.instacne.Player_Data_instacne.Gem;
        Gold = FireBaseDB.instacne.Player_Data_instacne.Gold;
        UpdateState();
    }

    /* 레벨업테스트
    private void Update()
    {
        
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Data.Exp += 200;
            if (Data.LevelUp())
            {
                RewardObject.SetActive(true);
                Level_Text.text = "" + Data.Level;
                Name = Data.Name;
                Level = Data.Level;
                Exp = Data.Exp;
                Stamina = Data.Stamina;
                Gem = Data.Gem;
                Gold = Data.Gold;
                UpdateState();
                StartCoroutine(LevelUpReward());
            }
        }
    }
    */

    void UpdateState()
    {
        NameText.text = Name;
        LevelText.text = "" + Level;
        ExpText.text = Exp + "/" + MaxExp;
        
        Invoke("Invoke_Gage", Time.deltaTime);
        StaminaText.text = "" + Stamina;
        GemText.text = "" + Gem;
        GoldText.text = "" + Gold;

    }
    void Invoke_Gage()
    {
        ExpGage.fillAmount = (Exp * 1.0f) / MaxExp;
    }
    IEnumerator LevelUpReward()
    {
        float time=0;
        while(time<=1)
        {
            time += Time.deltaTime/2;
            RewardObject.transform.position = Vector3.Lerp(RewardObject.transform.position, CenterPosi.transform.position, time);
            yield return new WaitForSeconds(Time.deltaTime);
        }


        
    }

    public void CloseLevelUp()
    {
        StopCoroutine(LevelUpReward());
        RewardObject.transform.position = SidePosi.position;
        RewardObject.SetActive(false);
        
    }

    public void OpenStageUI()
    {
        
        Stage.SetActive(true);

        
    }
    public void CloseStageUI()
    {
        UpdateDate();
        Stage.SetActive(false);
    }

    public void OpenSummonUI()
    {
        Summon.SetActive(true);
    }
    public void CloseSummonUI()
    {
        UpdateDate();
        Summon.SetActive(false);
    }

    public void OpenSettingUI()
    {
        //Setting.SetActive(true); 옵션기능구현안함
    }
    public void CloseSettingUI()
    {
        UpdateDate();
        Setting.SetActive(false);
    }
    public void OpenShopUI()
    {
        Shop.SetActive(true);
    }
    public void CloseShopUI()
    {
        UpdateDate();
        Shop.SetActive(false);
    }
    public void OpenGemShopUI()
    {
        GemShop.SetActive(true);
    }
    public void CloseGemShopUI()
    {
        UpdateDate();
        GemShop.SetActive(false);
    }
    public void OpenGoldShopUI()
    {
        GoldShop.SetActive(true);
    }
    public void CloseGoldShopUI()
    {
        UpdateDate();
        GoldShop.SetActive(false);
    }
    public void OpenMissonShopUI()
    {
        Misson.SetActive(true);
    }
    public void CloseMissonShopUI()
    {
        UpdateDate();
        Misson.SetActive(false);
    }
    public void OpenRewardShopUI()
    {
        Reward.SetActive(true);
    }
    public void CloseRewardShopUI()
    {
        UpdateDate();
        Reward.SetActive(false);
    }

    public void OpenDeckUI()
    {
        Deck.SetActive(true);
    }
    public void CloseDeckUI()
    {
        UpdateDate();
        Deck.SetActive(false);
    }
    public void OpenCharacterUI()
    {
        Character.SetActive(true);
    }
    public void CloseCharacterUI()
    {
        UpdateDate();
        Character.SetActive(false);
    }
}
