using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearPanel : MonoBehaviour
{
    [SerializeField]
    public Text Text_clearPanel;

    private string m_highScore = "HightScore";
    private string m_scoreString = "Score";

    private void Awake()
    {
        transform.gameObject.SetActive(true);
    }
    public void Show()
    {
        Debug.Log("Move");

        transform.gameObject.SetActive(true);

        //int score = FindObjectOfType<ScoreText>().Score;
        //int highScore = FindObjectOfType<ScoreText>().GetHighScore();
        int score = PlayerPrefs.GetInt(m_scoreString);
        int highScore = PlayerPrefs.GetInt(m_highScore);

        Debug.Log(score);
        Debug.Log(highScore);

        Text_clearPanel.text =
            "HighScore : " + highScore.ToString() + "\n" +
            "Score : " + score.ToString();


    }

    public void Start()
    {
        Show();
    }
    public void OnClick_Retry()
    {
        //SceneManager.LoadScene("GameScene"); 
    }

}
