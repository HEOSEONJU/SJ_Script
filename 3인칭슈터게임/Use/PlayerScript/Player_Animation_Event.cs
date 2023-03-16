using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Animation_Event : MonoBehaviour
{
    [SerializeField]
    Player_Manager _Manager;
    



    public void After_Reload()
    {
        _Manager.Shoot_Manager.Equip_Gun.Reload_After_Function();
    }
    public void After_Swap()
    {
        _Manager.Shoot_Manager.After_Swap();
    }
}
