using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//플레이어 머리에 직접 삽입
public class PlayerColliisionCheck : MonoBehaviour
{
    [SerializeField] private PlayerManager m_playerManager;
    private FoodGenerator m_foodGenerator;
    private Collider2D m_collider;
    private Rigidbody2D m_playerRigid;
    private bool m_isInBody = false;

    private void Awake()
    {
        m_collider = GetComponent<Collider2D>();
        m_playerRigid = GetComponent<Rigidbody2D>();

        m_foodGenerator = FindObjectOfType<FoodGenerator>();
    }

    //벽 & 몸
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" && !m_playerManager.IsCrashed)
        {
            //무적
            //Player Manager의 이동 방향 및 속도...까진 없어도 되고 카운트다운 조정(1초 정도?)하고 뒤로 밀리게 하면......?
            m_playerManager.CountDown = 1;
            m_playerManager.IsCrashed = true;
            //collision.transform.GetComponent<Collider2D>().isTrigger = true;
        }
    }


    //벽(무적해제용 판정) & 여의주 & 몸통
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DragonBall")
        {
            m_playerManager.CreateBodyParts();
            m_foodGenerator.RemoveFood(collision.transform.position);
            Destroy(collision.gameObject);
        }
        //거의 폐기
        else if(collision.gameObject.tag == "Body")
        {
            if (m_isInBody)
            {
                return;
            }
            Debug.Log("아야! 몸에 맞앗어요");
            m_isInBody = true;
            m_collider.isTrigger = true;
        }
    }

    //이것도 거의 폐기,,,
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Body")
        {
            if (!m_isInBody)
            {
                return;
            }
            m_isInBody = false;
            m_collider.isTrigger = false;
        }
    }
}
