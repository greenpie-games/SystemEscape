using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCam : MonoBehaviour
{
    [SerializeField]
    GameObject toCenter;

    float desiredCameraSize;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(toCenter.transform.position.x, toCenter.transform.position.y, -10f);
        float angle = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        if (Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.y * transform.position.y) < 11.5f)
        {
            desiredCameraSize = 2.5f;
        }
        else
        {
            desiredCameraSize = 7.5f;
        }
        if (desiredCameraSize < GetComponent<Camera>().orthographicSize)
        {
            GetComponent<Camera>().orthographicSize -= .1f;
        }
        else if (desiredCameraSize > GetComponent<Camera>().orthographicSize + .1f)
        {
            GetComponent<Camera>().orthographicSize += .1f;
        }
    }
}
