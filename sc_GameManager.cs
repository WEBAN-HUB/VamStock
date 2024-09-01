using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc_GameManager : MonoBehaviour
{
    public static sc_GameManager instance;
    [Header("# Game Control")]
    public bool isLive;
    public float gameTime;
    public float maxGameTime;
    [Header("# Player Info")]
    public int playerId;
    public float health;
    public float maxHealth = 100;
    public int level;
    public int kill;
    public int exp;
    public int[] nextExp = { 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };
    [Header("# Game Object")]
    public sc_player Player;
    public sc_PoolManager PoolManager;
    public sc_LevelUp UI_LevelUp;
    public GameObject UI_Result;
    public GameObject UI_GameClear;
    public Transform UI_Joy;
    [Header("# Map Create")]
    public GameObject MapParent;
    public float map_width_left = 0;
    public float map_width_right = 0;
    public GameObject[] PF_MAP;
    public List<GameObject> MapList;
    private int MapSize = 15;

    private void Awake()
    {
        if (sc_GameManager.instance == null)
        {
            sc_GameManager.instance = this;
            //this 이 스크립트 자신, gameObject 이 스크립트를 컴포넌트로 사용하는 게임 오브젝트
            // DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        Player = FindObjectOfType<sc_player>();
        PoolManager = FindObjectOfType<sc_PoolManager>();

        isLive = true;
        playerId = sc_Main.instance.selectedCharacter;
        maxGameTime = 60f;
    }

    private void Start()
    {
        health = maxHealth;

        if (playerId == 1 || playerId == 4)
        {
            UI_LevelUp.Select(0);
        }
        else if (playerId == 2 || playerId == 3)
        {
            UI_LevelUp.Select(1);
        }
        // 맵 생성 로직 
        int randomInt;
        for (int i = 0; i < MapSize; i++)
        {
            randomInt = Random.Range(0, PF_MAP.Length);

            if (i == 0)
            {
                GameObject instance = Instantiate(PF_MAP[randomInt], MapParent.transform.position, Quaternion.identity);
                instance.transform.parent = MapParent.transform;
                MapList.Add(instance);
            }
            else if (i % 2 == 1)
            {
                map_width_left -= PF_MAP[randomInt].transform.Find("GroundTrigger").GetComponent<BoxCollider2D>().size.x;
                Vector3 mapPos = MapParent.transform.position;
                mapPos.x = map_width_left;
                GameObject instance = Instantiate(PF_MAP[randomInt], mapPos, Quaternion.identity);
                instance.transform.parent = MapParent.transform;
                MapList.Add(instance);

            }
            else if (i % 2 == 0)
            {
                map_width_right += PF_MAP[randomInt].transform.Find("GroundTrigger").GetComponent<BoxCollider2D>().size.x;
                Vector3 mapPos = MapParent.transform.position;
                mapPos.x = map_width_right;
                GameObject instance = Instantiate(PF_MAP[randomInt], mapPos, Quaternion.identity);
                instance.transform.parent = MapParent.transform;
                MapList.Add(instance);
            }
            MapList = MapList.OrderBy(map => map.transform.position.x).ToList();
        }

        sc_AudioManager.instance.PlayBgm(true);
    }

    private void Update()
    {
        if(!isLive)
        {
            return;
        }
        gameTime += Time.deltaTime;

        if(gameTime > maxGameTime)
        {
            gameTime = maxGameTime;
            GameClear();
        }
    }

    public void GetExp(int mExp)
    {
        exp += mExp;
        if(exp >= nextExp[Mathf.Min(level,nextExp.Length-1)])
        {
            exp = exp - nextExp[Mathf.Min(level, nextExp.Length - 1)];
            level++;
            UI_LevelUp.Show();
        }
    }

    public void GameOver()
    {
        StartCoroutine(GameOverRoutine());
    }

    IEnumerator GameOverRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.3f);
        UI_Result.SetActive(true);
        Stop();

        sc_AudioManager.instance.PlayBgm(false);
        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Lose);
    }

    public void GameClear()
    {
        StartCoroutine(GameClearRoutine());
    }
    IEnumerator GameClearRoutine()
    {
        isLive = false;
        yield return new WaitForSeconds(0.5f);
        UI_GameClear.SetActive(true);
        Stop();

        sc_AudioManager.instance.PlayBgm(false);
        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Win);
    }

    public void Stop()
    {
        isLive = false;
        Time.timeScale = 0;
        //UI_Joy.localScale = Vector3.zero;
    }

    public void Resume()
    {
        isLive = true;
        Time.timeScale = 1;
        //UI_Joy.localScale = Vector3.one;
    }

    public void BtnReturnToMainMenu()
    {
        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Select);
        sc_Main.instance.nextScene = sc_Main.instance.mainScene;
        Resume();
        SceneManager.LoadScene("Loading");
    }
}
