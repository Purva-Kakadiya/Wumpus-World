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
    [SerializeField] private Button rotateButton;
    [SerializeField] private Transform confirmPanel;
    [SerializeField] private Cell square;
    [SerializeField] private Cell triangle;
    [SerializeField] private Cell hexagon;
    [SerializeField] private Transform customMap;
    [SerializeField] private LayerMask gridSpawnLayer;

    private Cell cell;
    private int spawnLayerValueInInt;


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
            confirmPanel.gameObject.SetActive(false);
        });

        cancelButton.onClick.AddListener(() => {
            DestroyCell();
            confirmPanel.gameObject.SetActive(false);
        });

        rotateButton.onClick.AddListener(() => {
            OnRotateButtonPressed?.Invoke(this, EventArgs.Empty);
        });

        confirmPanel.gameObject.SetActive(false);
    }
    private void SpawnTheCell(Cell cell) {

        Vector2 spawnLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        cell = Instantiate(cell, spawnLocation, Quaternion.identity, customMap);
        cell.gameObject.layer = spawnLayerValueInInt;
    }

    private void DestroyCell() {
        Destroy(cell.gameObject);
    }

}