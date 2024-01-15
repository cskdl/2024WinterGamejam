using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClearPanel : MonoBehaviour
{
    [SerializeField]
    public Text Text_clearPanel;

    private void Awake()
    {
        transform.gameObject.SetActive(true);
    }
    public void Show()
    {
        Debug.Log("Move");

        transform.gameObject.SetActive(true);

        int score = FindObjectOfType<ScoreText>().GetScore();
        int highScore = FindObjectOfType<ScoreText>().GetHighScore();

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
