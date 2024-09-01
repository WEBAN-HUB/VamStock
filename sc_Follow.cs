using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Follow : MonoBehaviour
{
    RectTransform rect;


    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void FixedUpdate()
    {
        // WorldToScreenPoint = ���� ���� ������Ʈ ��ġ�� ��ũ�� ��ǥ�� ��ȯ
        rect.position = Camera.main.WorldToScreenPoint(sc_GameManager.instance.Player.transform.position);
    }
}
