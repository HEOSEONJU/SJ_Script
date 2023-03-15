
using UnityEngine;
using Firebase.Auth;
using Firebase.Database;
using System;
using System.Collections;


using Firebase.Extensions;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;
using System.Reflection;
using static UnityEngine.UI.CanvasScaler;

//using System.Threading.Tasks;
//using System.Threading;

public class FireBase_Manager:MonoBehaviour
{
    //������
    [SerializeField]
    GameObject Player_Char;
    [SerializeField]
    GameObject UI_Object;
    



    [SerializeField]
    Player_Data Data;
    [SerializeField]
    Item_List item_List_Ojbect;
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
    bool LoginState=false;

    
    public bool LoadingState = false;
    // Start is called before the first frame update

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        string ID= "zxaz741@gmail.com";
        string Pass = "12as12";
        Init();
        Login(ID, Pass);
        


    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            save();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Load();
        }



    }

    public void save()
    {
        string json =JsonUtility.ToJson(Data);
        Fire_DataBase.Child("Users").Child(LoginID).SetRawJsonValueAsync(json);

        
        
        


    }
    public void Load()//������� �����ؼ� i���� ���� �ٲ� for������ �ٸ� ������� �ε��� �ؾ��� =>�ݹ��̹�� �Լ��̿��ؼ� �ذ�
    {
        StartCoroutine(Load_Data());


    }
    IEnumerator Load_Data()
    {
        LoadingState = false;

        Fire_DataBase.Child("Users").Child(LoginID).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted)
            {
                Debug.Log("����");
            }
            else if (task.IsCanceled)
            {
                Debug.Log("���");
            }
            else if (task.IsCompleted)
            {
                Weapon_Load(task.Result);
                for (int i = 0; i < 3; i++)
                {
                    Armor_Load(task.Result, i);
                }
                for (int i = 0; i < 20; i++)
                {
                    Item_Load(task.Result, i);
                }

                Gold_Load(task.Result);

                Position_Load(task.Result);
                
            }
            LoadingState = true;

        });
        while(!LoadingState)
        { 
            yield return null;
        }
        
        Debug.Log("�ε��Ϸ�");
        
    }



    void Weapon_Load(DataSnapshot Load_Data)
    {
        
        Data.Equip_Weapon_Item.Insert_Data(item_List_Ojbect.Search_item(Convert.ToInt32(Load_Data.Child("Equip_Weapon_Item").Child("INDEX").Value)));
        Data.Equip_Weapon_Item.Upgrade = Convert.ToInt32(Load_Data.Child("Equip_Weapon_Item").Child("Upgrade").Value);
        Data.Equip_Weapon_Item.count = Convert.ToInt32((Load_Data.Child("Equip_Weapon_Item").Child("count").Value));

    }
    void Armor_Load(DataSnapshot Load_Data, int i)
    {
        Data.Equip_Armor_Item[i].Insert_Data(item_List_Ojbect.Search_item(Convert.ToInt32(Load_Data.Child("Equip_Armor_Item").Child(i.ToString()).Child("INDEX").Value)));
        Data.Equip_Armor_Item[i].Upgrade = Convert.ToInt32((Load_Data.Child("Equip_Armor_Item").Child(i.ToString()).Child("Upgrade").Value));
        Data.Equip_Armor_Item[i].count = Convert.ToInt32((Load_Data.Child("Equip_Armor_Item").Child(i.ToString()).Child("count").Value));
        
    }
    void Item_Load(DataSnapshot Load_Data,int i)
    {

        Data.Items[i].Insert_Data(item_List_Ojbect.Search_item(Convert.ToInt32(Load_Data.Child("Items").Child(i.ToString()).Child("INDEX").Value)));
        Data.Items[i].Upgrade = Convert.ToInt32((Load_Data.Child("Items").Child(i.ToString()).Child("Upgrade").Value));
        Data.Items[i].count = Convert.ToInt32((Load_Data.Child("Items").Child(i.ToString()).Child("count").Value));

    }

    public void Gold_Load(DataSnapshot Load_Data)
    {
        Data.Gold = Convert.ToInt32(Load_Data.Child("Gold").Value);
    }
    public void Position_Load(DataSnapshot Load_Data)
    {
        Data.Last_Position.x = Convert.ToInt32(Load_Data.Child("Last_Position").Child("x").Value);
        Data.Last_Position.y = Convert.ToInt32(Load_Data.Child("Last_Position").Child("y").Value);
        Data.Last_Position.z = Convert.ToInt32(Load_Data.Child("Last_Position").Child("z").Value);

    }
    public bool Use_Money(int n, Player_Data Data)
    {
        Fire_DataBase.Child("Users").Child(LoginID).Child("Gold").GetValueAsync().ContinueWithOnMainThread(task =>
        {

            Data.Gold = (int)task.Result.Value;
        });
        if(Data.Gold >= n) 
        {
            Data.Gold-= n;
            Fire_DataBase.Child("Users").Child(LoginID).Child("Gold").SetValueAsync(Data.Gold);
            return true;
        }
        return false;
    }

    void Init()
    {
        Fire_DataBase = FirebaseDatabase.DefaultInstance.RootReference;
        auth = FirebaseAuth.DefaultInstance;
        auth.StateChanged += OnChange;
    }
    void OnChange(object sender, EventArgs e) 
    {
        
        if(auth.CurrentUser !=user)
        {
            bool signed = (auth.CurrentUser !=user && auth.CurrentUser!=null);
            if(!signed && user !=null) 
            {
                //�α׾ƿ�
                LoginState = false;
            }
            
            user = auth.CurrentUser;
            
            
            if (signed)
            {
                LoginState = true;
                //�α���
                Load();

                StartCoroutine(Init_Player_Char());
            }
        }
    }
    IEnumerator Init_Player_Char()
    {
        while(!LoadingState)
        {
            yield return null;
        }

        var Temp = Instantiate(Player_Char, Data.Last_Position, Quaternion.identity).GetComponent<Player_Manager>();
        Temp.FireBase_M = this;
        Temp._UI = Instantiate(UI_Object).GetComponent<UI_Manager>();
        Temp.Init(Data);
        
        
    }
    

    
    void CreateUser(string ID,string Password)
    {
        auth.CreateUserWithEmailAndPasswordAsync(ID, Password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                return;
            }
            if(task.IsFaulted)
            {
                return;
            }
            FirebaseUser Temp = task.Result;
        }
        
        );
        



    }
    void Login(string ID, string Password)
    {
        auth.SignInWithEmailAndPasswordAsync(ID, Password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                return;
            }
            if (task.IsFaulted)
            {
                return;
            }
            if (task.IsCompleted)
            {
                
                Debug.Log("�α��νõ�  ����");

                
            }
        }

);
    }
    void Logout()
    {
        auth.SignOut();
    }



}

