using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public int PlayerHP { get; private set; } = 3;
    public int Score { get; private set; } = 0;

    [SerializeField] private int m_scorePerSecond = 3;

    private float m_scoreTimer = 0;

    private void Update()
    {
        m_scoreTimer += Time.deltaTime;
        if(m_scoreTimer >= 1)
        {
            m_scoreTimer = 0;
            Score += m_scorePerSecond;
        }
    }

    public void UpdateScore(int pScore = 100)
    {
        Score += pScore;
        Debug.Log($"Score is now {Score}");
    }

    public void UpdateHP(int pDamage = 1)
    {
        PlayerHP -= pDamage;
        Debug.Log($"HP is now {PlayerHP}");
    }
}