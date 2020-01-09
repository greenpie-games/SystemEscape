using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SystemObject : MonoBehaviour
{
    static protected int frames_lookahead = 500;

    [SerializeField]
    protected float gravityPull;

    [SerializeField]
    Vector2 initialVelocity;

    // Parents are a tree, meaning there CANNOT BE CIRCULAR RELATIONSHIPS.
    [SerializeField]
    GameObject[] gravityParents;

    protected Vector2 velocity;

    protected List<Vector2> projectedLocations;
    Vector2 lastProjectedVelocity;

    static Vector2 frameScale = new Vector2(0.01f, 0.01f);

    // Start is called before the first frame update
    void Start()
    {
        velocity = initialVelocity;
        projectedLocations = new List<Vector2>();
    }

    virtual public void PerFrameActions()
    {
        projectedLocations.Clear();
        for (int i = 0; i < frames_lookahead; i++)
        {
            AddProjectedLocation(i);
        }
        foreach (GameObject parent in gravityParents)
        {
            velocity += VelocityVectorDelta(parent, 0);
        }
        transform.position = (Vector2)transform.position + Vector2.Scale(velocity, frameScale);
        lastProjectedVelocity = velocity;
    }

    Vector2 VelocityVectorDelta(GameObject parent, int framePlus)
    {
        SystemObject parentSystemObject = parent.GetComponent<SystemObject>();
        float pull = parentSystemObject.gravityPull;
        Vector2 parentPosition = parent.transform.position;
        if (framePlus > 0)
        {
            if (framePlus > parentSystemObject.projectedLocations.Count)
                parentSystemObject.AddProjectedLocation(framePlus);
            parentPosition = parentSystemObject.projectedLocations[framePlus];
        }
        Vector2 myPosition = transform.position;
        if (framePlus > 0)
        {
            myPosition = projectedLocations[framePlus - 1];
        }
        float invSquare = 1.0f / (Vector2.Distance(parentPosition, myPosition) * Vector2.Distance(parentPosition, myPosition));
        Vector2 normalizedDirection = (parentPosition - myPosition).normalized;
        return new Vector2(normalizedDirection.x * pull * invSquare, normalizedDirection.y * pull * invSquare);
    }

    protected void AddProjectedLocation(int framePlus)
    {
        foreach (GameObject parent in gravityParents)
        {
            lastProjectedVelocity += VelocityVectorDelta(parent, framePlus);
        }
        if (framePlus == 0)
            projectedLocations.Add((Vector2)transform.position + Vector2.Scale(lastProjectedVelocity, frameScale));
        else
            projectedLocations.Add(projectedLocations[framePlus - 1] + Vector2.Scale(lastProjectedVelocity, frameScale));
    }
}
