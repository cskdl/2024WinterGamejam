using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTail : MonoBehaviour
{
    public Transform EnemyTailGfx;
    public float circleDiameter;
    public float tailSpeed; // ������ �̵� �ӵ�

    private List<Transform> enemyTail = new List<Transform>();
    private List<Vector2> positions = new List<Vector2>();

    private void Start()
    {
        positions.Add(EnemyTailGfx.position);
    }

    private void Update()
    {
        float distance = ((Vector2)EnemyTailGfx.position - positions[0]).magnitude;

        if (distance >= circleDiameter)
        {
            Vector2 direction = ((Vector2)EnemyTailGfx.position - positions[0]).normalized;

            positions.Insert(0, positions[0] + direction * circleDiameter);
            positions.RemoveAt(positions.Count - 1);

            distance -= circleDiameter;
        }

        for (int i = 0; i < enemyTail.Count; i++)
        {
            enemyTail[i].position = Vector2.Lerp(positions[i + 1], positions[i], distance / circleDiameter);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("food"))
        {
            Destroy(other.gameObject);
            AddTail();
        }
    }

    public void AddTail()
    {
        Transform tail = Instantiate(EnemyTailGfx, positions[positions.Count - 1], Quaternion.identity, transform);
        enemyTail.Add(tail);
        positions.Add(tail.position);
    }

    public void MoveTail(Vector2 headPosition)
    {
        for (int i = 0; i < enemyTail.Count; i++)
        {
            enemyTail[i].position = Vector2.Lerp(enemyTail[i].position, headPosition, Time.deltaTime * tailSpeed);
        }
    }
}