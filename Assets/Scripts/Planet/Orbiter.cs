using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orbiter : MonoBehaviour
{
    [SerializeField]
    public float angle = 0.0f;
    private float radius = 15.0f;
    private float speed = -0.001f;
    public Vector2 velocity;

    // Update is called once per frame
    void Update()
    {
        angle += speed;
        velocity = (new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius);
        velocity.x -= transform.position.x; velocity.y -= transform.position.y;
        transform.position = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * radius;
    }
}
