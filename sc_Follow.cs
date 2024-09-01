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
        // WorldToScreenPoint = 월드 상의 오브젝트 위치를 스크린 좌표로 변환
        rect.position = Camera.main.WorldToScreenPoint(sc_GameManager.instance.Player.transform.position);
    }
}
