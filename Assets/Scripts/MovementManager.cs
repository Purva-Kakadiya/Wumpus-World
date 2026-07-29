using System;
using UnityEngine;

public class MovementManager : MonoBehaviour {

    [SerializeField] private float moveDistance = 0.15f;
    [SerializeField] private float rotationAngle = 10f;

    private Cell cell;
    private Vector3 moveDir;
    private bool moveRequested = false;

    private void Awake() {
        cell = GetComponent<Cell>();
    }

    private void Update() {

        if(cell.IsObjectSelected()) {
            moveDir = new Vector3();
            if (Input.GetKey(KeyCode.W)) {
                moveDir = Vector3.up;
            }
            if (Input.GetKey(KeyCode.S)) {
                moveDir = Vector3.down;
            }
            if (Input.GetKey(KeyCode.A)) {
                moveDir = Vector3.left;
            }
            if (Input.GetKey(KeyCode.D)) {
                moveDir = Vector3.right;
            }

            if (moveDir != Vector3.zero) {
                moveRequested = true;
            }
        }
    }

    private void FixedUpdate() {
        if (moveRequested) {
            transform.position += moveDir * moveDistance;
            moveRequested = false;
        }

    }

    public void GoToPoint(Vector3 movePoint) {
        transform.position = movePoint;
    }

    public void RotateObject() {
        if (SelectionManager.Instance.GetActiveObject() != null) {
            Cell activeCell = (SelectionManager.Instance.GetActiveObject()).GetComponent<Cell>();
            float activeRotationOnZAxis = activeCell.transform.eulerAngles.z;
            activeCell.transform.rotation = Quaternion.Euler(0f, 0f, activeRotationOnZAxis + rotationAngle);
        }

    }

}