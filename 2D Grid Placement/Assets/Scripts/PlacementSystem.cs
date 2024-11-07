using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [SerializeField]
    private GameObject obstaclePrefab;

    private List<Vector3Int> obstaclePositions = new List<Vector3Int>();
    private int num = -1; // Initialize with -1 to avoid selection until a key is pressed.
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        SetObstaclesForScene();
    }

    private void SetObstaclesForScene()
    {
        string sceneName = SceneManager.GetActiveScene().name;

        // Clear previous obstacles
        obstaclePositions.Clear();

        // Set obstacle positions based on the scene name
        if (sceneName == "Level1")
        {
            obstaclePositions = new List<Vector3Int>
            {
                //TODO set obstacle tiles
            };
        }
        else if (sceneName == "Level2")
        {
            obstaclePositions = new List<Vector3Int>
            {
                //TODO set obstacle tiles
            };
        }

        // send obstacle prefab to obstacle tiles
        foreach (Vector3Int pos in obstaclePositions)
        {
            Instantiate(obstaclePrefab, grid.CellToWorld(pos), Quaternion.identity);
        }
    }

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
                if (!obstaclePositions.Contains(gridPosition))
                {
                    // Place the trail piece if the cell is not an obstacle
                    trail[num].transform.position = grid.CellToWorld(gridPosition);
                    num = -1; // Deselect the piece after placing
                }
                else
                {
                    Debug.Log("Cannot place trail here - it's an obstacle.");
                }

            }
        }

        if (lastDetectedPosition != gridPosition)
        {
            lastDetectedPosition = gridPosition;
        }
    }
}