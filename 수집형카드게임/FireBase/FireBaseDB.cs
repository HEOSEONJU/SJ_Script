using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase.Firestore;
using Firebase.Extensions;
using JetBrains.Annotations;
using System.Xml.Linq;
using UnityEngine.Assertions;
using System;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine.UI;
using System.Globalization;

public class FireBaseDB : MonoBehaviour
{

    public static FireBaseDB instacne;
    [SerializeField]
    public PlayerData Player_Data_instacne;
    FirebaseFirestore DB;
    
    Data_TO_INT TEMP_I;
    Data_TO_STRING TEMP_S;
    Data_TO_Array TEMP_A;
    Data_TO_Char TEMP_C;
    Data_TO_TIME TEMP_T;

    [SerializeField]
    TitleManager _Title;
    
    // Start is called before the first frame update
    void Start()
    {
        if (instacne == null)
        {
            instacne = this;
            DB = FirebaseFirestore.DefaultInstance;
            DontDestroyOnLoad(this);
            TEMP_I = new Data_TO_INT();
            TEMP_S = new Data_TO_STRING();
            TEMP_C = new Data_TO_Char();
            TEMP_A = new Data_TO_Array();
            TEMP_T = new Data_TO_TIME();
        }
        else
        {
            Destroy(this.gameObject);
        }
        
    }

    
    

    // Update is called once per frame
    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.R))
        {
            
            Upload_Data(StoreTYPE.NAME);
            Upload_Data(StoreTYPE.LEVEL);
            Upload_Data(StoreTYPE.EXP);
            Upload_Data(StoreTYPE.GOLD);
            Upload_Data(StoreTYPE.GEM);
            Upload_Data(StoreTYPE.PACK);
            Upload_Data(StoreTYPE.HAVE);
            Upload_Data(StoreTYPE.DECK);
            Upload_Data(StoreTYPE.CHAR);
            Upload_Data(StoreTYPE.USECHAR);
            Upload_Data(StoreTYPE.STAGE);
            Upload_Data(StoreTYPE.STAMINA);
            //UPLOAD_LEVEL_EXP();
            //ALL_UPLOAD();
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Download_Data(StoreTYPE.NAME);
            Download_Data(StoreTYPE.LEVEL);
            Download_Data(StoreTYPE.EXP);
            Download_Data(StoreTYPE.GOLD);
            Download_Data(StoreTYPE.GEM);
            Download_Data(StoreTYPE.PACK);
            Download_Data(StoreTYPE.HAVE);
            Download_Data(StoreTYPE.DECK);
            Download_Data(StoreTYPE.CHAR);
            Download_Data(StoreTYPE.USECHAR);
            Download_Data(StoreTYPE.STAGE);
            Download_Data(StoreTYPE.STAMINA);
        }
        */
    }

    public bool Check_ID(string ID)
    {
        DB.Document("ID_List/iIMCnt0VoWUoljB0XMwj").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            Assert.IsNull(task.Exception);
            var cha = task.Result.ConvertTo<Data_TO_STRING_Array>();
            foreach ( var ID_ in cha.ID ) 
            {
               if(ID_==ID)
                {
                    return true;
                }
               
            }
            return false;
            
        });
        return false;
    }
    public void Login(string ID,string PW)
    {
        DB.Document("ID_List/iIMCnt0VoWUoljB0XMwj").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            
            Assert.IsNull(task.Exception);//널 구분
            
            var cha = task.Result.ConvertTo<Data_TO_STRING_Array>();
            
            for (int i=0;i<cha.ID.Length;i++)
            {
                if (cha.ID[i] == ID && cha.PW[i] == PW)
                {
                    Player_Data_instacne.ID = cha.ID[i];
                    
                    Download_Data(StoreTYPE.NAME);
                    Download_Data(StoreTYPE.LEVEL);
                    Download_Data(StoreTYPE.EXP);
                    Download_Data(StoreTYPE.GOLD);
                    Download_Data(StoreTYPE.GEM);
                    Download_Data(StoreTYPE.PACK);
                    Download_Data(StoreTYPE.HAVE);
                    Download_Data(StoreTYPE.DECK);
                    Download_Data(StoreTYPE.CHAR);
                    Download_Data(StoreTYPE.USECHAR);
                    Download_Data(StoreTYPE.STAGE);
                    Download_Data(StoreTYPE.STAMINA);
                    Download_Data(StoreTYPE.TIME);
                    _Title.Sucess_Login();
                    return;
                }

            }
            _Title.Faile_Text();

        });
        
    }

    public void Create_ID(string ID,string Password, string NickName)
    {
        if(ID=="" || Password=="" || NickName=="")
        {
            _Title.Empty_Text();
        }
        DB.Document("ID_List/iIMCnt0VoWUoljB0XMwj").GetSnapshotAsync().ContinueWithOnMainThread(task =>
        {
            
            Assert.IsNull(task.Exception);

            bool ck = false;
            var cha = task.Result.ConvertTo<Data_TO_STRING_Array>();
            for (int i=0;i< cha.ID.Length;i++)
            {
                if (cha.ID[i] == ID)
                {
                    ck = true;
                    break;
                }
            }
            //Debug.Log(ID+"/"+Password+"/"+NickName); 

            if (!ck)
            {
                
                List<string> T_ID = cha.ID.ToList();
                List<string> T_PW = cha.PW.ToList();
                T_ID.Add(ID);
                T_PW.Add(Password);
                cha.ID = T_ID.ToArray();
                cha.PW = T_PW.ToArray();
                DB.Document("ID_List/iIMCnt0VoWUoljB0XMwj").SetAsync(cha);

                _Title.Init_Text();
                Upload_Data(StoreTYPE.NAME);
                Upload_Data(StoreTYPE.LEVEL);
                Upload_Data(StoreTYPE.EXP);
                Upload_Data(StoreTYPE.GOLD);
                Upload_Data(StoreTYPE.GEM);
                Upload_Data(StoreTYPE.PACK);
                Upload_Data(StoreTYPE.HAVE);
                Upload_Data(StoreTYPE.DECK);
                Upload_Data(StoreTYPE.CHAR);
                Upload_Data(StoreTYPE.USECHAR);
                Upload_Data(StoreTYPE.STAGE);
                Upload_Data(StoreTYPE.STAMINA);
                Upload_Data(StoreTYPE.TIME);
                Debug.Log("데이터생성중");
            }
            else
            {
                _Title.Duplication_Text();

            }
           


        });


        
    }

    public void Download_Data(StoreTYPE TYPE)
    {
        switch (TYPE)
        {
            case StoreTYPE.NAME:
                DB.Document(Player_Data_instacne.ID + "/NAME").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_STRING>();
                    Player_Data_instacne.Name = cha.Value;
                });
                break;
            case StoreTYPE.LEVEL:
                DB.Document(Player_Data_instacne.ID + "/Level").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_INT>();
                    Player_Data_instacne.Level = cha.Value;
                });
                break;
            case StoreTYPE.EXP:
                DB.Document(Player_Data_instacne.ID + "/EXP").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_INT>();
                    Player_Data_instacne.Exp = cha.Value;
                });
                break;
            case StoreTYPE.GOLD:
                DB.Document(Player_Data_instacne.ID + "/GOLD").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_INT>();
                    Player_Data_instacne.Gold = cha.Value;
                });
                break;
            case StoreTYPE.GEM:
                DB.Document(Player_Data_instacne.ID + "/GEM").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_INT>();
                    Player_Data_instacne.Gem = cha.Value;
                });
                break;
            case StoreTYPE.PACK:
                DB.Document(Player_Data_instacne.ID + "/PACK").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_INT>();
                    Player_Data_instacne.SpellCardPack = cha.Value;
                });
                break;
            case StoreTYPE.HAVE:

                DB.Document(Player_Data_instacne.ID + "/HAVE").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_Array>();
                    Player_Data_instacne.HaveCard= cha.INDEX.ToList();
                });
                break;
            case StoreTYPE.DECK:
                DB.Document(Player_Data_instacne.ID + "/DECK").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_Array>();
                    Player_Data_instacne.DeckCards = cha.INDEX.ToList();
                });
                break;
            case StoreTYPE.CHAR:
                DB.Document(Player_Data_instacne.ID + "/CHAR").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    Data_TO_Char cha = task.Result.ConvertTo<Data_TO_Char>();
                    Player_Data_instacne.MonsterCards.Clear();
                    for (int i=0;i<cha.INDEX.Length;i++)
                    {
                        MonsterInfo TEMP_MI= new MonsterInfo();
                        TEMP_MI.ID = cha.INDEX[i];
                        TEMP_MI.Level = cha.LEVEL[i];
                        TEMP_MI.BreaK_Lim = cha.BREAK[i];
                        Player_Data_instacne.MonsterCards.Add(TEMP_MI);
                    }
                });
                break;
            case StoreTYPE.USECHAR:
                DB.Document(Player_Data_instacne.ID + "/USECHAR").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_Array>();
                    Player_Data_instacne.UseMonsterCards = cha.INDEX.ToList();
                });
                break;
            case StoreTYPE.STAGE:
                DB.Document(Player_Data_instacne.ID + "/STAGE").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_Array>();
                    Player_Data_instacne.StageClear = cha.INDEX.ToList();
                });
                break;
            case StoreTYPE.STAMINA:
                DB.Document(Player_Data_instacne.ID + "/STAMINA").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_INT>();
                    Player_Data_instacne.Stamina = cha.Value;
                });
                break;
            case StoreTYPE.TIME:
                DB.Document(Player_Data_instacne.ID + "/TIME").GetSnapshotAsync().ContinueWithOnMainThread(task =>
                {
                    Assert.IsNull(task.Exception);
                    var cha = task.Result.ConvertTo<Data_TO_TIME>();
                    Player_Data_instacne.LastTime = cha.DAY;
                });
                break;
        }
    }
    public void Upload_Data(StoreTYPE TYPE)
    {
        switch(TYPE) 
        {
            case StoreTYPE.NAME:
                TEMP_S.Value = Player_Data_instacne.Name;
                DB.Document(Player_Data_instacne.ID+"/NAME").SetAsync(TEMP_S).ContinueWithOnMainThread(tast => { Debug.Log(1); });
                break;
            case StoreTYPE.LEVEL:
                TEMP_I.Value = Player_Data_instacne.Level;
                DB.Document(Player_Data_instacne.ID + "/LEVEL").SetAsync(TEMP_I).ContinueWithOnMainThread(tast => { Debug.Log(2); });
                break;
            case StoreTYPE.EXP:
                TEMP_I.Value = Player_Data_instacne.Exp;
                DB.Document(Player_Data_instacne.ID + "/EXP").SetAsync(TEMP_I).ContinueWithOnMainThread(tast => { Debug.Log(3); });
                break;
            case StoreTYPE.GOLD:
                TEMP_I.Value = Player_Data_instacne.Gold;
                DB.Document(Player_Data_instacne.ID + "/GOLD").SetAsync(TEMP_I).ContinueWithOnMainThread(tast => { Debug.Log(4); });
                break;
            case StoreTYPE.GEM:
                TEMP_I.Value = Player_Data_instacne.Gem;
                DB.Document(Player_Data_instacne.ID + "/GEM").SetAsync(TEMP_I).ContinueWithOnMainThread(tast => { Debug.Log(5); });
                break;
            case StoreTYPE.PACK:
                TEMP_I.Value = Player_Data_instacne.SpellCardPack;
                DB.Document(Player_Data_instacne.ID + "/PACK").SetAsync(TEMP_I).ContinueWithOnMainThread(tast => { Debug.Log(6); });
                break;
            case StoreTYPE.HAVE:
                TEMP_A.INDEX = Player_Data_instacne.HaveCard.ToArray();
                DB.Document(Player_Data_instacne.ID + "/HAVE").SetAsync(TEMP_A).ContinueWithOnMainThread(tast => { Debug.Log(7); });
                break;
            case StoreTYPE.DECK:
                TEMP_A.INDEX = Player_Data_instacne.DeckCards.ToArray();
                DB.Document(Player_Data_instacne.ID + "/DECK").SetAsync(TEMP_A).ContinueWithOnMainThread(tast => { Debug.Log(8); });
                break;
            case StoreTYPE.CHAR:
                List<int> T1 = new List<int>();
                List<int> T2 = new List<int>();
                List<int> T3 = new List<int>();
                foreach (MonsterInfo INFO in Player_Data_instacne.MonsterCards)
                {
                    T1.Add(INFO.ID);
                    T2.Add(INFO.Level);
                    T3.Add(INFO.BreaK_Lim);
                }
                TEMP_C.INDEX = T1.ToArray();
                TEMP_C.LEVEL = T2.ToArray();
                TEMP_C.BREAK = T3.ToArray();
                DB.Document(Player_Data_instacne.ID + "/CHAR").SetAsync(TEMP_C).ContinueWithOnMainThread(tast => { Debug.Log(9); });
                break;
            case StoreTYPE.USECHAR:
                TEMP_A.INDEX = Player_Data_instacne.UseMonsterCards.ToArray();
                DB.Document(Player_Data_instacne.ID + "/USECHAR").SetAsync(TEMP_A).ContinueWithOnMainThread(tast => { Debug.Log(10); });
                break;
            case StoreTYPE.STAGE:
                TEMP_A.INDEX = Player_Data_instacne.StageClear.ToArray();
                DB.Document(Player_Data_instacne.ID + "/STAGE").SetAsync(TEMP_A).ContinueWithOnMainThread(tast => { Debug.Log(11); });
                break;
            case StoreTYPE.STAMINA:
                TEMP_I.Value = Player_Data_instacne.Stamina;
                DB.Document(Player_Data_instacne.ID + "/STAMINA").SetAsync(TEMP_I).ContinueWithOnMainThread(tast => { Debug.Log(12); });
                break;
            case StoreTYPE.TIME:
                TEMP_T.DAY = DateTime.Now;
                DB.Document(Player_Data_instacne.ID + "/TIME").SetAsync(TEMP_T).ContinueWithOnMainThread(tast => { Debug.Log(13); });
                break;
        }
    }
}
public enum StoreTYPE
{
    NAME,
    LEVEL,
    EXP,
    GOLD,
    GEM,
    PACK,
    HAVE,
    DECK,
    CHAR,
    USECHAR,
    STAGE,
    STAMINA,
    TIME
}


[FirestoreData]
public class Data_TO_TIME
{
    [FirestoreProperty]
    public DateTime DAY { get; set; }
    
    

}

[FirestoreData]
public class Data_TO_STRING_Array
{
    [FirestoreProperty]
    public string[] ID { get; set; }
    [FirestoreProperty]
    public string[] PW { get; set; }

}
[FirestoreData]
public class Data_TO_STRING
{
    [FirestoreProperty]
    public string Value { get; set; }
}

[FirestoreData]
public class Data_TO_INT
{
    [FirestoreProperty]
    public int Value { get; set; }
}

[FirestoreData]
public class Data_TO_Array
{
    [FirestoreProperty]
    public int[] INDEX { get; set; }
}
[FirestoreData]
public class Data_TO_Char
{
    [FirestoreProperty]
    public int[] INDEX { get; set; }
    [FirestoreProperty]
    public int[] LEVEL { get; set; }
    [FirestoreProperty]
    public int[] BREAK { get; set; }
}

[FirestoreData]
public class CharDATA
{
    [FirestoreProperty]     
    public string Name{ get; set; }
    [FirestoreProperty]
    public string Description { get; set; }
    [FirestoreProperty]
    public string Attack { get; set; }
    [FirestoreProperty]
    public string Defence { get; set; }
    [FirestoreProperty]
    public int[] INDEX { get; set; }

}

