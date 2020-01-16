﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SystemManager : MonoBehaviour
{
    // These should be in tree order (stars first, then planets, then moons, then player)
    [SerializeField]
    List<SystemObject> systemObjects;

    [SerializeField]
    PlayerShip player;

    [SerializeField]
    CameraController mainCameraController;

    // Start is called before the first frame update
    void Start()
    {
        systemObjects = GetComponentsInChildren<SystemObject>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        int gameSpeed = 4;
        if (player.FindNearbyParent(-1) != null)
        {
            mainCameraController.targetLocation = player.FindNearbyParent(-1).transform.position;
            mainCameraController.targetViewPortSize = Constants.PLANET_ZOOM;
            gameSpeed = 1;
        }
        else
        {
            mainCameraController.targetLocation = new Vector2(0f, 0f);
            mainCameraController.targetViewPortSize = Constants.SYSTEM_ZOOM;
        }
        foreach (SystemObject o in systemObjects)
            o.ComputeNewLocations();
        foreach (SystemObject o in systemObjects)
        {
            o.MoveToNextLocation(gameSpeed);
            o.CheckReferenceFrame(gameSpeed);
        }
    }
}
