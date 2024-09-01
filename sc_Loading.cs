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
        AsyncOperation op = SceneManager.LoadSceneAsync(name); // �񵿱� Scene �ε� ( �ε��� Scene �̸� )
        op.allowSceneActivation = false;  // Scene �� �ε� �Ǿ����� �ٷ� �������� .
        yield return new WaitForSecondsRealtime(2.1f); // 2�� ���

        // AsyncOperation.progress -> �ε� ���������� 0.9�� �ִ밪�̱� ������ 0.9�� �Ǳ� ������ ��ٸ���.
        while (op.progress < 0.9f)
        {
            yield return new WaitForSecondsRealtime(0.1f);
        }
        op.allowSceneActivation = true; // �ε��� Scene ����.
    }
}
