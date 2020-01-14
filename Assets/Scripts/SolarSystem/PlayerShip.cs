using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PlayerShip : SystemObject
{
    List<LineRenderer> lookAheadLines;
    [SerializeField]

    void LineBetweenPositions(List<Vector2> positions, Color color)
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
        line.SetColors(color, color);
        lookAheadLines.Add(line);
    }

    override protected GameObject[] PrimaryGravityParent(int iFramePlus)
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

    // Finds any flybys in the player's predicted trajectory.
    List<List<Vector2>> FindAllPredictedFlybys()
    {
        bool flyByInProgress = false;
        List<List<Vector2>> output = new List<List<Vector2>> { };
        for (int i = 0; i < frames_lookahead; i++)
        {
            if (FindNearbyParent(i))
            {
                if (!flyByInProgress)
                    output.Add(new List<Vector2> { });
                output.Last().Add(PositionAtTime(i));
                flyByInProgress = true;
            }
            else
            {
                flyByInProgress = false;
            }
        }
        return output;
    }

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();
        lookAheadLines = new List<LineRenderer> { };
    }

    override public void ComputeNewLocations()
    {
        if (Input.GetKey(KeyCode.W))
            velocity.y += 0.01f;
        if (Input.GetKey(KeyCode.S))
            velocity.y -= 0.01f;
        if (Input.GetKey(KeyCode.A))
            velocity.x -= 0.01f;
        if (Input.GetKey(KeyCode.D))
            velocity.x += 0.01f;
        base.ComputeNewLocations();
        foreach (LineRenderer line in lookAheadLines)
            Destroy(line.gameObject);
        lookAheadLines.Clear();
        LineBetweenPositions(projectedLocations, new Color(.75f, .75f, 2f, .5f));
        foreach (List<Vector2> flyby in FindAllPredictedFlybys())
            LineBetweenPositions(flyby, new Color(1f, 1f, 0f, 1f));
    }
}
