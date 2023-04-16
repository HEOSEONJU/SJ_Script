using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon_Slot : MonoBehaviour
{
    public bool _isRight;
    public bool _isLeft;

    //public Weapon_Item Weapon_Item;
    public Transform _handPosition;

    public Player_Weapon_Collider _weaponColldier;

    public GameObject _currentWeapon;


    public void Init(Weapon_Item Main_Weapon)
    {
        _handPosition = this.transform;
        Change_Weapon(Main_Weapon);
    }
    
    public void Destroy_Current_Weapon()
    {
        Destroy(_currentWeapon);
    }
    public void Change_Weapon(Weapon_Item temp)
    {
        
        if(_currentWeapon !=null)
        {
            
            Destroy(_currentWeapon);
        }
        
        if (temp != null)
        {
            _currentWeapon = Instantiate(temp.WeaponPrefab, _handPosition);
            _currentWeapon.transform.GetChild(0).GetChild(0).tag = "Weapon";
            _weaponColldier = _currentWeapon.GetComponentInChildren<Player_Weapon_Collider>();
        }
        

    }

    public void ableWeaponCollider(bool able)
    {
        if (_weaponColldier != null)
        {
            _weaponColldier.AbleCollider(able);
        }
    }



}
