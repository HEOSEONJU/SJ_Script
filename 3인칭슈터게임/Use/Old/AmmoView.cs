using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class AmmoView : MonoBehaviour
{
    Gun FirstGun;
    public Text CP;
    public Text MP;
    void Start()
    {
        FirstGun= GetComponent<Gun>();
    }

    // Update is called once per frame
    void Update()
    {
        //T.text = FirstGun.CurrentAmmo + "/" + FirstGun.CurrentPack + "/" + GameManager.instance.Hp;
    }
}
