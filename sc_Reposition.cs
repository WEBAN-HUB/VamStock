using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static Cinemachine.DocumentationSortingAttribute;

public class sc_Reposition : MonoBehaviour
{
    Collider2D coll;
    public static float width_Ground = 40;

    private void Awake()
    {
        coll = GetComponent<Collider2D>();
    }

    private void Update()
    {

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Area"))
            return;

        Vector3 playerPos = sc_GameManager.instance.Player.transform.position;
        Vector3 myPos = this.transform.position;

        float distanceX = Mathf.Abs(playerPos.x - myPos.x);
        float distanceY = Mathf.Abs(playerPos.y - myPos.y);

        Vector3 playerDir = sc_GameManager.instance.Player.inputVec;
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;

        switch (transform.tag)
        {
            case "Ground":
                int listCount = sc_GameManager.instance.MapList.Count;
                if (dirX == -1)
                {
                    GameObject lastMap = sc_GameManager.instance.MapList[listCount-1];
                    sc_GameManager.instance.map_width_left -= lastMap.transform.Find("GroundTrigger").GetComponent<BoxCollider2D>().size.x;
                    sc_GameManager.instance.map_width_right -= lastMap.transform.Find("GroundTrigger").GetComponent<BoxCollider2D>().size.x;
                    Vector3 movePos = sc_GameManager.instance.MapList[0].transform.position;
                    movePos.x = sc_GameManager.instance.map_width_left;
                    lastMap.transform.position = movePos;
                    sc_GameManager.instance.MapList.RemoveAt(listCount-1);
                    sc_GameManager.instance.MapList.Insert(0,lastMap);
                }
                else if (dirX == 1)
                {
                    GameObject firstMap = sc_GameManager.instance.MapList[0];
                    sc_GameManager.instance.map_width_right += firstMap.transform.Find("GroundTrigger").GetComponent<BoxCollider2D>().size.x;
                    sc_GameManager.instance.map_width_left += firstMap.transform.Find("GroundTrigger").GetComponent<BoxCollider2D>().size.x;
                    Vector3 movePos = sc_GameManager.instance.MapList[^1].transform.position;
                    movePos.x = sc_GameManager.instance.map_width_right;
                    firstMap.transform.position = movePos;
                    sc_GameManager.instance.MapList.Add(firstMap);
                    sc_GameManager.instance.MapList.RemoveAt(0);
                }
                break;
            case "Enemy":
                if (coll.enabled)
                {
                    List<sc_SpawnPoint> spawnPoint = FindObjectOfType<sc_Spawner>().spawnPoint;
                    for (int i = 0; i < spawnPoint.Count; i++)
                    {
                        int ranInt = UnityEngine.Random.Range(0, spawnPoint.Count);
                        if (!spawnPoint[ranInt].isPossibleSpawn)
                        {
                            continue;
                        }
                        else
                        {
                            this.transform.position = spawnPoint[ranInt].transform.position;
                        }
                    }
                }
                break;
        }
    }
}
