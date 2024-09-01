using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class sc_PoolManager : MonoBehaviour
{
    public GameObject[] PF_Object;

    public List<GameObject>[] pools;

    private void Awake()
    {
        pools = new List<GameObject>[PF_Object.Length];

        for (int index = 0; index < pools.Length; index++)
        {
            pools[index] = new List<GameObject>();
        }
    }

    public GameObject Get(int index)
    {
        GameObject select = null;
        // ... ������ Ǯ�� ���(��Ȱ��ȭ ��) �ִ� ���� ������Ʈ ����
        foreach(GameObject item in pools[index])
        {  
            // ... �߰��ϸ� select ������ �Ҵ�
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }    
          
        // ... �� ã������?
        if(!select)
        {
            // ... ���Ӱ� �����ϰ� select ������ �Ҵ�
            select = Instantiate(PF_Object[index], this.transform);
            pools[index].Add(select);
        }


        return select;
    }
}
