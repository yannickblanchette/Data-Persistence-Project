using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager instance {  get; private set; }

    public TextMeshProUGUI bestScoreText;

    public string playerName { get; private set; }
    public int bestScore { get; private set; }
    public string bestScoreName { get; private set; }

    public const int sceneNumberMenu = 0;
    public const int sceneNumberMain = 1;


    private void Awake()
    {
        // start of new code
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        // end of new code

        instance = this;
        DontDestroyOnLoad(gameObject);

        playerName = "";
        bestScore = 0;
        bestScoreName = "";
        LoadGameData();
        DisplayBestScore();
    }


    public void SetPlayerName(string playerName)
    {
        this.playerName = playerName;
        Debug.Log($"GameManager.SetPlayerName {this.playerName}");
    }


    public void SetBestScore(int bestScore, string playerName)
    {
        this.bestScore = bestScore;
        this.bestScoreName = playerName;
        Debug.Log($"GameManager.SetBestScore {this.bestScoreName} {this.bestScore}");
    }


    [System.Serializable]
    class SaveData
    {
        public int bestScore;
        public string bestScoreName;
    }


    public void SaveGameData()
    {
        SaveData data = new SaveData();
        data.bestScore = bestScore;
        data.bestScoreName = bestScoreName;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
        Debug.Log($"GameManager.SaveGameData {this.bestScoreName} {this.bestScore}");
    }


    public void LoadGameData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            this.bestScore = data.bestScore;
            this.bestScoreName = data.bestScoreName;
            Debug.Log($"GameManager.LoadGameData {this.bestScoreName} {this.bestScore}");
        }
    }


    public void StartGame()
    {
        //Check if the player entered his/her name in the field. If not don't start the game yet
        if (playerName == "")
        {
            return;
        }

        SceneManager.LoadScene(sceneNumberMain);
    }


    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit(); // original code to quit Unity player
#endif
    }

    public void DisplayBestScore()
    {
        bestScoreText.text = $"Best Score: {bestScoreName}: {bestScore}";
    }
}
