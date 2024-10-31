using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    // private camera object
    [SerializeField]
    private Camera sceneCamera;

    // last mouse position
    private Vector3 lastPosition;

    // layer that raycast affects
    [SerializeField]
    private LayerMask placementLayermask;

    // returns last mouse position using raycasting for a specific layer
    public Vector3 GetMousePos()
    {
        Vector3 mousePos = sceneCamera.ScreenToWorldPoint(Input.mousePosition);

        return mousePos;
    }
}