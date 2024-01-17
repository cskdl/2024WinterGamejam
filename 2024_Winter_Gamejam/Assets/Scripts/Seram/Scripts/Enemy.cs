using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Enemy : MonoBehaviour
{
    public float speed;
    //public Rigidbody2D target;



    bool isLive;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    float m_timer = 3;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        this.transform.position = Camera.main.ViewportToWorldPoint(new Vector3(0, 0));
        Vector3 temp = transform.position;
        temp.z = 10;
        this.transform.position = temp;
    }

    void FixedUpdate()
    {
        if (m_timer > 0)
        {
            m_timer -= Time.fixedDeltaTime;
        }
        else
        {
            MoveTowardsTarget();
            LookAtTarget();
        }
    }

    void MoveTowardsTarget()
    {
        Vector2 temp = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);
        Vector2 dirVec = temp - rigid.position;
        Vector2 nextVec = dirVec.normalized;
        //rigid.MovePosition((rigid.position + nextVec) * speed * Time.fixedDeltaTime);
        //rigid.velocity = Vector3.zero;
        rigid.velocity = this.transform.right * speed * Time.fixedDeltaTime;
    }

    void LookAtTarget()
    {
        Vector3 temp = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y);
        Vector3 targetDir = temp - transform.position;
        float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }
}
