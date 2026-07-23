using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PerceptUI : MonoBehaviour {

    public static PerceptUI Instance { get; private set; }

    [SerializeField] private Button spawnSquareButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Square square;


    private void Awake() {

        if(Instance != null) {
            Debug.Log("More than one Instance for PerceptUI!");
        }
        Instance = this;

        spawnSquareButton.onClick.AddListener(() => {
            SpawnTheSquare();
            confirmButton.gameObject.SetActive(true);
            cancelButton.gameObject.SetActive(true);
        });

        confirmButton.onClick.AddListener(() => {
            confirmButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        });

        cancelButton.onClick.AddListener(() => {
            square.DestroySelf();
            confirmButton.gameObject.SetActive(false);
            cancelButton.gameObject.SetActive(false);
        });

        confirmButton.gameObject.SetActive(false);
        cancelButton.gameObject.SetActive(false);
    }

    private void SpawnTheSquare() {

        Vector2 spawnLocation = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Square squareCell = Instantiate(square, spawnLocation, Quaternion.identity);
    }

    private void Update() {
    }

}