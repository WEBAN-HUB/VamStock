using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Scanner : MonoBehaviour
{
    // 범위
    public float scanRange;
    // 레이어
    public LayerMask targetLayer;
    // 스캔 결과 배열
    public RaycastHit2D[] targets;
    // 목표물로 정할 가장 가까운 적 transform
    public Transform targetTransform;

    private void FixedUpdate()
    {
        // CircleCastAll 원형의 캐스트를 쏘고 모든 결과를 반환함
        // 매개 변수 1. 시작 위치 2. 원의 반지름 3. 캐스팅 방향 4. 캐스팅 길이 5. 대상 레이어
        targets = Physics2D.CircleCastAll(transform.position,scanRange,Vector2.zero,0,targetLayer);
        targetTransform = GetNearest();
    }

    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100;

        foreach (RaycastHit2D target in targets)
        {
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            // Vector3.Distance(A,B) 벡터 A와 B의 거리를 계산하는 함수 
            /*
            float num = A.x - B.x;
            float num2 = A.y - B.y;
            float num3 = A.z - B.z;
            return (float)Math.Sqrt(num * num + num2 * num2 + num3 * num3);
            */
            float curdiff = Vector3.Distance(myPos,targetPos);
            if(curdiff < diff)
            {
                diff = curdiff;
                result = target.transform;
            }
        }
        return result;
    }
}
