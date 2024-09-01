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
        // ������ ȣ��Ǵ� �Լ� �����
        // 1. �� �Ŵ����� sceneLoaded�� ü���� �Ǵ�
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnDisable()
    {
        // 3. �� ��ũ��Ʈ�� ���Ե� ������Ʈ�� disable�� �� ȣ��Ǵ� �Լ��� ü�� ����
        // SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void BtnGameStart()
    {
        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Select);

        nextScene = protoScene;
        SceneManager.LoadScene("Loading");
    }
    // 2. ü���� �ɾ �� �Լ��� �� ������ ȣ��ȴ�.
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
                    // OnClick �Լ��� OnClickStart �Լ� ����
                    findedBtnGameStart.onClick.AddListener(BtnGameStart);
                }
            }
        }
    }
}
