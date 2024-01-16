using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] private float m_duration = 5f;
    [SerializeField] private float m_attackSpeed = 350;
    private Rigidbody2D m_rigid;
    private float m_timer = 0;

    private void Awake()
    {
        m_rigid = this.GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        m_timer = 0;
    }

    private void Update()
    {
        m_timer += Time.deltaTime;
        if(m_timer >= m_duration)
        {
            this.gameObject.SetActive(false);
        }
        m_rigid.velocity = this.transform.right * m_attackSpeed * Time.fixedDeltaTime;
    }
}
