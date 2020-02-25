using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetCam : MonoBehaviour
{
    [SerializeField]
    GameObject toCenter;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(toCenter.transform.position.x, toCenter.transform.position.y, -10f);
        float angle = Mathf.Atan2(transform.position.y, transform.position.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    }
}
