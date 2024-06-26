using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public static MainManager Instance;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject PostGameUI;
    public GameObject HighScoreField;
    public TextMeshProUGUI HighScoreText;
    private MenuUIHandler MenuUIHandler;

    private bool m_Started = false;
    private int m_Points;
    [SerializeField] bool m_GameOver = false;

    public string playerName;
    void Start()
    {
        MenuUIHandler = GameObject.Find("MenuUIHandler").GetComponent<MenuUIHandler>();
        LoadName();
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = MenuUIHandler.playerName + "'s Score: " + m_Points;
    } 

    public void GameOver()
    {
        m_GameOver = true;
        PostGameUI.SetActive(true);
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int playerScore;
    }

    public void SaveName()
    {
        TMP_InputField scoreField = HighScoreField.GetComponent<TMP_InputField>();
        SaveData data = new SaveData();

        string path = Application.persistentDataPath + "/savefile.json";
        string json = File.ReadAllText(path);
        SaveData dataCheck = JsonUtility.FromJson<SaveData>(json);

        Debug.Log("Player Score: " + dataCheck.playerScore);
        if (dataCheck.playerScore < m_Points)
        {
            data.playerName = scoreField.text;
            data.playerScore = m_Points;

            json = JsonUtility.ToJson(data);

            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        }
    }

    public void LoadName()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            playerName = data.playerName;
            m_Points = data.playerScore;
            HighScoreText.SetText("Best Score : " + playerName + " : " + m_Points);
            m_Points = 0;
        }
    }
}
