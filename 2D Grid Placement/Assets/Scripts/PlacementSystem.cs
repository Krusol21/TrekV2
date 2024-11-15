using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using static UnityEngine.UI.Image;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using TMPro;

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
    private ObstacleManager obstacleManager;

    [SerializeField]
    public Grid grid;

    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;

    private List<GameObject> trail = new List<GameObject>();

    private int num = -1; // Initialize with -1 to avoid selection until a key is pressed.
    private Vector3Int lastDetectedPosition = Vector3Int.zero;

    [SerializeField] public GameObject congratsText;

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

        congratsText.gameObject.SetActive(false);
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
            trail[num].transform.position = grid.CellToWorld(gridPosition);

            // Rotate the trail piece when 'R' is pressed
            if (Input.GetKeyDown(KeyCode.R))
            {
                trail[num].transform.Rotate(0, 0, 90);
                src.clip = rotateTrail;
                src.Play();
            }

            // Place the trail piece when the left mouse button is clicked
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log(gridPosition);
                if (!obstacleManager.obstaclePositions.Contains(gridPosition))
                {
                    // Place the trail piece if the cell is not an obstacle
                    trail[num].transform.position = grid.CellToWorld(gridPosition);
                    src.clip = placeTrail;
                    src.Play();

                    trail[num].GetComponent<Trail>().CurrentPos = gridPosition;
                    trail[num].GetComponent<Trail>().Rotation = trail[num].transform.rotation.eulerAngles.z;

                    trail[num].GetComponent<Trail>().CheckAnswer();

                    if (answerManager.CheckAnswers(trail))
                    {
                        Debug.Log("All trails are placed correctly!");
                        congratsText.gameObject.SetActive(true);
                    }

                    num = -1; // Deselect the piece after placing
                }
                else
                {
                    Debug.Log("Cannot place trail here - it's an obstacle.");
                }
            }
        }
    }

}