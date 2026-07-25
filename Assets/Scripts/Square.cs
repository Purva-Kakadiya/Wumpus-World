using System;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private Button squareSelectButton;
    [SerializeField] private LayerMask castHitLayer;
    [SerializeField] private float edgeLength = 0.5f;
    [SerializeField] private float extraDistance = 0.5f;
    [SerializeField] private float boxCastMaxDistance = 0.5f;

    private Color rayColor = Color.red;
    private BoxCollider2D boxCollider2D;

    private void Awake() {

        boxCollider2D = gameObject.GetComponent<BoxCollider2D>();

        squareSelectButton.onClick.AddListener(() => {
            ActivateSquareMovement();
        });
    }

    private void Update() {

        if (SelectionManager.Instance.GetActiveObject() == gameObject) {

            SquareMovementHandler();
        } else {

            ActivateBoxCast();
        }

    }

    private void SquareMovementHandler() {

        Vector3 moveDir = new Vector3();
        if (Input.GetKeyDown(KeyCode.W)) {
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

        transform.position += moveDir * moveDistance;
    }

    private void ActivateBoxCast() {

        Vector2 squareCenterLocal = boxCollider2D.offset;
        Vector2 localRightEdgeCenter = squareCenterLocal + new Vector2(boxCollider2D.size.x / 2, 0f);
        Vector2 globalRightEdgeCenter = transform.TransformPoint(localRightEdgeCenter);
        Vector2 boxSize = new Vector2(edgeLength, boxCollider2D.size.y);

        RaycastHit2D hit = Physics2D.BoxCast(globalRightEdgeCenter, boxSize, 0f, Vector2.right, boxCastMaxDistance, castHitLayer);

        if ((hit.collider != null) && (hit.collider != GetComponent<BoxCollider2D>())) {
            if (hit.collider.TryGetComponent<Square>(out Square targetSquare)) {
                targetSquare.SnapToTheRightSide(globalRightEdgeCenter + new Vector2(extraDistance, 0f));
            }
        }
    }

    private void SnapToTheRightSide(Vector2 snapPoint) {
        Vector2 globalSnapPointForCenter = snapPoint + new Vector2(boxCollider2D.size.x / 2, 0f);
        transform.position = globalSnapPointForCenter;
    }

    public void ActivateSquareMovement() {
        SelectionManager.Instance.SetActiveObject(gameObject);
    }

    public void DeactivateSquareMovement() {
        SelectionManager.Instance.SetEveryObjectDeactive();
    }

}