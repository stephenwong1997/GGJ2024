using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponContoller : MonoBehaviour
{
    [SerializeField] private GameObject _rocket;
    [SerializeField] private GameObject _grenade;

    private void OnValidate()
    {
        if (_rocket == null) Debug.LogError($"{this.gameObject.name}: _rocket null!");
        if (_grenade == null) Debug.LogError($"{this.gameObject.name}: _grenade null!");
    }

    private void Awake()
    {
        DisableAllWeapons();
    }

    public void EnableWeapon(EItemType itemType)
    {
        // Switch off everything first
        DisableAllWeapons();

        switch (itemType)
        {
            case EItemType.Rocket: _rocket.SetActive(true); return;
            case EItemType.Grenade: _grenade.SetActive(true); return;
        }
    }

    public void DisableAllWeapons()
    {
        _rocket.SetActive(false);
        _grenade.SetActive(false);
    }
}
