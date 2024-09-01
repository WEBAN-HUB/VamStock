using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_LevelUp : MonoBehaviour
{
    RectTransform rect;
    sc_Item[] items;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
        items = GetComponentsInChildren<sc_Item>(true);
    }

    public void Show()
    {
        Next();
        rect.localScale = Vector3.one;
        sc_GameManager.instance.Stop();
        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.LevelUp);
        sc_AudioManager.instance.EffectBgm(true);
    }

    public void Hide()
    {
        rect.localScale = Vector3.zero;
        sc_GameManager.instance.Resume();
        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Select);
        sc_AudioManager.instance.EffectBgm(false);
    }

    public void Select(int index)
    {
        items[index].OnClick();
    }

    void Next()
    {
        // 1. 모든 아이템 비활성화
        foreach (sc_Item item in items)
        {
            item.gameObject.SetActive(false);
        }
        // 2. 랜덤으로 3개 아이템 활성화
        int[] ran = new int[3];
        while (true)
        {
            ran[0] = Random.Range(0, items.Length);
            ran[1] = Random.Range(0, items.Length);
            ran[2] = Random.Range(0, items.Length);
            if (ran[0] != ran[1] && ran[1] != ran[2] && ran[0] != ran[2])
                break;
        }

        for (int index = 0; index < ran.Length; index++)
        {
            sc_Item ranItem = items[ran[index]];

            // 3. 만렙 아이템의 경우는 소비 아이템으로 대체
            if (ranItem.level == ranItem.data.damages.Length)
            {
                items[4].gameObject.SetActive(true);
            }
            else
            {
                ranItem.gameObject.SetActive(true);
            }
        }
    }
}
