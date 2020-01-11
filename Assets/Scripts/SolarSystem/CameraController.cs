using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Vector2 targetLocation;
    public float targetViewPortSize;

    // Start is called before the first frame update
    void Start()
    {
        targetLocation = new Vector2(0f, 0f);
        targetViewPortSize = 5f;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Vector2.MoveTowards(transform.position, targetLocation, 0.2f);
        pos.z = -10;
        transform.position = pos;
        Camera camera = GetComponent<Camera>();
        if (camera.orthographicSize > targetViewPortSize) camera.orthographicSize -= 0.1f;
        if (camera.orthographicSize < targetViewPortSize) camera.orthographicSize += 0.1f;
    }
}
