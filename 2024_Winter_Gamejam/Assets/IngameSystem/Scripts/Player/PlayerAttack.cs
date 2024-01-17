using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//x키를 누르면 바라보는 방향으로 공격을 함
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject m_fireball;
    [SerializeField] private PlayerColliisionCheck m_collisionCheck;
    [SerializeField] private AudioClip m_AttackSound;
    [SerializeField] private float m_delay = 2;
    private float m_timer = 0;
    private GameObject[] m_fireballSets;
    private int m_index = 0;

    private void Awake()
    {
        m_fireballSets = new GameObject[5];
        m_index = m_fireballSets.Length;
        for(int i = 0; i < m_index; i++)
        {
            m_fireballSets[i] = Instantiate(m_fireball);
            m_fireballSets[i].SetActive(false);
        }
    }

    private void Update()
    {
        if (m_timer < m_delay)
        {
            m_timer += Time.deltaTime;
        }
        else if (m_timer >= m_delay && Input.GetKeyDown(KeyCode.X))
        {
            //공격
            ++m_index;
            if(m_index >= m_fireballSets.Length)
            {
                m_index = 0;
            }
            m_fireballSets[m_index].transform.position = this.transform.position;
            m_fireballSets[m_index].transform.rotation = this.transform.rotation;
            m_fireballSets[m_index].SetActive(true);
            m_collisionCheck.PlaySound(m_AttackSound);
            m_timer = 0;
        }
    }
}
