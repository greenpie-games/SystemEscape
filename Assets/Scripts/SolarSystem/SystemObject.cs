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
    protected GameObject[] gravityParents;

    protected Vector2 velocity;

    protected List<Vector2> projectedLocations;
    List<Vector2> projectedVelocities;

    static Vector2 frameScale = new Vector2(0.0025f, 0.0025f);

    // Start is called before the first frame update
    virtual protected void Start()
    {
        velocity = initialVelocity;
        projectedLocations = new List<Vector2>();
        projectedVelocities = new List<Vector2>();
    }

    virtual public void ComputeNewLocations(int gameSpeed)
    {
        projectedLocations.Clear();
        projectedVelocities.Clear();
        for (int i = 0; i < frames_lookahead; i++)
            AddProjectedLocation(i);
    }

    public void MoveToNextLocation(int gameSpeed)
    {
        transform.position = projectedLocations[gameSpeed - 1];
        velocity = projectedVelocities[gameSpeed - 1];
    }

    virtual protected GameObject PrimaryGravityParent(int iFramePlus)
    {
        if (gravityParents.Count() == 0) return null;
        return gravityParents[0];
    }

    public Vector2 PositionAtTime(int framePlus)
    {
        Vector2 myPosition = transform.position;
        if (framePlus >= 0)
        {
            if (1 + framePlus > projectedLocations.Count)
            {
                Debug.LogWarning("Solar system objects out of tree order");
                AddProjectedLocation(framePlus);
            }
            myPosition = projectedLocations[framePlus];
        }
        return myPosition;
    }

    virtual public void HandlePostMoveDraws()
    {

    }

    public Vector2 NetDelta(int framePlus)
    {
        return projectedLocations[framePlus] - projectedLocations[0];
    }

    Vector2 VelocityVectorDelta(GameObject parent, int framePlus)
    {
        SystemObject parentSystemObject = parent.GetComponent<SystemObject>();
        float pull = parentSystemObject.gravityPull;
        Vector2 parentPosition = parentSystemObject.PositionAtTime(framePlus);
        Vector2 myPosition = PositionAtTime(framePlus);
        float invSquare = 1.0f / (Vector2.Distance(parentPosition, myPosition) * Vector2.Distance(parentPosition, myPosition));
        Vector2 normalizedDirection = (parentPosition - myPosition).normalized;
        return new Vector2(normalizedDirection.x * pull * invSquare, normalizedDirection.y * pull * invSquare);
    }

    protected void AddProjectedLocation(int framePlus)
    {
        if (framePlus == 0) {
            projectedVelocities.Add(velocity);
            projectedLocations.Add((Vector2)transform.position);
        }
        else
        {
            projectedVelocities.Add(projectedVelocities[framePlus - 1]);
            projectedLocations.Add(projectedLocations[framePlus - 1]);
        }
        GameObject parent = PrimaryGravityParent(framePlus - 1);
        while (parent != null)
        {
            projectedVelocities[framePlus] += VelocityVectorDelta(parent, framePlus - 1);
            parent = parent.GetComponent<SystemObject>().PrimaryGravityParent(framePlus - 1);
        }
        projectedLocations[framePlus] += Vector2.Scale(projectedVelocities.Last(), frameScale);
    }
}
