using System;
using UnityEngine;

public class MovementManager : MonoBehaviour {

    [SerializeField] private float moveDistance = 0.15f;
    [SerializeField] private Vector3 rotationAngle = new Vector3(0f, 0f, 10f);

    private Cell cell;
    private Vector3 moveDir;
    private bool moveRequested = false;
    private bool rotationRequested = false;

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

            if(Input.GetKeyDown(KeyCode.R)) {
                rotationRequested = true;
            }
        }
    }

    private void FixedUpdate() {
        if (moveRequested) {
            transform.position += moveDir * moveDistance;
            moveRequested = false;
        }

        if(rotationRequested) {
            //transform.rotation = Quaternion.Euler(0f, 0f, transform.rotation.z + 10f);
            transform.Rotate(rotationAngle);
            rotationRequested = false;
        }
    }

    public void GoToPoint(Vector3 movePoint) {
        transform.position = movePoint;
    }

    public Transform GetObjectTransform(Cell cell) {
        return cell.transform;
    }

}