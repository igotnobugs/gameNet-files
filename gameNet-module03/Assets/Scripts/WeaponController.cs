using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public bool weaponsActive;
    
    [Header("Weapons E")]
    public Weapon[] weapons;

    [Header("Set Up")]
    public Vehicle owner;


    // Update is called once per frame
    void Update()
    {
        if (!weaponsActive) return;
        // Weapons here
        if (Input.GetKey(KeyCode.E)) {
            if (weapons.Length <= 0) return;
            foreach (Weapon weap in weapons) {
                weap.Fire(owner.viewId);
            }
        }
    }

    public void DisableWeapons() {
        weaponsActive = false;
        foreach (Weapon weap in weapons) {
            weap.gameObject.SetActive(false);
        }
    }

    public void EnableWeapons() {     
        foreach (Weapon weap in weapons) {
            weap.gameObject.SetActive(true);
        }
        weaponsActive = true;
    }

}
