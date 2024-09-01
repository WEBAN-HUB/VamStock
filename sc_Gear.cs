using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Gear : MonoBehaviour
{
    public sc_ItemData.ItemType type;
    public float rate;

    public void Init(sc_ItemData data)
    {
        // Basic set
        name = "Gear" + data.itemId;
        transform.parent = sc_GameManager.instance.Player.transform;
        transform.localPosition = Vector3.zero;

        // Property set
        type = data.itemType;
        rate = data.damages[0];
        ApplyGear();
    }

    public void LevelUp(float rate)
    {
        this.rate = rate;
        ApplyGear();
    }

    void ApplyGear()
    {
        switch (type)
        {
            case sc_ItemData.ItemType.Glove:
                RateUp();
                break;
            case sc_ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        sc_Weapon[] weapons = transform.parent.GetComponentsInChildren<sc_Weapon>();

        foreach(sc_Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    float speed = 150 * sc_Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                default:
                    float weaponRate = 0.5f * sc_Character.WeaponRate;
                    weapon.speed = weaponRate * (1f - rate);
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 5 * sc_Character.Speed;
        sc_GameManager.instance.Player.playerSpeed = speed + (speed * rate);
    }
}
