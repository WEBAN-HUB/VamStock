using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Character : MonoBehaviour
{
    public static float Speed
    {
        get { return sc_GameManager.instance.playerId == 1 ? 1.1f : 1f;}
    }

    public static float WeaponSpeed
    {
        get { return sc_GameManager.instance.playerId == 2 ? 1.1f : 1f; }
    }

    public static float WeaponRate
    {
        get { return sc_GameManager.instance.playerId == 2 ? 0.9f : 1f; }
    }
    public static float Damage
    {
        get { return sc_GameManager.instance.playerId == 3 ? 1.2f : 1f; }
    }
    public static int Count
    {
        get { return sc_GameManager.instance.playerId == 4 ? 1 : 0; }
    }
}
