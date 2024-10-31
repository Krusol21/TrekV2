using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    [SerializeField]
    private List<GameObject> trail = new List<GameObject>();

    private int num = -1; // Initialize with -1 to avoid selection until a key is pressed.
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Update()
    {
        Vector3 mousePosition = inputManager.GetMousePos();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);

        // Check for key presses to set which trail piece to follow the mouse
        if (Input.GetKeyDown(KeyCode.Alpha1)) num = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2)) num = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3)) num = 2;

        // Ensure num is valid and a trail piece is selected
        if (num >= 0)
        {
            // Make the selected trail piece follow the mouse position on the grid
            trail[num].transform.position = grid.CellToWorld(gridPosition);

            // Rotate the trail piece when the 'R' key is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Rotate the sprite's pivot by 90 degrees
                trail[num].transform.Rotate(0, 0, 90);
            }

            // Place the trail piece when the left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                trail[num].transform.position = grid.CellToWorld(gridPosition);
                num = -1; // Deselect the piece after placing
            }
        }

        // Update last detected position for further logic if needed
        if (lastDetectedPosition != gridPosition)
        {
            lastDetectedPosition = gridPosition;
        }
    }
}