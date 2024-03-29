using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public int PlayerHP { get; private set; } = 3;
    public int Score { get; private set; } = 0;

    [SerializeField] private int m_scorePerSecond = 3;
    [SerializeField] private LifeSetting m_lifeSetting;
    [SerializeField] private GameDirector m_gameDirector;
    [SerializeField] private ScoreText m_scoreText;

    private float m_scoreTimer = 0;
    private float m_countDown = 3;

    private void Update()
    {
        if (m_countDown > 0) m_countDown -= Time.deltaTime;
        if (m_countDown <= 0) m_scoreTimer += Time.deltaTime;
        if(m_scoreTimer >= 1)
        {
            m_scoreTimer = 0;
            Score += m_scorePerSecond;
            if (m_scoreText != null) m_scoreText.AddScore(3);
        }
    }

    public void UpdateScore(int pScore = 100)
    {
        Score += pScore;
        //Debug.Log($"Score is now {Score}");
        if (m_scoreText != null) m_scoreText.AddScore(pScore);
    }

    public void UpdateHP(int pDamage = 1)
    {
        PlayerHP -= pDamage;
        //Debug.Log($"HP is now {PlayerHP}");
        if(m_lifeSetting != null) m_lifeSetting.playerLife--;
        if (m_gameDirector != null) m_gameDirector.UpdateLives(m_lifeSetting.playerLife);
    }
}
