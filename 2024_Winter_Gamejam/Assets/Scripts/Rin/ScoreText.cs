using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText : MonoBehaviour
{
    int score = 10;
    Text ScoreT;

    private void Awake()
    {
        ScoreT = GetComponent<Text>();
    }
    private void AddScore()
    {
        //여의주하고 시간에 따라 스코어 증가하는 코드 작성 요망
        Debug.Log(score);
        ScoreTextAcc();

    }

    private void Start()
    {
        AddScore();
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
