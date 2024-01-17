using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 머리에 직접 삽입
public class PlayerColliisionCheck : MonoBehaviour
{
    [SerializeField] private PlayerManager m_playerManager;
    private FoodGenerator m_foodGenerator;
    private IngameManager m_ingameManager;
    private Collider2D m_collider;
    private Rigidbody2D m_playerRigid;
    private bool m_isInBody = false;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        m_playerRigid = GetComponent<Rigidbody2D>();

        m_foodGenerator = FindObjectOfType<FoodGenerator>();
        m_ingameManager = FindObjectOfType<IngameManager>();
    }

    //벽
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" && !m_playerManager.IsCrashed && m_playerManager.CountDown <= 0)
        {
            m_playerManager.CountDown = 1;
            m_playerManager.IsCrashed = true;

            m_ingameManager.UpdateHP();
        }
    }


    //여의주
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DragonBall")
        {
            m_playerManager.CreateBodyParts();
            m_foodGenerator.RemoveFood(collision.gameObject);
            Destroy(collision.gameObject);
            m_ingameManager.UpdateScore();
        }
    }
}
