using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class EditMapOptionsUI : MonoBehaviour {

    public static EditMapOptionsUI Instance { get; private set; }

    [SerializeField] private Button spawnSquareButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Transform confirmPanel;
    [SerializeField] private Cell square;
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

        confirmButton.onClick.AddListener(() => {
            confirmPanel.gameObject.SetActive(false);
        });

        cancelButton.onClick.AddListener(() => {
            DestroyCell();
            confirmPanel.gameObject.SetActive(false);
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

    private void Update() {
    }

}