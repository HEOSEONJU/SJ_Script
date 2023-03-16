using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class CharInfo : MonoBehaviour
{
    //public PlayerInfos Data;
    public CharCardScriptTable CharData;
    public int MYNUM;//자기몇번째인지
    public int CharId;
    public int CharLevel;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Level;
    public Image CharImage;
    public Image CharBGImage;
    public Image Rankimage;

    public GameObject CharUI;

    

    private void OnEnable()
    {
        

        Reset();

    }



    public void OpenCharUI()
    {
        
        CharUI.SetActive(true);
        CharUI.GetComponent<CharUI>().Setting(CharId, CharLevel, MYNUM);
    }
    public void Reset()
    {
        CharId = FireBaseDB.instacne.Player_Data_instacne.UseMonsterCards[MYNUM];

        CharLevel=FireBaseDB.instacne.Player_Data_instacne.Find_Monster_Data(CharId).Level;

        Level.text = "LV :" + CharLevel;
        Name.text = "" + CharData.Monster[CharId].CardName;
        CharImage.sprite = CharData.Monster[CharId].Image;
        
        Rankimage.sprite = CharData.Monster[CharId].RankImage;
        CharBGImage.sprite = CharData.Monster[CharId].BGImage;
    }
}

