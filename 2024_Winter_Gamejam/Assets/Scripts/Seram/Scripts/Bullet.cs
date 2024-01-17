using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{
    public float destroyDelay = 3f; // 파괴될 때까지의 대기 시간

    void Start()
    {
        StartCoroutine(DestroyAfterDelay());
    }

    IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        Destroy(gameObject);
    }
}
