using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;

public class PlacementSystem : MonoBehaviour
{
    public AudioSource src;
    public AudioClip placeTrail, rotateTrail;

    [SerializeField]
    private GameObject lTrail;
    private GameObject trail1;

    [SerializeField]
    private GameObject shortTrail;
    private GameObject trail2;

    [SerializeField]
    private GameObject longTrail;
    private GameObject trail3;

    //[SerializeField]
    //private GameObject longTrail2;
    //private GameObject trail4;

    //[SerializeField] private List<GameObject> Trails;
    //[SerializeField] private List<Vector2> TrailPositions;

    [SerializeField] private Tilemap woods;

    [SerializeField]
    private InputManager inputManager;

    [SerializeField]
    private AnswerManager answerManager;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    private List<GameObject> trail = new List<GameObject>();

    [SerializeField]
    private GameObject obstaclePrefab;

    private List<Vector3Int> obstaclePositions = new List<Vector3Int>();
    private int num = -1; // Initialize with -1 to avoid selection until a key is pressed.
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    private void Start()
    {
        //for(int i = 0; i < Trails.count; i++)
        //{
        //    GameObject newTrail = Instantiate(Trails[i], TrailPositions[i], Quaternion.identity);
        //    trail.Add(newTrail);
        //}

        Instantiate(lTrail, new Vector3(-5, -2.8f, 0), Quaternion.identity);
        Instantiate(shortTrail, new Vector3(-1, -3.8f, 0), Quaternion.identity);
        Instantiate(longTrail, new Vector3(3.5f, -3, 0), Quaternion.identity);
        trail1 = GameObject.FindGameObjectWithTag("LTrail");
        trail2 = GameObject.FindGameObjectWithTag("ShortTrail");
        trail3 = GameObject.FindGameObjectWithTag("LongTrail");

        trail.Add(trail1);
        trail.Add(trail2);
        trail.Add(trail3);

        answerManager.SetAnswerList(trail);

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
            //Debug.Log(trail[num].transform.position);
            // Make the selected trail piece follow the mouse position on the grid
            trail[num].transform.position = grid.CellToWorld(gridPosition);

            // Rotate the trail piece when the 'R' key is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                // Rotate the sprite's pivot by 90 degrees
                trail[num].transform.Rotate(0, 0, 90);
                //Debug.Log(trail[num].transform.rotation.eulerAngles.z);
                src.clip = rotateTrail;
                src.Play();
            }

            // Place the trail piece when the left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {

                Debug.Log(gridPosition);
                if (!obstaclePositions.Contains(gridPosition))
                {
                    // Place the trail piece if the cell is not an obstacle
                    trail[num].transform.position = grid.CellToWorld(gridPosition);
                    src.clip = placeTrail;
                    src.Play();

                    // Set the trail piece's current position and rotation
                    trail[num].GetComponent<Trail>().CurrentPos = gridPosition;
                    trail[num].GetComponent<Trail>().Rotation = trail[num].transform.rotation.eulerAngles.z;

                    // Check if the trail piece is placed correctly
                    trail[num].GetComponent<Trail>().CheckAnswer();

                    if (answerManager.CheckAnswers(trail))
                    {
                        Debug.Log("All trails are placed correctly!");
                    }

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