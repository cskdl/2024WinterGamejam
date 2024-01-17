using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFire : MonoBehaviour
{
    public GameObject fireballPrefab;
    public float fireballSpeed = 10f;
    public float initialDelay = 3f;
    public float fireRate = 2f;

    private void Start()
    {
        StartCoroutine(FireRoutine());
    }

    IEnumerator FireRoutine()
    {
        yield return new WaitForSeconds(initialDelay);

        while (true)
        {
            FireAtPlayer();
            yield return new WaitForSeconds(1f / fireRate);
        }
    }

    void FireAtPlayer()
    {
        Vector3 direction = (FindObjectOfType<PlayerAttack>().transform.position - transform.position).normalized;
        GameObject fireball = Instantiate(fireballPrefab, transform.position, Quaternion.identity);

        Rigidbody2D rb = fireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * fireballSpeed;
        }
    }
}
