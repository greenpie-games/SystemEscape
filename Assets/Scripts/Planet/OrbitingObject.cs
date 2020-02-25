using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitingObject : MonoBehaviour
{
    [SerializeField]
    public float angle = 0.0f;
    private float radius = 6.0f;
    private float speed = -0.001f;

    // Update is called once per frame
    void Update()
    {
        angle += speed;
        transform.position = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
    }
}
