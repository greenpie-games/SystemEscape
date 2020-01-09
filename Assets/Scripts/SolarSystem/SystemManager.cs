﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SystemManager : MonoBehaviour
{
    [SerializeField]
    List<SystemObject> systemObjects;

    // Start is called before the first frame update
    void Start()
    {
        systemObjects = GetComponentsInChildren<SystemObject>().ToList();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (SystemObject o in systemObjects)
        {
            o.PerFrameActions();
        }
    }
}
