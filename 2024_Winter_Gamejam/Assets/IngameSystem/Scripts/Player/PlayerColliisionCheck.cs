using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 머리에 직접 삽입
public class PlayerColliisionCheck : MonoBehaviour
{
    [SerializeField] private PlayerManager m_playerManager;
    [SerializeField] private AudioClip m_eatSound;
    [SerializeField] private AudioClip m_bumpSound;
    private FoodGenerator m_foodGenerator;
    private TutorialFood m_tutorialFoodGenerator;
    private IngameManager m_ingameManager;
    private Collider2D m_collider;
    private Rigidbody2D m_playerRigid;
    private bool m_isInBody = false;
    private AudioSource m_audioSource;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        m_playerRigid = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();

        m_tutorialFoodGenerator = FindObjectOfType<TutorialFood>();
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

            if(m_ingameManager != null) m_ingameManager.UpdateHP();
            PlaySound(m_bumpSound);
        }
        if (collision.gameObject.CompareTag("Enemy"))
        {
            m_playerManager.CountDown = 1;
            m_playerManager.IsCrashed = true;
            if (m_ingameManager != null) m_ingameManager.UpdateHP();
        }
    }


    //여의주
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DragonBall")
        {
            m_playerManager.CreateBodyParts();
            if(m_foodGenerator != null) m_foodGenerator.RemoveFood(collision.gameObject);
            if (m_tutorialFoodGenerator != null) m_tutorialFoodGenerator.RemoveFood(collision.gameObject);
            Destroy(collision.gameObject);
            if (m_ingameManager != null) m_ingameManager.UpdateScore();
            PlaySound(m_eatSound);
        }
        else if (collision.gameObject.CompareTag("bullet"))
        {
            //if (m_ingameManager != null) m_ingameManager.UpdateHP();
        }
    }

    public void PlaySound(AudioClip pClip)
    {
        m_audioSource.Stop();
        m_audioSource.time = 0;
        m_audioSource.clip = pClip;
        m_audioSource.Play();
    }
}
