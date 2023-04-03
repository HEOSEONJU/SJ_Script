using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Master : MonoBehaviour
{

    public static Game_Master instance = null;
    public static Game_Master Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Game_Master();
            }
            return instance;
        }

    }
    void Awake()
    {
        if(instance==null)
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    [SerializeField]
    Player_Manager player_manager;
    [SerializeField]
    UI_Manager _UI;
    [SerializeField]
    Item_List item_List_Object;
    [SerializeField]
    Quest_List Quest_List_Object;


    public Player_Manager PM
    {
        get { return player_manager; }
        
    }
    public  UI_Manager UI
    {
        get { return _UI; }
    }

    public Item_List ILO
    {
        get { return item_List_Object; }
    }
    
    public Quest_List QLO 
    { 
        get { return Quest_List_Object; } 
    }

    
    public void Load_List(Item_List Temp_Item, Quest_List Temp_Quset)
    {
        item_List_Object = Temp_Item;
        Quest_List_Object = Temp_Quset;
    }


    
    public void Load(Player_Manager Temp_PM, UI_Manager Temp_UI)
    {
        player_manager = Temp_PM;
        _UI = Temp_UI;
        
    }

}
