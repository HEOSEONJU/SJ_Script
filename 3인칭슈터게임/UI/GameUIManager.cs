using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;
public class GameUIManager : MonoBehaviour
{

    public GameManager Infos;
    public GameObject Player;
    public Text BulletText;
    public Text BulletMAXText;
    public Transform GunImage;
    public Text HPText;
    public Image HPImage;
    [SerializeField]
    Player_Manager PlayerInformation;
    public GameCamera Main;
    public GameCamera SUB;

    public Slider PlayerStamina;

    public GameObject CenterPopup;
    public GameObject Popup;
    public bool PopupCheck;
    public bool AnotherThing;
    public int MissionNumber;
    public GameObject Boom;
    public bool BoomCheck;

    public Slider BossHP;
    public Slider BossShield;
    public EnemyBossHP EnemyBoss;

    public AudioSource BGM;

    public Animation StageClear;
    public Animation Warnnig;
    public Transform Centerposi;
    public Transform Endposi;
    public GameObject Defeat;
    public GameObject Victory;
    public Image Fadeimage;
    bool Clear;
    // Start is called before the first frame update
    void Start()
    {
        BGM.Play();
        AnotherThing = false;
        BoomCheck = false;
        MissionNumber = 0;
        PopupCheck = true;
        //UserGun = Player.GetComponent<Gun>();
        PlayerInformation = Player.GetComponent<Player_Manager>();
        PlayerStamina.value = 1.0f;
        Boom.SetActive(false);
        Clear = false;


    }

    // Update is called once per frame
    void Update()
    {
        PauseGame();
        BulletView();
        BoomView();
        HPView();
        Regen();
        PlayerStamina.value = PlayerInformation.Booster / PlayerInformation.BoosterMAX;
        ActivePopup();
        MissionView();
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            PopupCheck = !PopupCheck;
        }
        BossView();
    }

    public void FadeOut()
    {
        
        StartCoroutine(FadeoutCoroutine());
    }
    IEnumerator FadeoutCoroutine()
    {
        
        
        Color color = Fadeimage.color;
        while (color.a<1.0f)
        {
            BGM.volume = Mathf.Lerp(BGM.volume, -0.1f, Time.deltaTime / 2.0f);
            color = Fadeimage.color;
            color.a += Time.deltaTime / 2f;
            Fadeimage.color = color;
            yield return new WaitForSeconds(Time.deltaTime);
        }
        yield return new WaitForSeconds(1.0f);

        SceneManager.LoadScene("ResultScene");
    }

    public void PauseGame()
    {
        if(Input.GetKeyDown(KeyCode.Escape)&GameManager.instance.Hp>0)
        {
            CenterPopup.SetActive(true);
            PlayerInformation.PauseFunction(true);
            PlayerInformation.Action = true;
            Main.Action= true;
            SUB.Action = true;

        }
        
    }
    public void OffPauseGame()
    {
        
        CenterPopup.SetActive(false);
        PlayerInformation.PauseFunction(false);
        PlayerInformation.Action = false;
        Main.Action = false;
        SUB.Action = false;

    }


    public void QuitGame()
    {
        Application.Quit();
    }

    public void InitBoss(EnemyBossHP boss)
    {
        
        EnemyBoss = boss;
        BossHP.gameObject.SetActive(true);
        BossShield.gameObject.SetActive(true);
        Warnnig.Play();
    }
    void BossView()
    {
        if (EnemyBoss != null)
        {
            BossHP.value = EnemyBoss.hp*1f / EnemyBoss.MAXHP;
            BossShield.value = EnemyBoss.ShieldPoint*1f / EnemyBoss.MAXShield;
        }
        else
        {
            BossHP.gameObject.SetActive(false);
            BossShield.gameObject.SetActive(false);
        }


    }

    public void MissionView()
    {
        if (AnotherThing == false)
        {
            switch (MissionNumber)
            {
                case 0:

                    if (GameManager.instance.EnemyID.Count >= 1)
                    {
                        Popup.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "소탕";
                        Popup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "현재지역의 적들을 전부 소탕하십시오.\n\n" + GameManager.instance.EnemyID.Count + "개의 적이 남았습니다";
                    }
                    else if (GameManager.instance.EnemyID.Count == 0)
                    {
                        Popup.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "소탕완료";
                        Popup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "현재지역의 모든 적들이 소탕되었습니다. 다음 지역으로 이동하십시오.";
                        if(Clear==false)
                        {
                            Clear = true;
                            StageClear.Play();
                            Invoke("DelayPopup", 3.5f);
                        }

                    }
                    break;
                case 1:
                    {
                        Popup.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "장비획득";
                        Popup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "새로운 장비를 얻고 다음 지역으로 이동하십시오.";
                    }
                    break;
                case 2:
                    {
                        Popup.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "적들과 보스 처치";
                        Popup.transform.GetChild(0).GetChild(2).GetComponent<Text>().text = "적들을 전부 처치하십시오.";
                    }
                    break;



            }
            
        }
        
        
    }

    public void ActivePopup()
    {
        if(!PlayerInformation.Action)
        { if (PopupCheck)
            {
                Popup.transform.GetChild(0).transform.position = Vector3.Lerp(Popup.transform.GetChild(0).transform.position, Popup.transform.GetChild(1).transform.position, Time.deltaTime * 2);
            }
            else
            {
                Popup.transform.GetChild(0).transform.position = Vector3.Lerp(Popup.transform.GetChild(0).transform.position, Popup.transform.GetChild(2).transform.position, Time.deltaTime * 2);
            }
        }
    }
    public void DelayPopup()
    {
        if (!PopupCheck)
        {
            PopupCheck = true;
        }
    


    }









    public void BulletView()
    {
        switch(PlayerInformation.Shoot_Manager.Equip_Gun.GunID)
        {
            case 3:
                BulletText.text = "";
                BulletMAXText.text = "";
                BulletText.transform.parent.GetChild(3).GetComponent<Slider>().value = PlayerInformation.Shoot_Manager.Equip_Gun.CurrentDelay / PlayerInformation.Shoot_Manager.Equip_Gun.Delay;
                break;
            default:
                BulletText.text = "" + PlayerInformation.Shoot_Manager.Equip_Gun.CurrentAmmo;
                BulletMAXText.text = "/" + PlayerInformation.Shoot_Manager.Equip_Gun.CurrentPack;
                BulletText.transform.parent.GetChild(3).GetComponent<Slider>().value = PlayerInformation.Shoot_Manager.Equip_Gun.CurrentAmmo / PlayerInformation.Shoot_Manager.Equip_Gun.CurrentReload;

                break;

        }

    }
    public void BoomView()
    {
        if(BoomCheck==true)
        {
            Boom.transform.GetChild(0).GetComponent<Text>().text = "" + PlayerInformation.Grenade_Manager.GrenadeCount;
            Boom.transform.GetChild(2).GetComponent<Slider>().value = PlayerInformation.Grenade_Manager.ThrowDelay / PlayerInformation.Grenade_Manager.ThrowMAXDelay;
        }
    }
    public void GunSwapImage(int OldGN,int GN)
    {

        switch (OldGN)
        {
            case 1:
                GunImage.transform.GetChild(0).gameObject.SetActive(false);
                break;
            case 2:
                GunImage.transform.GetChild(1).gameObject.SetActive(false);
                break;
            case 3:
                GunImage.transform.GetChild(2).gameObject.SetActive(false);
                break;
        }
        switch (GN)
        {
            case 1:
                GunImage.transform.GetChild(0).gameObject.SetActive(true);
                break;
            case 2:
                GunImage.transform.GetChild(1).gameObject.SetActive(true);
                break;
            case 3:
                GunImage.transform.GetChild(2).gameObject.SetActive(true);
                break;
        }
    }


    public void HPView()
    {
    Infos.HpText.text = Math.Truncate(Infos.Hp * 100f / Infos.MAXHp) + "%";
        Infos.oldHp = Mathf.Lerp(Infos.oldHp, Infos.Hp, Time.deltaTime);
        if (Infos.Hp == Infos.MAXHp)
        {
            HPImage.fillAmount = 1.0f;
        }
        HPImage.fillAmount = Infos.oldHp / Infos.MAXHp;
    }




    public void Regen()
    {
        if (Infos.Hit >= 0&Infos.Hp>0)
        {
            Infos.Hit -= Time.deltaTime;
        }
        else
        {
            Infos.Heal += Time.deltaTime;
            if (Infos.Heal >= 1)
            {
                Infos.Heal = 0;
                Infos.Hp += 1;
                if (Infos.Hp >= Infos.MAXHp)
                {
                    Infos.Hp = Infos.MAXHp;
                }
                Infos.oldHp = Infos.Hp;
            }
        }
    }

    public void BoomGet()
    {
        BoomCheck = true;
        
        Boom.SetActive(true);
        
        

    }


}
