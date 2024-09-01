using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class sc_Loading : MonoBehaviour
{
    [SerializeField] Slider Loading_bar;
    [SerializeField] float time;
    [SerializeField] string scenenames;
    [SerializeField] GameObject Loading_bar_text;

    public RuntimeAnimatorController[] animCon;
    public Animator animator;
    private void Awake()
    {
        scenenames = sc_Main.instance.nextScene;
        Loading_bar.maxValue = 2.0f;
        StartCoroutine(LoadScene(scenenames));

        animator.runtimeAnimatorController = animCon[sc_Main.instance.selectedCharacter - 1];
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        Loading_bar.value = time;
        Loading_bar_text.transform.GetComponent<Text>().text = "Loading... " + Mathf.Min((Mathf.Floor(time * 100f) / 200f),1) * 100f + "%";

    }

    IEnumerator LoadScene(string name)
    {
        yield return null;
        AsyncOperation op = SceneManager.LoadSceneAsync(name); // 비동기 Scene 로딩 ( 로딩할 Scene 이름 )
        op.allowSceneActivation = false;  // Scene 이 로딩 되었을때 바로 실행할지 .
        yield return new WaitForSecondsRealtime(2.1f); // 2초 대기

        // AsyncOperation.progress -> 로딩 진행정도가 0.9가 최대값이기 때문에 0.9가 되기 전까지 기다린다.
        while (op.progress < 0.9f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        op.allowSceneActivation = true; // 로딩된 Scene 실행.
    }
}
