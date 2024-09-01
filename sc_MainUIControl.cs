using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sc_MainUIControl : MonoBehaviour
{
    [Header("# Button")]
    public GameObject UI_MainTitle;
    public GameObject UI_CharacterChange;
    public Button UI_BtnCharacterChange;

    public GameObject[] lockCharacter;
    public GameObject[] unlockCharacter;

    
    

    private void Start()
    {
        UnlockCharacter();
    }
    public void UnlockCharacter()
    {
        for (int index = 0; index < lockCharacter.Length; index++)
        {
            string archiveName = sc_ArchiveManager.instance.archives[index].ToString();
            bool isUnlocked = PlayerPrefs.GetInt(archiveName) == 1;
            lockCharacter[index].SetActive(!isUnlocked);
            unlockCharacter[index].SetActive(isUnlocked);
        }
    }

    public void BtnChangeCharacter(int characterNum)
    {
        sc_Main.instance.selectedCharacter = characterNum;
        PlayerPrefs.SetInt("selectedCharacter", characterNum);
        Image[] characterObjects = UI_CharacterChange.GetComponentsInChildren<Image>();
        if(characterObjects != null)
        {
            foreach (Image characterObject in characterObjects)
            {
                if (characterObject.gameObject.name.Equals("BtnCharacter" + sc_Main.instance.selectedCharacter.ToString()))
                {
                    characterObject.color = new Color(1, 199f / 255f, 152f / 255f);
                }
                else if (!characterObject.gameObject.name.Contains("_Lock"))
                {
                    characterObject.color = Color.white;
                }
            }
        }

        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Select);
    }


    public void BtnCharacterSelect()
    {
        ShowUI(UI_CharacterChange);
        HideUI(UI_MainTitle);

        Image[] characterObjects = UI_CharacterChange.GetComponentsInChildren<Image>();
        if (characterObjects != null)
        {
            foreach (Image characterObject in characterObjects)
            {
                if (characterObject.gameObject.name.Equals("BtnCharacter" + sc_Main.instance.selectedCharacter.ToString()))
                {
                    characterObject.color = new Color(1, 199f / 255f, 152f / 255f);
                }
                else if(!characterObject.gameObject.name.Contains("_Lock"))
                {
                    characterObject.color = Color.white;
                }
            }
        }

        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Select);
    }
    public void BtnMainMenu()
    {
        ShowUI(UI_MainTitle);
        HideUI(UI_CharacterChange);

        sc_AudioManager.instance.PlaySfx(sc_AudioManager.Sfx.Select);
    }

    public void BtnExit()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    public void ShowUI(GameObject UIGameObject)
    {
        UIGameObject.SetActive(true);
    }
    public void HideUI(GameObject UIGameObject)
    {
        UIGameObject.SetActive(false);
    }
}
