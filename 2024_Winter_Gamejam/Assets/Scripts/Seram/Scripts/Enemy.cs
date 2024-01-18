using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    public float speed;
    public Rigidbody2D target;

    Rigidbody2D rigid;
    SpriteRenderer spriter;

    float m_timer = 3;
    float m_delay = 0;
    private float maxHp = 10;
    private float currentHp;

    public Slider healthSlider;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        spriter = GetComponent<SpriteRenderer>();
        currentHp = maxHp;
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

    void GetDamage(float damageAmount)
    {
        currentHp -= damageAmount;
        UpdateHealthSlider();
    }

    bool IsAlive()
    {
        return currentHp > 0;
    }

    void UpdateHealthSlider()
    {
        if (healthSlider != null)
        {
            healthSlider.value = currentHp / maxHp;
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("BorderBullet"))
        {
            int damageAmount = 1;
            GetDamage(damageAmount);
        }
    }

    void Update()
    {
        if (!IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
