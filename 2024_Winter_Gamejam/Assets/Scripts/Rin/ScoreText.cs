using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText : MonoBehaviour
{
    int score = 0;
    Text ScoreT;

    private void Awake()
    {
        ScoreT = GetComponent<Text>();
    }
    private void AddScore()
    {
        score++;

    }
    private void ScoreTextAcc()
    {
        ScoreT.text = score.ToString();
    }
    public int GetScore()
    {
        return score;
    }

    string highScore = "HightScore";
    public int GetHighScore()
    {
        int hightScore = PlayerPrefs.GetInt(highScore);
        return hightScore;
    }
    public void SetHightScore(int cur_score)
    {
        if(cur_score > GetHighScore())
            PlayerPrefs.SetInt(highScore, cur_score);
    }
}
