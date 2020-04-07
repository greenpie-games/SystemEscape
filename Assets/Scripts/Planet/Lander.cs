using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lander : MonoBehaviour
{

    [SerializeField]
    Text instructionalText;

    [SerializeField]
    ParticleSystem exhaust;

    [SerializeField]
    Image fuelIndicator;

    const float crashTolerance = 0.01f;

    const float maxFuel = 250f;
    private float fuel = 250f;
    float planetRadius = 10.2f;

    enum state
    {
        attachedToOrbiter,
        playerControlled,
        landed,
        crashed
    };

    private state currentState;
    Vector2 velocity;
    public float absVelocity;

    void PhysUpdate()
    {
        // Gravitational effects
        velocity.x -= transform.position.normalized.x * 0.00006f;
        velocity.y -= transform.position.normalized.y * 0.00006f;
        transform.position = (Vector2)transform.position + velocity;
        absVelocity = Mathf.Sqrt(velocity.x * velocity.x + velocity.y * velocity.y);
        if (Mathf.Sqrt(transform.position.x * transform.position.x + transform.position.y * transform.position.y) < planetRadius)
        {
            if (absVelocity > crashTolerance)
            {
                currentState = state.crashed;
            }
            else
            {
                currentState = state.landed;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        var exhaustEmission = exhaust.emission;
        if (currentState == state.attachedToOrbiter)
        {
            instructionalText.text = "Press space bar to deorbit";
            if (Input.GetKey(KeyCode.Space))
            {
                currentState = state.playerControlled;
                velocity = transform.parent.gameObject.GetComponent<Orbiter>().velocity * .95f;
                transform.SetParent(null, true);
            }
        }
        else if (currentState == state.playerControlled)
        {
            instructionalText.text = "WAD to move...";
            if (Input.GetKey(KeyCode.W) && fuel > 0)
            {
                velocity += (Vector2)(0.0004f * transform.up);
                exhaustEmission.enabled = true;
                fuel -= 1f;
                fuelIndicator.fillAmount = fuel / maxFuel;
            }
            else
            {
                exhaustEmission.enabled = false;
            }
            if (Input.GetKey(KeyCode.A))
                transform.Rotate(Vector3.forward * 0.75f);
            if (Input.GetKey(KeyCode.D))
                transform.Rotate(Vector3.forward * -0.75f);
            PhysUpdate();
        }
        else if (currentState == state.landed)
        {
            instructionalText.text = "Successful landing! Press Space Bar to exit.";
            exhaustEmission.enabled = false;
        }
        else if (currentState == state.crashed)
        {
            instructionalText.text = "Crash! Game over.";
            exhaustEmission.enabled = false;
        }
    }
}
