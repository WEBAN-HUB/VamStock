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
        // ... 선택한 풀의 놀고(비활성화 된) 있는 게임 오브젝트 접근
        foreach(GameObject item in pools[index])
        {  
            // ... 발견하면 select 변수에 할당
            if (!item.activeSelf)
            {
                select = item;
                select.SetActive(true);
                break;
            }
        }    
          
        // ... 못 찾았으면?
        if(!select)
        {
            // ... 새롭게 생성하고 select 변수에 할당
            select = Instantiate(PF_Object[index], this.transform);
            pools[index].Add(select);
        }


        return select;
    }
}
