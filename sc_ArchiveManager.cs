using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class sc_ArchiveManager : MonoBehaviour
{
    public static sc_ArchiveManager instance;

    public GameObject uiNotice;

    public enum Archive
    {
        UnlockPlayer3,
        UnlockPlayer4
    }

    public Archive[] archives;
    WaitForSecondsRealtime wait;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        // enum 값 배열로 초기화 하는 법
        archives = (Archive[])Enum.GetValues(typeof(Archive));
        
        if(!PlayerPrefs.HasKey("MyData"))
        {
            Init();
        }
        wait = new WaitForSecondsRealtime(5f);
    }

    void Init()
    {
        // PlayerPrefs : 간단한 저장 기능을 제공하는 유니티 제공 클래스
        // SetInt : Key와 값으로 int형 데이터를 저장
        PlayerPrefs.SetInt("MyData", 1);

        /*PlayerPrefs.SetInt("UnlockPlayer3", 0);
        PlayerPrefs.SetInt("UnlockPlayer4", 0);*/
        foreach (Archive archive in archives) 
        {
            PlayerPrefs.SetInt(archive.ToString(), 0);
        }


    }
    // Start is called before the first frame update
    void Start()
    {

    }

    private void LateUpdate()
    {
        if (SceneManager.GetActiveScene().name.Equals("GamePlayproto"))
        {
            foreach (Archive archive in archives)
            {
                CheckArchive(archive);
            }
        }
    }

    void CheckArchive(Archive archive)
    {
        bool isArchive = false;

        switch (archive)
        {
            case Archive.UnlockPlayer3:
                isArchive = sc_GameManager.instance.kill >= 10;
                break;
            case Archive.UnlockPlayer4:
                isArchive = sc_GameManager.instance.gameTime >= sc_GameManager.instance.maxGameTime;
                break;
        }
        if(isArchive && PlayerPrefs.GetInt(archive.ToString()) == 0)
        {
            PlayerPrefs.SetInt(archive.ToString(), 1);

            for (int index = 0; index < uiNotice.transform.childCount; index++)
            {
                bool isActive = index == (int)archive;
                uiNotice.transform.GetChild(index).gameObject.SetActive(isActive);
            }
            StartCoroutine(NoticeRoutine());
        }
    }

    IEnumerator NoticeRoutine()
    {
        uiNotice.SetActive(true);

        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.LevelUp);
        yield return wait;
        uiNotice.SetActive(false);
    }
}
