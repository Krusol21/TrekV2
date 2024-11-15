using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ObstacleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject obstaclePrefab;

    [SerializeField]
    private GameObject startPrefab;

    [SerializeField]
    private GameObject endPrefab;

    [SerializeField]
    private PlacementSystem placementSystem;

    public List<Vector3Int> obstaclePositions = new List<Vector3Int>();

    private void SetObstaclesForScene()
    {
        // Define the play grid dimensions
        Vector3Int gridStart = new Vector3Int(-7, -1, 0); // Bottom-left corner of the 6x14 play grid
        int gridWidth = 14;
        int gridHeight = 6;

        // Define the exclusion box
        Vector3Int exclusionStart = new Vector3Int(-3, 1, 0); // Bottom-left corner of the 3x6 box to exclude
        int exclusionWidth = 6;
        int exclusionHeight = 3;

        // Define the start and end positions
        Vector3Int startPosition = new Vector3Int(-3, 2, 0);
        Vector3Int endPosition = new Vector3Int(3, 2, 0);

        // Clear any previous obstacles
        obstaclePositions.Clear();

        // Loop through each tile in the 6x14 grid
        for (int x = gridStart.x; x < gridStart.x + gridWidth; x++)
        {
            for (int y = gridStart.y; y < gridStart.y + gridHeight; y++)
            {
                Vector3Int tilePosition = new Vector3Int(x, y, 0);

                // Check if the tile is outside the 3x6 exclusion box
                bool inExclusionBox = x >= exclusionStart.x && x < exclusionStart.x + exclusionWidth &&
                                      y >= exclusionStart.y && y < exclusionStart.y + exclusionHeight;

                // Skip if it's inside the exclusion box, start position, or end position
                if (!inExclusionBox && tilePosition != startPosition && tilePosition != endPosition)
                {
                    obstaclePositions.Add(tilePosition);
                    Instantiate(obstaclePrefab, placementSystem.grid.CellToWorld(tilePosition), Quaternion.identity);
                }
            }
        }

        // Additional specific obstacles for Level 1
        string sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "Level1")
        {
            // Specific additional obstacles for Level 1, excluding start and end positions
            Vector3Int[] extraObstacles = new Vector3Int[]
            {
                new Vector3Int(-3, 1, 0),
                new Vector3Int(-2, 1, 0),
                new Vector3Int(-1, 1, 0),
                new Vector3Int(-1, 2, 0),
                new Vector3Int(1, 3, 0),
                new Vector3Int(2, 3, 0)
            };

            foreach (var pos in extraObstacles)
            {
                if (pos != startPosition && pos != endPosition)
                {
                    obstaclePositions.Add(pos);
                    Instantiate(obstaclePrefab, placementSystem.grid.CellToWorld(pos), Quaternion.identity);
                }
            }

            // Place the start and endpoint prefabs
            Instantiate(startPrefab, placementSystem.grid.CellToWorld(startPosition), Quaternion.identity);
            Instantiate(endPrefab, placementSystem.grid.CellToWorld(endPosition), Quaternion.identity);
        }
    }

    private void Start()
    {
        SetObstaclesForScene();
    }
}
