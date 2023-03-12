using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy_Text : MonoBehaviour
{

    public TextMeshProUGUI Heal;
    public TextMeshProUGUI Damage;
    public Animator anim;

    
    void Awake()
    {
        anim = GetComponent<Animator>();
        
        
    }
    

    public void Setting(int value,int Type)
    {
        switch (Type)
        {
            case 0:
                anim.SetTrigger("Heal");
                Heal.text = "" + value;
                break;
            case 1:
                anim.SetTrigger("Damage");//����������
                //Debug.Log("�������򺯰�");
                Damage.color=Color.red;
                Damage.text = "" + value;
                break;
            case 2:
                anim.SetTrigger("Damage");//����������
                //Debug.Log("�������򺯰�");
                Damage.color= Color.blue;
                Damage.text = "" + value;
                break;
            case 3:
                anim.SetTrigger("Damage");//ũ��Ƽ��
                //Debug.Log("ġ��Ÿ���򺯰�");

                Color Orange = new Color();
                Orange.r = 1;
                Orange.g = 127 / 255f;
                Orange.b = 0;
                Orange.a = 1;
                Damage.color = Orange;
                

                Damage.text = "" + value;
                break;
            default:
                break;

        }


    }
    
}
