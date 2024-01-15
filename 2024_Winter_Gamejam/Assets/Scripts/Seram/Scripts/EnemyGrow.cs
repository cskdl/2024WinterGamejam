using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGrow : MonoBehaviour
{
    public EnemyTail enemyTail;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("food"))
        {
            Destroy(other.gameObject,0.02f);
            enemyTail.AddTail();
        }
    }
}
    