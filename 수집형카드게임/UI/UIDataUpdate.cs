using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class UIDataUpdate : MonoBehaviour
{
    //public PlayerInfos Data;
    [Header("TextUI")]
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI LevelText;
    public TextMeshProUGUI ExpText;
    public TextMeshProUGUI StaminaText;
    public TextMeshProUGUI GemText;
    public TextMeshProUGUI GoldText;
    // Update is called once per frame

    private void OnEnable()
    {
        Invoke("UpdateData",Time.deltaTime);
        
    }

    private void Update()
    {
        //UpdateData();
    }

    public void UpdateData()
    {
        if (NameText != null)
        {
            NameText.text = FireBaseDB.instacne.Player_Data_instacne.Name;

        }
        if (LevelText != null)
        {
            LevelText.text = "" + FireBaseDB.instacne.Player_Data_instacne.Level;

        }
        if (ExpText != null)
        {
            ExpText.text = "" + FireBaseDB.instacne.Player_Data_instacne.Exp;

        }
        if (StaminaText != null)
        {
            StaminaText.text = "" + FireBaseDB.instacne.Player_Data_instacne.Stamina;

        }
        if (GemText != null)
        {
            GemText.text = "" + FireBaseDB.instacne.Player_Data_instacne.Gem;

        }
        if (GoldText != null)
        {
            GoldText.text = "" + FireBaseDB.instacne.Player_Data_instacne.Gold;

        }
    }

    
}
