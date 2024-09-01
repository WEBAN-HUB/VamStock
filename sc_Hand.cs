using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Hand : MonoBehaviour
{
    public bool isLeft;
    public SpriteRenderer spriter;

    SpriteRenderer player;

    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f,-0.15f,0);
    Quaternion leftRot = Quaternion.Euler(0,0,-35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);

    private void Awake()
    {
        // 자기 자신에게 spriterenderer가 있을 경우 첫 번째 배열로 들어간다
        player = GetComponentsInParent<SpriteRenderer>()[1];

    }

    private void LateUpdate()
    {
        bool isReverse = player.flipX;

        if (isLeft) // 근접무기
        {
            transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse;
            spriter.sortingOrder = isReverse ? 6 : 8;
        }
        else // 원거리 무기
        {
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            spriter.flipX = isReverse;
            spriter.sortingOrder = isReverse ? 8 : 6;
        }
    }
}
