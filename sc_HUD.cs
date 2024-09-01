using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Cinemachine.DocumentationSortingAttribute;

public class sc_HUD : MonoBehaviour
{
    // UI ���� �޸�
    // Rect Transform�� Anchor ����� Shift = ������ ����, Alt = ��ġ(ũ��) ����
    // 

    public enum InfoType
    {
        Exp,
        Level,
        Kill,
        Time,
        Health
    }
    public InfoType type;

    Text myText;
    Slider mySlider;

    private void Awake()
    {
        myText = GetComponent<Text>();
        mySlider = GetComponent<Slider>();   
    }

    private void LateUpdate()
    {
        switch (type)
        {
            case InfoType.Exp:
                float curExp = sc_GameManager.instance.exp;
                float maxExp = sc_GameManager.instance.nextExp[Mathf.Min(sc_GameManager.instance.level, sc_GameManager.instance.nextExp.Length - 1)];
                mySlider.value = curExp / maxExp;
                break;
            case InfoType.Level:
                myText.text = string.Format("Lv.{0:F0}", sc_GameManager.instance.level + 1); 
                break;
            case InfoType.Kill:
                myText.text = string.Format("{0:F0}", sc_GameManager.instance.kill);
                break;
            case InfoType.Time:
                float remainTime = sc_GameManager.instance.maxGameTime - sc_GameManager.instance.gameTime;
                int min = Mathf.FloorToInt(remainTime / 60);
                int sec = Mathf.FloorToInt(remainTime % 60);
                myText.text = string.Format("{0:D2}:{1:D2}", min, sec);
                break;
            case InfoType.Health:
                float curHealth = sc_GameManager.instance.health;
                float maxHealth = sc_GameManager.instance.maxHealth;
                mySlider.value = curHealth / maxHealth;
                break;
        }   
    }
}
