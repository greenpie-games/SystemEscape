using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerShip : SystemObject
{
    List<LineRenderer> lookAheadLines;
    [SerializeField]

    void LineBetweenPositions(List<Vector2> positions, bool flyByColor)
    {
        GameObject lineObject = new GameObject("Line");
        LineRenderer line = lineObject.AddComponent(typeof(LineRenderer)) as LineRenderer;
        line.positionCount = positions.Count;
        line.startWidth = 0.02f;
        line.endWidth = 0.02f;
        // Convert positions to Vector3s so we can use them to draw lines.
        Vector3[] vec3Positions = new Vector3[] { };
        Array.Resize(ref vec3Positions, positions.Count);
        for (int i = 0; i < positions.Count; i++)
            vec3Positions[i] = (Vector3)positions[i];
        line.SetPositions(vec3Positions);
        line.material = new Material(Shader.Find("Sprites/Default"));
        Color color = new Color(1f, 0f, 1f, 1f);
        if (flyByColor)
        {
            color = new Color(1f, 1f, 0f, 1f);
        }
        line.SetColors(color, color);
        lookAheadLines.Add(line);
    }

    override protected GameObject PrimaryGravityParent(int iFramePlus)
    {
        if (FindNearbyParent(iFramePlus) == null) return gravityParents[0];
        return FindNearbyParent(iFramePlus);
    }

    // Finds a nearby parent (if there is one)
    public GameObject FindNearbyParent(int framePlus)
    {
        foreach (GameObject parent in gravityParents)
            if (Vector2.Distance(parent.GetComponent<SystemObject>().PositionAtTime(framePlus), PositionAtTime(framePlus)) < 0.4f)
                return parent;
        return null;
    }

    // For each location in player's trajectory, returns true if it is a flyby.
    List<bool> FindAllPredictedFlybys()
    {
        List<bool> output = new List<bool> { };
        for (int i = 0; i < frames_lookahead; i++)
        {
            if (FindNearbyParent(i))
                output.Add(true);
            else
                output.Add(false);
        }
        return output;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        lookAheadLines = new List<LineRenderer> { };
    }

    // Normalize predicted future locations by parent position
    override public void HandlePostMoveDraws()
    {
        // Clear old draws.
        foreach (LineRenderer line in lookAheadLines)
            Destroy(line.gameObject);
        lookAheadLines.Clear();

        // Get flyBys *BEFORE* position normalization.
        List<bool> flyBys = FindAllPredictedFlybys();

        // Normalize drawn display positions to primary parent's position
        SystemObject parent = PrimaryGravityParent(0).GetComponent<SystemObject>();
        for (int i = 0; i < frames_lookahead; i++)
            projectedLocations[i] -= parent.NetDelta(i);

        // Find all flyBys and draw them in a different color than non-flyBys.
        bool flyByInProgress = flyBys[0];
        List<Vector2> flyBy = new List<Vector2> { };
        for (int i = 0; i < frames_lookahead; i++)
        {
            flyBy.Add(projectedLocations[i]);
            // Flyby status changed, time to start a new line.
            if (flyByInProgress != flyBys[i])
            {
                LineBetweenPositions(flyBy, flyByInProgress);
                flyBy = new List<Vector2> { };
                flyByInProgress = !flyByInProgress;
            }
        }
        LineBetweenPositions(flyBy, flyByInProgress);
    }

    override public void ComputeNewLocations(int gameSpeed)
    {
        if (Input.GetKey(KeyCode.W))
            velocity += (Vector2)(0.005f * gameSpeed * transform.up);
        if (Input.GetKey(KeyCode.S))
            velocity -= (Vector2)(0.005f * gameSpeed * transform.up);
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(Vector3.forward * 1.75f);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(Vector3.forward * -1.75f);
        base.ComputeNewLocations(gameSpeed);
    }
}
