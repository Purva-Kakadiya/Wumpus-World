using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EditMapOptionsUI : MonoBehaviour {

    public static EditMapOptionsUI Instance { get; private set; }

    public event EventHandler OnRotateButtonPressed;

    [SerializeField] private Button spawnSquareButton;
    [SerializeField] private Button spawnTriangleButton;
    [SerializeField] private Button spawnHexagonButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform confirmPanel;
    [SerializeField] private Cell square;
    [SerializeField] private Cell triangle;
    [SerializeField] private Cell hexagon;
    [SerializeField] private Transform spawnLevel;
    [SerializeField] private LayerMask gridSpawnLayer;

    private int spawnLayerValueInInt;
    private Cell lastSpawnedCell;
    private Cell callerCell = null;
    private int spawnObjectNumber = 0;


    private void Awake() {

        if(Instance != null) {
            Debug.Log("More than one Instance for PerceptUI!");
        }
        Instance = this;

        spawnLayerValueInInt = Mathf.RoundToInt(Mathf.Log(gridSpawnLayer.value, 2));

        spawnSquareButton.onClick.AddListener(() => {
            confirmPanel.gameObject.SetActive(true);
            SpawnTheCell(square);
        });

        spawnTriangleButton.onClick.AddListener(() => {
            confirmPanel.gameObject.SetActive(true);
            SpawnTheCell(triangle);
        });

        spawnHexagonButton.onClick.AddListener(() => {
            confirmPanel.gameObject.SetActive(true);
            SpawnTheCell(hexagon);
        });

        confirmButton.onClick.AddListener(() => {
            OnConfirmPressed();
        });

        cancelButton.onClick.AddListener(() => {
            if (callerCell == lastSpawnedCell) {
                DestroyCell();
            } else {
                callerCell.CellMovementCanceled();
                callerCell = null;
            }
            confirmPanel.gameObject.SetActive(false);
        });

        confirmPanel.gameObject.SetActive(false);
    }

    private void OnConfirmPressed() {

        if (lastSpawnedCell == callerCell) {
            Transform updatedTransform = lastSpawnedCell.GetTransform(lastSpawnedCell);
            lastSpawnedCell.CellMovementConfirmed();

            lastSpawnedCell = null;

        } else {
            callerCell.CellMovementConfirmed();
            callerCell = null;
        }
        confirmPanel.gameObject.SetActive(false);
    }

    private void SpawnTheCell(Cell cell) {

        Vector2 spawnLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cell = Instantiate(cell, spawnLocation, Quaternion.identity, spawnLevel);
        cell.gameObject.layer = spawnLayerValueInInt;
        cell.gameObject.name = $"Cell{spawnObjectNumber}";
        cell.ActivateCellMovement();

        lastSpawnedCell = cell;
        spawnObjectNumber++;
    }

    public void SetConfirmPanelActive(Cell callerCell) {
        confirmPanel.gameObject.SetActive(true);
        this.callerCell = callerCell;
    }

    private void DestroyCell() {
        Destroy(lastSpawnedCell.gameObject);
        lastSpawnedCell = null;
    }

}