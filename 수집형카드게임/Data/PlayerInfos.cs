using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;



[System.Serializable]
public class PlayerData
{
    public string ID;
    public string Password;
    public string Name;
    public int Level;
    public int Exp;
    public int Stamina;
    public int Gem;
    public int Gold;
    public int SpellCardPack;
    public int UsePack;
    public List<int> GetPack;
    public List<int> HaveCard;

    public List<int> DeckCards;
    public List<MonsterInfo> MonsterCards;
    public List<int> UseMonsterCards;


    public List<int> StageClear;//0잠금,1오픈/2클리어

    //public int LGold = 10000;
    //public int LGem = 1000;
    //public int LCardPack = 10;
    
    public DateTime CurrentTime;
    
    public DateTime LastTime;

    


    public void Init_Account(string id, string password, string Input_name)
    {
        
        ID = id;
        Password = password;
        Name = Input_name;

        Level = 1;
        Exp = 0;
        Stamina = 60;
        Gem = 0;
        Gold = 10000;
        SpellCardPack = 10;
        UsePack = 0;

        HaveCard=new List<int>();

        DeckCards= new List<int>();

        
        DeckCards.Add(1);
        DeckCards.Add(1);
        DeckCards.Add(1);
        DeckCards.Add(2);
        DeckCards.Add(2);
        DeckCards.Add(2);
        DeckCards.Add(3);
        DeckCards.Add(3);
        DeckCards.Add(3);
        DeckCards.Add(7);



        MonsterCards = new List<MonsterInfo>();
        UseMonsterCards =new List<int>();

        MonsterInfo temp= new MonsterInfo();
        temp.Init_Monset(2);
        MonsterCards.Add(temp);
        UseMonsterCards.Add(temp.ID);

        temp = new MonsterInfo();
        temp.Init_Monset(3);
        MonsterCards.Add(temp);
        UseMonsterCards.Add(temp.ID);

        temp = new MonsterInfo();
        temp.Init_Monset(4);
        MonsterCards.Add(temp);
        UseMonsterCards.Add(temp.ID);

        temp = new MonsterInfo();
        temp.Init_Monset(5);
        MonsterCards.Add(temp);
        UseMonsterCards.Add(temp.ID);
        //처음장착할4명





        StageClear = new List<int>();
        for (int i=0;i<18;i++)
        {
            StageClear.Add(0);
        }
        StageClear[0]=1;
        CurrentTime = LastTime = DateTime.Now;
    }



    

}





    [System.Serializable]
public class MonsterInfo
{
    public int ID;
    public int Level;
    public int BreaK_Lim;

    public void Init_Monset(int id)
    {
        ID = id;
        Level = 1;
        BreaK_Lim = 1;

    }
}

public class PlayerInfos : MonoBehaviour
{
    


    public int LGold = 10000;
    public int LGem = 1000;
    public int LCardPack = 10;

    [SerializeField]
    GameObject Camera;
    [SerializeField]
    GameObject Canvas;

    
    public DateTime CurrentTime;
    
    public DateTime LastTime;

    int Max_Stamina = 60;
    int RegenTimer = 10;
         


    private void Awake()
    {
        
        
        Camera.SetActive(true);
        Camera.GetComponent<ManagementMainUI>().Init();
        Canvas.SetActive(true);
        FireBaseDB.instacne.Player_Data_instacne.CurrentTime = DateTime.Now;
        
        TimeSpan T =FireBaseDB.instacne.Player_Data_instacne.CurrentTime - FireBaseDB.instacne.Player_Data_instacne.LastTime;
        FireBaseDB.instacne.Player_Data_instacne.LastTime = DateTime.Now;
        FireBaseDB.instacne.Upload_Data(StoreTYPE.TIME);
        int M = T.Hours*60+T.Minutes;
        if(M>=RegenTimer)
        {
            FireBaseDB.instacne.Player_Data_instacne.Stamina += M / RegenTimer;
            if(FireBaseDB.instacne.Player_Data_instacne.Stamina>60)
            {
                FireBaseDB.instacne.Player_Data_instacne.Stamina = 60;
            }
            FireBaseDB.instacne.Upload_Data(StoreTYPE.STAMINA);
        }

    }


    private void Update()
    {
        CurrentTime=DateTime.Now;
        StamainaAdd();
    }

    void StamainaAdd()
    {

        TimeSpan temp = CurrentTime - LastTime;

        if (temp.Minutes >= RegenTimer)
        {
            InfiniteLoopDetector.Run();
            LastTime.AddMinutes(RegenTimer);
            if (FireBaseDB.instacne.Player_Data_instacne.Stamina < Max_Stamina)
            {
                FireBaseDB.instacne.Player_Data_instacne.Stamina += 1;
                FireBaseDB.instacne.Upload_Data(StoreTYPE.STAMINA);
                FireBaseDB.instacne.Upload_Data(StoreTYPE.TIME);
            }
        }
        
        
    }



    public void StageClear_Function(int i)
    {

        FireBaseDB.instacne.Player_Data_instacne.StageClear[i] = 2;
        if(i< FireBaseDB.instacne.Player_Data_instacne.StageClear.Count-1)
        {
            if (FireBaseDB.instacne.Player_Data_instacne.StageClear[i + 1] == 0)
            {
                FireBaseDB.instacne.Player_Data_instacne.StageClear[i + 1] = 1;
                FireBaseDB.instacne.Upload_Data(StoreTYPE.STAGE);
            }
        }

    }

    public bool LevelUp()
    {
        if(FireBaseDB.instacne.Player_Data_instacne.Exp >= 200)
        {
            FireBaseDB.instacne.Player_Data_instacne.Level += 1;
            FireBaseDB.instacne.Upload_Data(StoreTYPE.LEVEL);
            FireBaseDB.instacne.Player_Data_instacne.Exp -= 200;
            FireBaseDB.instacne.Upload_Data(StoreTYPE.EXP);

            FireBaseDB.instacne.Player_Data_instacne.Gold += LGold;
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GOLD);
            FireBaseDB.instacne.Player_Data_instacne.Gem += LGem;
            FireBaseDB.instacne.Upload_Data(StoreTYPE.GEM);
            FireBaseDB.instacne.Player_Data_instacne.SpellCardPack += LCardPack;
            FireBaseDB.instacne.Upload_Data(StoreTYPE.PACK);
            return true;
        }
        else
        {
            return false;
        }
    }
    

    
}

