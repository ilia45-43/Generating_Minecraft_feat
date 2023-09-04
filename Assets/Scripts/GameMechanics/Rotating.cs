using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotating : MonoBehaviour
{
    float a = 0;
    public float amplitude = 0.5f;
    public float speed = 1000;
    private void FixedUpdate()
    {
        transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);
    }
}
