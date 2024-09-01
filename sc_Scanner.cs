using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sc_Scanner : MonoBehaviour
{
    // ����
    public float scanRange;
    // ���̾�
    public LayerMask targetLayer;
    // ��ĵ ��� �迭
    public RaycastHit2D[] targets;
    // ��ǥ���� ���� ���� ����� �� transform
    public Transform targetTransform;

    private void FixedUpdate()
    {
        // CircleCastAll ������ ĳ��Ʈ�� ��� ��� ����� ��ȯ��
        // �Ű� ���� 1. ���� ��ġ 2. ���� ������ 3. ĳ���� ���� 4. ĳ���� ���� 5. ��� ���̾�
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
            // Vector3.Distance(A,B) ���� A�� B�� �Ÿ��� ����ϴ� �Լ� 
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
