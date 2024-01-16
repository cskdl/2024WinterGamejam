using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//�÷��̾� �Ӹ��� ���� ����
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

    //�� & ��
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.tag == "Wall" && !m_playerManager.IsCrashed)
        {
            //����
            //Player Manager�� �̵� ���� �� �ӵ�...���� ��� �ǰ� ī��Ʈ�ٿ� ����(1�� ����?)�ϰ� �ڷ� �и��� �ϸ�......?
            m_playerManager.CountDown = 1;
            m_playerManager.IsCrashed = true;
            //collision.transform.GetComponent<Collider2D>().isTrigger = true;
        }
    }


    //��(���������� ����) & ������ & ����
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "DragonBall")
        {
            m_playerManager.CreateBodyParts();
            m_foodGenerator.RemoveFood(collision.transform.position);
            Destroy(collision.gameObject);
        }
        //���� ���
        else if(collision.gameObject.tag == "Body")
        {
            if (m_isInBody)
            {
                return;
            }
            Debug.Log("�ƾ�! ���� �¾Ѿ��");
            m_isInBody = true;
            m_collider.isTrigger = true;
        }
    }

    //�̰͵� ���� ���,,,
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
