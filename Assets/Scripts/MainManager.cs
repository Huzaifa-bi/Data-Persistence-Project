using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    public Text nameText;
    public Text highestScoreText;

    private bool m_Started = false;
    private int m_Points;
    private int currentScore;
    private string playerName;

    private bool m_GameOver = false;

    private void Awake()
    {
        SetPlayerName(GameManager.Instance.userName);
        LoadPlayerData();
    }

    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
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
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        UpdateScore(m_Points);
        m_GameOver = true;
        GameOverText.SetActive(true);
    }

    public void SetPlayerName(string name)
    {
        playerName = name;
        nameText.text = "Name: " + playerName;
        LoadPlayerData();
    }

    private void LoadPlayerData()
    {
        int highScore = PlayerPrefs.GetInt(playerName + "_highScore", 0);
        highestScoreText.text = "Best Score: " + highScore.ToString();
    }

    public void UpdateScore(int score)
    {
        currentScore = score;
        int highScore = PlayerPrefs.GetInt(playerName + "_highScore", 0);

        if (currentScore > highScore)
        {
            PlayerPrefs.SetInt(playerName + "_highScore", currentScore);
            PlayerPrefs.Save();
            highestScoreText.text = "Best Score: " + currentScore.ToString();
        }
    }
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
