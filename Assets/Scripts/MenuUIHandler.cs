using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuUIHandler : MonoBehaviour
{
    public static MenuUIHandler Instance;
    public string playerName = "XYZ";
    public GameObject playerNameField;
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    public void Exit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
        MainManager.Instance.SaveName();
    }
   
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void UpdatePlayerName()
    {
        TMP_InputField nameField = playerNameField.GetComponent<TMP_InputField>();
        playerName = nameField.text;
        Debug.Log("Current Stored Player Name" + playerName);
    }
}
