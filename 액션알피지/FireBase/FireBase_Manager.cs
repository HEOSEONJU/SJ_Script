
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System;
using System.Collections;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using TMPro;

using UnityEngine.SceneManagement;
using static UnityEngine.Rendering.DebugUI;
using System.Web;
using UnityEditor;
using Quests;

//using System.Threading.Tasks;
//using System.Threading;

public class FireBase_Manager : MonoBehaviour
{
    //생성용
    [SerializeField]
    public GameObject Player_Char;
    [SerializeField]
    GameObject UI_Object;
    #region 데이터
    [SerializeField]
    Player_Data Data;
    [SerializeField]
    Item_List item_List_Object;
    [SerializeField]
    Quest_List Quest_List_Object;

    #endregion
    static FireBase_Manager instance = null;
    static FireBase_Manager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new FireBase_Manager();
            }
            return instance;
        }
    }

    DatabaseReference Fire_DataBase;
    public FirebaseAuth auth;
    public FirebaseUser user;
    public string LoginID => user.UserId;
    [SerializeField]
    bool LoginState = false;
    public bool LoadingState = false;
    bool Delay_Trigger = false;

    int Choice_ID;

    public int Set_ID
    {
        set { Choice_ID = value; }
    }

    

    [SerializeField]
    Transform NPC_Parent;

    

    [SerializeField]
    TextMeshProUGUI TEXT;

    delegate void Event();
    Event eventFunc;
    Queue<Event> eventQueue;
    
    Queue<string> Text_value;//텍스트 큐형식 같이 저장

    [SerializeField]
    Vector3 Starat_Posi;
    struct Event_Struct
    {
        public Event Func;
    }
    
    
    void Start()

    {
        DontDestroyOnLoad(this.gameObject);
        
        
        Init();
    }





    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            //save();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            //Load();
        }
        
        if (eventQueue.Count > 0)//비동기 이벤트처리
        {
             eventQueue.Dequeue()();
        }

    }


    void Init()
    {
        Text_value = new Queue<string>();
        eventQueue = new Queue<Event>();
        while (item_List_Object.Add_Item.Count > 0)//추가아이템
        {
            item_List_Object.Insert_New_Item(item_List_Object.Add_Item.First());
            item_List_Object.Add_Item.Remove(item_List_Object.Add_Item.First());
        }
        while (item_List_Object.Delete_Item.Count > 0)//삭제할 아이템
        {
            item_List_Object.Delete_Old_item(item_List_Object.Delete_Item.First());
            item_List_Object.Delete_Item.Remove(item_List_Object.Delete_Item.First());
        }
        item_List_Object.Sort_item();//아이템 정렬


        Quest_List_Object.Sort_List();//퀘스트정렬
        Game_Master.instance.Load_List(item_List_Object, Quest_List_Object);//게임마스터에 아이템/퀘스트 리스트추가

        Fire_DataBase = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
    }
    public void NPC_Setting()//씬 이동후 스타터들이 NPC들 생성자 적용
    {
        GameObject GO = GameObject.Find("NPC");
        if (GO == null)
        {
            return;
        }
        NPC_Parent = GO.transform;
        Base_NPC[] T = NPC_Parent.GetComponentsInChildren<Base_NPC>();
        foreach (var item in T)
        {
            item.Init();
        }
    }

    public void Save_On_FireBase()
    {
        #region 무기
        if (Data.Equip_Weapon_Item.Base_item != null)
            Data.Equip_Weapon_Item.INDEX = Data.Equip_Weapon_Item.Base_item.Item_Index;
        else
            Data.Equip_Weapon_Item.INDEX = 0;
        
        #endregion
        #region 방어구
        foreach (Item_Data T in Data.Equip_Armor_Item)
        {
            if (T.Base_item != null)
                T.INDEX = T.Base_item.Item_Index;
            else
                T.INDEX = 0;
        }
        #endregion
        
        #region 아이템

        foreach (Item_Data T in Data.Items)
        {
            if (T.Base_item != null)
            {
                T.INDEX = T.Base_item.Item_Index;

            }
            else
                T.INDEX = 0;
        }
        
        #endregion
        #region 퀘스트 마지막위치


        
        if (Game_Master.instance.PM != null)
        {
            Data.Accepted_Quest.Clear();
            foreach (Quest_Process QB in Game_Master.instance.PM._playerQuestBox.QBL)
            {
                Quest_Info Temp = new Quest_Info();
                Temp.INDEX = QB.Quest_ID;
                Temp.Progress = QB.Progress;
                Temp.Point = QB.Point;
                
                Data.Accepted_Quest.Add(Temp);
            }
            Data.Last_Position = Game_Master.instance.PM.transform.position;
        }
        
        #endregion
        string json = JsonUtility.ToJson(Data);
        Fire_DataBase.Child("Users").Child(LoginID).Child(Choice_ID.ToString()).SetRawJsonValueAsync(json);
    }

    #region 로드
    IEnumerator Load_Data()
    {
        //Debug.Log("시작");
        LoadingState = false;
        
        //Debug.Log("로드데이터시작" + LoginID);
        
        Fire_DataBase.Child("Users").Child(LoginID).Child(Choice_ID.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                
                ES_ADD("데이터로드실패");
            }
            else if (task.IsCanceled)
            {
                
                ES_ADD("데이터로드취소");
            }
            else if (task.IsCompleted)
            {
                
                ES_ADD("데이터로드시작");


                
                if (Weapon_Load(task.Result))
                {
                    
                    Armor_Load(task.Result);
                    Item_Load(task.Result);
                    Gold_Load(task.Result);
                    Position_Load(task.Result);
                    Quest_Load(task.Result);
                }
                else
                {
                    
                    Init_Char();
                }
                ES_ADD("데이터로드완료");
            }
            LoadingState = true;

        });
        
        yield return null;
        
    }//데이터불러오기코루틴
    bool Weapon_Load(DataSnapshot Load_Data)
    {
        if((Load_Data.Child("Equip_Weapon_Item").Child("INDEX").Value)==null)
        {
            return false;
        }
        Data.Equip_Weapon_Item.Insert_Data(item_List_Object.Search_item(Convert.ToInt32(Load_Data.Child("Equip_Weapon_Item").Child("INDEX").Value)));
        Data.Equip_Weapon_Item.Upgrade = Convert.ToInt32(Load_Data.Child("Equip_Weapon_Item").Child("Upgrade").Value);
        Data.Equip_Weapon_Item.count = 0;
        return true;

    }
    void Armor_Load(DataSnapshot Load_Data)
    {
        Data.Equip_Armor_Item = new List<Item_Data>();
        Item_Data Temp;
        for (int i = 0; i < 3; i++)
        {
            Temp = new Item_Data();
            item T = item_List_Object.Search_item(Convert.ToInt32(Load_Data.Child("Equip_Armor_Item").Child(i.ToString()).Child("INDEX").Value));
            if (T == null)
            {
                Temp.Upgrade = 0;
                Temp.INDEX = 0;
                Temp.count = 0;
            }
            else
            {
                Temp.Insert_Data(T);
                Temp.Upgrade = Convert.ToInt32((Load_Data.Child("Equip_Armor_Item").Child(i.ToString()).Child("Upgrade").Value));

            }

            Data.Equip_Armor_Item.Add(Temp);
        }


    }
    void Item_Load(DataSnapshot Load_Data)
    {

        Data.Items = new List<Item_Data>();
        Item_Data Temp;
        for (int i = 0; i < 20; i++)
        {
            Temp = new Item_Data();
            item T = item_List_Object.Search_item(Convert.ToInt32(Load_Data.Child("Items").Child(i.ToString()).Child("INDEX").Value));
            if (T == null)
            {
                Temp.Upgrade = 0;
                Temp.count = 0;
                Temp.INDEX = 0;
            }
            else
            {
                Temp.Insert_Data(T, Convert.ToInt32((Load_Data.Child("Items").Child(i.ToString()).Child("count").Value)));
                Temp.Upgrade = Convert.ToInt32((Load_Data.Child("Items").Child(i.ToString()).Child("Upgrade").Value));
            }

            Data.Items.Add(Temp);
        }
    }
    public void Gold_Load(DataSnapshot Load_Data)
    {
        Data.Current_Gold = Convert.ToInt32(Load_Data.Child("Gold").Value);
    }
    public void Position_Load(DataSnapshot Load_Data)
    {
        Data.Last_Position.x = Convert.ToInt32(Load_Data.Child("Last_Position").Child("x").Value);
        Data.Last_Position.y = Convert.ToInt32(Load_Data.Child("Last_Position").Child("y").Value);
        Data.Last_Position.z = Convert.ToInt32(Load_Data.Child("Last_Position").Child("z").Value);

    }
    public void Quest_Load(DataSnapshot Load_Data)
    {
        //Debug.Log("퀘스트로드시작");
        Data.Accepted_Quest = new List<Quest_Info>();
        for (int i = 0; i < Load_Data.Child("Accepted_Quest").ChildrenCount; i++)
        {
            Quest_Info TEMPINFO = new Quest_Info();

            TEMPINFO.INDEX = Convert.ToInt32(Load_Data.Child("Accepted_Quest").Child(i.ToString()).Child("INDEX").Value);
            TEMPINFO.Progress = Convert.ToInt32(Load_Data.Child("Accepted_Quest").Child(i.ToString()).Child("Progress").Value);
            TEMPINFO.Point = Convert.ToInt32(Load_Data.Child("Accepted_Quest").Child(i.ToString()).Child("COM").Value);
            Data.Accepted_Quest.Add(TEMPINFO);
        }
        Data.Complted_Quest = new List<int>();
        for (int i = 0; i < Load_Data.Child("Complted_Quest").ChildrenCount; i++)
        {
            Data.Complted_Quest.Add(Convert.ToInt32(Load_Data.Child("Complted_Quest").Child(i.ToString()).Value));
        }
        


    }


    public void Init_Char()
    {
        
        Data=new Player_Data();
        Data.Equip_Weapon_Item = new Item_Data();
        Data.Equip_Armor_Item = new List<Item_Data>();
        Data.Items = new List<Item_Data>();
        //Debug.Log("생성시작");
        Data.Equip_Weapon_Item.Insert_Data(item_List_Object.Search_item(1000));
        //Debug.Log("무기생성완료");
        
        Item_Data Temp;
        
        for (int i = 0; i < 3; i++)
        {
            Temp = new Item_Data();
            Temp.Upgrade = 0;
            Temp.INDEX = 0;
            Temp.count = 0;
            Data.Equip_Armor_Item.Add(Temp);
        }
        //Debug.Log("방어구생성완료");
        
        
        for (int i = 0; i < 20; i++)
        {
            Temp = new Item_Data();
            Temp.Upgrade = 0;
            Temp.count = 0;
            Temp.INDEX = 0;
            Data.Items.Add(Temp);
        }
        //Debug.Log("아이템생성완료");
        Data.Current_Gold = 0;
        Data.Last_Position = Starat_Posi;
        Data.Accepted_Quest = new List<Quest_Info>();
        Data.Complted_Quest = new List<int>();
        //Debug.Log("생성종료");
        Save_On_FireBase();
    }
    #endregion

    public IEnumerator Init_Player_Char()//플레이어 캐릭터 생성
    {
        while (!LoadingState)
        {
            yield return null;
        }

        Instantiate(Player_Char, Data.Last_Position + Vector3.up, Quaternion.identity).TryGetComponent<Player_Manager>(out Player_Manager PM);
        PM._fireBaseManger = this;
        Instantiate(UI_Object).TryGetComponent<UI_Manager>(out UI_Manager _UI);
         

        Game_Master.instance.Load(PM, _UI);
        _UI.Init();
        PM.Init();
        


    }

    public IEnumerator SceneLoading(string SceneName)
    {
        
        StartCoroutine(Load_Data());
        
        while (!LoadingState)
        {
            
            yield return null;
        }
        
        AsyncOperation operation = SceneManager.LoadSceneAsync(SceneName);
        operation.allowSceneActivation = true;
        

        while (!operation.isDone)
        {
            
            yield return null;
        }
        
        //ES_ADD("불러오기 완료");
        yield break;

        
    }

    #region 이벤트 큐영역
    
    
    public void ES_ADD(string value)
    {
        
        Text_value.Enqueue(value);
        eventQueue.Enqueue(Text_Function);
    }



    void Text_Function()//비동기 텍스트변경
    {
        string T= Text_value.Dequeue();
        if(T!=null) 
        {
            TEXT.text = T;
        }
        else
        {
            TEXT.text = "큐가 빔";
        }
        
    }
    #endregion
    public Player_Data CPD()
    {
        return Data;
    }
}


