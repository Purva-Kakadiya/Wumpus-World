using System;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private Button squareSelectButton;
    [SerializeField] private LayerMask castHitLayer;
    [SerializeField] private float edgeLength = 1f;
    [SerializeField] private float boxHeight = 0.5f;

    private bool isMovable = false;
    private Color rayColor = Color.red;

    private void Awake() {
        squareSelectButton.onClick.AddListener(() => {
            ActivateSquareMovement();
        });
    }

    private void Update() {

        if(SelectionManager.Instance.GetActiveObject() == gameObject) {
            Vector3 moveDir = new Vector2();
            if(Input.GetKeyDown(KeyCode.W)) {
                moveDir = Vector3.up;
            }
            if (Input.GetKeyDown(KeyCode.S)) {
                moveDir = Vector3.down;
            }
            if (Input.GetKeyDown(KeyCode.A)) {
                moveDir = Vector3.left;
            }
            if (Input.GetKeyDown(KeyCode.D)) {
                moveDir = Vector3.right;
            }

            transform.position += moveDir * moveDistance * Time.deltaTime;
        }


        BoxCollider2D boxCollider = gameObject.GetComponent<BoxCollider2D>();

        Vector2 squareCenterLocal = boxCollider.offset;
        Vector2 localRightEdgeCenter = squareCenterLocal + new Vector2(boxCollider.size.x / 2, 0f);
        Vector2 globalRightEdgeCenter = transform.TransformPoint(localRightEdgeCenter);
        Vector2 boxSize = new Vector2(edgeLength, edgeLength);

        RaycastHit2D hit = Physics2D.BoxCast(globalRightEdgeCenter + new Vector2(boxHeight, 0f), boxSize, 0f, Vector2.right, boxHeight, castHitLayer);

        if (hit.collider != GetComponent<BoxCollider2D>()) {
            Debug.Log("you boxCast hit: " + hit.collider.name);
        }

        Debug.DrawRay(globalRightEdgeCenter + new Vector2(0, edgeLength / 2), Vector2.right * (edgeLength + boxHeight), rayColor);
        Debug.DrawRay(globalRightEdgeCenter - new Vector2(0, edgeLength / 2), Vector2.right * (edgeLength + boxHeight), rayColor);
        Debug.DrawRay(globalRightEdgeCenter + new Vector2(edgeLength, edgeLength / 2), Vector2.down * (edgeLength + boxHeight), rayColor);

    }

    public void ActivateSquareMovement() {
        isMovable = true;
        SelectionManager.Instance.SetActiveObject(gameObject);
    }

    public void DeactivateSquareMovement() {
        isMovable = false;
    }

    public void DestroySelf() {
        Destroy(gameObject);
    }

    //public void GameInput_MoveUp(object sender, EventArgs e) {
    //    if (isMovable) {
    //        transform.position += new Vector3(0, moveDistance * Time.deltaTime);
    //    }
    //}

    //public void GameInput_MoveDown(object sender, EventArgs e) {
    //    if (isMovable) {
    //        transform.position += new Vector3(0, -moveDistance * Time.deltaTime);
    //    }
    //}

    //public void GameInput_MoveLeft(object sender, EventArgs e) {
    //    if (isMovable) {
    //        transform.position += new Vector3(-moveDistance * Time.deltaTime, 0);
    //    }
    //}

    //public void GameInput_MoveRight(object sender, EventArgs e) {
    //    if (isMovable) {
    //        transform.position += new Vector3(moveDistance * Time.deltaTime, 0);
    //    }
    //}

}