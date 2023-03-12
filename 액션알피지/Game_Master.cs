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
    Player_Manager player_manager;
    UI_Manager _UI;

    public UI_Manager Call_UI()
    {
        return _UI;
    }
    public void Load_UI(UI_Manager temp)
    {
        _UI = temp;
    }

    public Player_Manager Call_Player()
    {
        return player_manager;
    }
    public void Load_Player(Player_Manager temp)
    {
        player_manager=temp;
    }


}
