using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Quest_NPC : Normal_NPC
{
    [SerializeField]
    const int Max_Quset = 6;
    public List<int> Have_Quest_ID;
    //public Vender_Function Function;
    public Quest_Function QF;
    public GameObject Quest_UI;
    public override void Init()
    {
        base.Init();
        QF.Init(this,Have_Quest_ID);
        
    }

    public override bool Check_UI()
    {
        Debug.Log("Quest_UI" + Quest_UI.activeSelf+"/ UI" + base.Check_UI());
        
        return (base.Check_UI() || Quest_UI.activeSelf);
    }

    public void Open_Quset()
    {
        Quest_UI.SetActive(true);
        QF.Reset_Page();
        Close_Window();
    }

    public void  Close_Quest()
    {

        Close_ALL_Window();
        
        
        

    }
    public override void Close_ALL_Window()
    {
        Quest_UI.SetActive(false);
        base.Close_ALL_Window();
    }
}
[System.Serializable]
public struct Quest_Set
{
    public int Quset_ID;
    public Quest_Type TYPE;

}