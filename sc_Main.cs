using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sc_Main : MonoBehaviour
{
    public int selectedCharacter;
    public static sc_Main instance;

    [Header("# Scene Name")]
    public string protoScene;
    public string mainScene;
    public string nextScene;

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

        Application.targetFrameRate = 60;
        Screen.SetResolution(3840, 2160, true);
        protoScene = "GamePlayproto";
        mainScene = "MainTitle";

        selectedCharacter = Mathf.Max(PlayerPrefs.GetInt("selectedCharacter"),1);
    }
    private void OnEnable()
    {
        // 씬마다 호출되는 함수 만들기
        // 1. 씬 매니저의 sceneLoaded에 체인을 건다
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        // 3. 이 스크립트가 포함된 오브젝트가 disable될 때 호출되는 함수를 체인 해제
        // SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void BtnGameStart()
    {
        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Select);

        nextScene = protoScene;
        SceneManager.LoadScene("Loading");
    }
    // 2. 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject objGameStart = GameObject.FindGameObjectWithTag("Button");
        if(objGameStart)
        {
            Button findedBtnGameStart = objGameStart.GetComponent<Button>();
            if (findedBtnGameStart)
            {
                if (findedBtnGameStart.gameObject.name == "BtnGameStart")
                {
                    // OnClick 함수에 OnClickStart 함수 연결
                    findedBtnGameStart.onClick.AddListener(BtnGameStart);
                }
            }
        }
    }
}
