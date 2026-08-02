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
    [SerializeField] private Transform customMap;
    [SerializeField] private LayerMask gridSpawnLayer;

    private int spawnLayerValueInInt;
    private Cell lastSpawnedCell;


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
            DestroyCell();
            confirmPanel.gameObject.SetActive(false);
        });

        confirmPanel.gameObject.SetActive(false);
    }

    private void OnConfirmPressed() {
        confirmPanel.gameObject.SetActive(false);

        Transform updatedTransform = lastSpawnedCell.GetTransform(lastSpawnedCell);
        lastSpawnedCell = null;
    }

    private void SpawnTheCell(Cell cell) {

        Vector2 spawnLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cell = Instantiate(cell, spawnLocation, Quaternion.identity, customMap);
        cell.gameObject.layer = spawnLayerValueInInt;
        cell.ActivateCellMovement();

        lastSpawnedCell = cell;
    }

    private void DestroyCell() {
        Destroy(lastSpawnedCell.gameObject);
        lastSpawnedCell = null;
    }

}