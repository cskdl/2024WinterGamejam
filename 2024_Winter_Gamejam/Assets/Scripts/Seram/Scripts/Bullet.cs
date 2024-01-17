using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Bullet : MonoBehaviour
{
    void Update()
    {
        CheckIfOutsideCameraView();
    }

    void CheckIfOutsideCameraView()
    {
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (screenPoint.x < 0 || screenPoint.x > Screen.width || screenPoint.y < 0 || screenPoint.y > Screen.height)
        {
            Destroy(gameObject);
        }
    }
}
