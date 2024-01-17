using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ScoreText : MonoBehaviour
{
    public int Score { get; private set; } = 0;
    Text ScoreT;

    string highScore = "HightScore";
    string m_scoreString = "Score";

    private void Awake()
    {
        ScoreT = GetComponent<Text>();
    }
    public void AddScore(int pScore)
    {
        //�������ϰ� �ð��� ���� ���ھ� �����ϴ� �ڵ� �ۼ� ���(20240117�ۼ� ��)
        Score += pScore;
        //Debug.Log(score);
        ScoreTextAcc();

    }

    private void Start()
    {
        AddScore(0);
    }
    private void ScoreTextAcc()
    {
        ScoreT.text = Score.ToString();
    }
    public int GetHighScore()
    {
        int hightScore = PlayerPrefs.GetInt(highScore);
        return hightScore;
    }
    public void SetHightScore(int cur_score)
    {
        if(cur_score > GetHighScore())
            PlayerPrefs.SetInt(highScore, cur_score);
        PlayerPrefs.SetInt(m_scoreString, cur_score);
    }
}
