using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerShip : SystemObject
{
    static int frames_lookahead = 500;

    LineRenderer lookAheadLines;

    void LineBetweenPositions(List<Vector2> positions)
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
        lookAheadLines = line;
    }

    override protected void PerFrameActions()
    {
        for (int i = 0; i < frames_lookahead;  i++)
        {
            AddProjectedLocation(i);
        }
        if (lookAheadLines != null)
            Destroy(lookAheadLines.gameObject);
        LineBetweenPositions(projectedLocations);

        if (Input.GetKey(KeyCode.W))
            velocity.y += 0.01f;
        if (Input.GetKey(KeyCode.S))
            velocity.y -= 0.01f;
        if (Input.GetKey(KeyCode.A))
            velocity.x -= 0.01f;
        if (Input.GetKey(KeyCode.D))
            velocity.x += 0.01f;
    }
}
