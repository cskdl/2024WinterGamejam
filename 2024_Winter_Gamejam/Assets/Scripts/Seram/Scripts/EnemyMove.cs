using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    public float speed = 3f;
    public float rotationSpeed = 200f;

    float velX = 0f;

    private void Update()
    {
        velX = Input.GetAxisRaw("Horizontal");
    }
    
    private void FixedUpdate()
    {
        transform.Translate(Vector2.up * speed * Time.fixedDeltaTime, Space.Self);

        transform.Rotate(Vector3.forward * -velX * rotationSpeed * Time.fixedDeltaTime);
    }
}
    