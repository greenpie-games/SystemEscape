using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lander : MonoBehaviour
{

    [SerializeField]
    Text instructionalText;

    enum state
    {
        attachedToOrbiter,
        playerControlled,
        landing
    };

    private state currentState;
    Vector2 velocity;

    void PhysUpdate()
    {
        // Gravitational effects
        velocity.x -= transform.position.normalized.x * 0.000006f;
        velocity.y -= transform.position.normalized.y * 0.000006f;
        transform.position = (Vector2)transform.position + velocity;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState == state.attachedToOrbiter)
        {
            instructionalText.text = "Press space bar to deorbit";
            if (Input.GetKey(KeyCode.Space))
            {
                currentState = state.playerControlled;
                velocity = transform.parent.gameObject.GetComponent<Orbiter>().velocity * .9f;
                transform.SetParent(null, true);
            }
        }
        else if (currentState == state.playerControlled)
        {
            instructionalText.text = "WAD to move...";
            if (Input.GetKey(KeyCode.W))
                velocity += (Vector2)(0.000025f * transform.up);
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.forward * 0.5f);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.forward * -0.5f);
            PhysUpdate();
        }
    }
}
