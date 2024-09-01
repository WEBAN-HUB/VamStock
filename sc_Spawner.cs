using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class sc_Spawner : MonoBehaviour
{
    public List<sc_SpawnPoint> spawnPoint;
    public SpawnData[] spawnData;
    public float levelTime;

    int level;
    float timer;

    private void Awake()
    {
        spawnPoint = GetComponentsInChildren<sc_SpawnPoint>().ToList();
        if (spawnPoint[0].gameObject.name == this.gameObject.name)
        {
            spawnPoint.RemoveAt(0);
        }

        levelTime = sc_GameManager.instance.maxGameTime / spawnData.Length;
    }
    void Update()
    {
        if (!sc_GameManager.instance.isLive)
            return;

        timer += Time.deltaTime;
        level = Mathf.Min(Mathf.FloorToInt(sc_GameManager.instance.gameTime / levelTime),spawnData.Length -1);

        if(timer > spawnData[level].spawnTime)
        {
            Spawn(0);
            //Spawn(UnityEngine.Random.Range(0,2));
            timer = 0f;
        }
    }

    void Spawn(int index)
    {
        for(int i = 0; i < spawnPoint.Count; i++)
        {
            int ranInt = UnityEngine.Random.Range(0, spawnPoint.Count);
            if (!spawnPoint[ranInt].isPossibleSpawn)
            {
                continue;
            }
            else
            {
                GameObject enemy = sc_GameManager.instance.PoolManager.Get(index);
                enemy.transform.position = spawnPoint[ranInt].transform.position;
                enemy.GetComponent<sc_Enemy>().Init(spawnData[level]);
                break;
            }
        }
    }
}

[Serializable]
public class SpawnData
{
    public int spriteType;
    public float spawnTime;
    public int health;
    public float speed;
    public float damage;
}