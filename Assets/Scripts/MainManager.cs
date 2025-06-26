using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string m_playerName = "";
    private string m_bestScoreName = "";
    private int m_bestScore = 0;

    
    // Start is called before the first frame update
    void Start()
    {
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

        InitScoreText();
        SetBestScore();
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
        m_playerName = GameManager.instance.playerName;
        ScoreText.text = $"Score : {m_playerName} : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        
        if (CalculateIfNewBestScore())
        {
            m_bestScoreName = m_playerName;
            m_bestScore = m_Points;
            GameManager.instance.SetBestScore(m_bestScore, m_playerName);
            GameManager.instance.SaveGameData();
        }
    }


    public void InitScoreText()
    {
        m_Points = 0;
        m_playerName = GameManager.instance.playerName;
        ScoreText.text = $"Score : {m_playerName} : {m_Points}";
    }


    public bool CalculateIfNewBestScore()
    {
        return m_Points > m_bestScore;
    }


    public void SetBestScore()
    {
        m_bestScore = GameManager.instance.bestScore;
        m_bestScoreName = GameManager.instance.bestScoreName;
        BestScoreText.text = $"Best Score : {m_bestScoreName} : {m_bestScore}";
    }

}
