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
        targetViewPortSize = Constants.SYSTEM_ZOOM;
    }

    float GetMoveSpeed()
    {
        float currSize = camera.orthographicSize;
        if (currSize > Constants.SYSTEM_ZOOM - 1)
        {
            return 0.2f - (currSize - Constants.SYSTEM_ZOOM) * .1f;
        }
        if (currSize < Constants.PLANET_ZOOM + 1)
        {
            return 0.2f - (Constants.PLANET_ZOOM - currSize) * .1f;
        }
        return 0.2f;
    }

    float GetZoomSpeed()
    {
        if (currSize > Constants.SYSTEM_ZOOM - 1)
        {
            return 0.1f - (currSize - Constants.SYSTEM_ZOOM) * .05f;
        }
        if (currSize < Constants.PLANET_ZOOM + 1)
        {
            return 0.1f - (Constants.PLANET_ZOOM - currSize) * .05f;
        }
        return .1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (camera.orthographicSize > targetViewPortSize) camera.orthographicSize -= GetZoomSpeed();
        if (camera.orthographicSize < targetViewPortSize) camera.orthographicSize += GetZoomSpeed();
        Vector3 pos = Vector2.MoveTowards(transform.position, targetLocation, GetMoveSpeed());
        pos.z = -10;
        transform.position = pos;
        Camera camera = GetComponent<Camera>();
    }
}
