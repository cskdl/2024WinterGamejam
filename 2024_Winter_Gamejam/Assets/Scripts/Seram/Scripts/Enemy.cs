using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;



    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    float m_timer = 3;
    float m_delay = 0;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        target = GameObject.FindObjectOfType<PlayerAttack>().GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if (m_timer > 0)
        {
            m_timer -= Time.fixedDeltaTime;
        }
        else
        {
            if (m_delay <= 0)
            {
                MoveTowardsTarget();
                LookAtTarget();
            }
            else
            {
                m_delay -= Time.fixedDeltaTime;
            }
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 temp = new Vector2(target.position.x, target.position.y);
        Vector2 dirVec = temp - rigid.position;
        Vector2 nextVec = dirVec.normalized;
        rigid.velocity = this.transform.right * speed * Time.fixedDeltaTime;
    }

    void LookAtTarget()
    {
        Vector3 temp = target.position;
        Vector3 targetDir = temp - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }

    void MoveBump()
    {
        rigid.velocity = this.transform.right * -speed * Time.fixedDeltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            m_delay = 1;
            MoveBump();
        }
    }
}
