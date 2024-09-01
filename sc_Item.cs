using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_Item : MonoBehaviour
{
    public sc_ItemData data;
    public int level;
    public sc_Weapon weapon;
    public sc_Gear gear;

    Image icon;
    Text textLevel;
    Text textName;
    Text textDesc;

    private void Awake()
    {
        // GetComponentsInChildren 첫번째 배열의 값은 자기 자신
        icon = GetComponentsInChildren<Image>()[1];
        icon.sprite = data.ItemIcon;

        // getcomponents의 배열 순서는 계층구조의 순서를 따라간다.
        Text[] texts = GetComponentsInChildren<Text>();
        textLevel = texts[0];
        textName = texts[1];
        textDesc = texts[2];

        textName.text = data.itemName;
    }

    private void OnEnable()
    {
        textLevel.text = "Lv." + (level);
        switch (data.itemType)
        {
            case sc_ItemData.ItemType.Melee:
            case sc_ItemData.ItemType.Range:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                break;
            case sc_ItemData.ItemType.Glove:
            case sc_ItemData.ItemType.Shoe:
                textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                break;
            default:
                textDesc.text = string.Format(data.itemDesc);
                break;
        }

    }
    public void OnClick()
    {
        switch (data.itemType)
        {
            case sc_ItemData.ItemType.Melee:
            case sc_ItemData.ItemType.Range:
                if(level == 0)
                {
                    GameObject newWeapon = new GameObject();
                    weapon = newWeapon.AddComponent<sc_Weapon>();
                    weapon.Init(data);
                }
                else
                {
                    float nextDamage = data.baseDamage;
                    int nextCount = 0;
                    nextDamage += data.baseDamage * data.damages[level];
                    nextCount += data.counts[level];
                    weapon.LevelUp(nextDamage,nextCount);
                }
                level++;
                break;
            case sc_ItemData.ItemType.Glove:
            case sc_ItemData.ItemType.Shoe:
                if(level == 0)
                {
                    GameObject newGear = new GameObject();
                    gear = newGear.AddComponent<sc_Gear>();
                    gear.Init(data);
                }
                else
                {
                    float nextRate = data.damages[level];
                    gear.LevelUp(nextRate);
                }
                level++;
                break;
            case sc_ItemData.ItemType.Heal:
                sc_GameManager.instance.health = sc_GameManager.instance.maxHealth;
                break;
        }
        if(level == data.damages.Length)
        {
            GetComponent<Button>().interactable = false;
        }
    }
}
