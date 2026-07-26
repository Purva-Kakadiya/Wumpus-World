using System;
using UnityEngine;
using UnityEngine.UI;

public class Square : MonoBehaviour {

    [SerializeField] private float moveDistance = 1f;
    [SerializeField] private Button squareSelectButton;

    private PolygonCollider2D polygonCollider;
    private BoxCastAndSnappingManager boxCastAndSnappingManager;

    private void Awake() {

        polygonCollider = GetComponent<PolygonCollider2D>();
        boxCastAndSnappingManager = GetComponent<BoxCastAndSnappingManager>();

        squareSelectButton.onClick.AddListener(() => {
            ActivateSquareMovement();
        });
    }

    public PolygonCollider2D GetCollider() {
        return polygonCollider;
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

        for (int pathIndex = 0; pathIndex < polygonCollider.pathCount; pathIndex++) {
            Vector2[] pathPoints = polygonCollider.GetPath(pathIndex);
            int pointsCount = pathPoints.Length;

            for (int i = 0; i < pointsCount; i++) {
                Vector2 currentGlobalPoint = polygonCollider.transform.TransformPoint(pathPoints[i]);
                Vector2 nextGlobalPoint = polygonCollider.transform.TransformPoint(pathPoints[(i + 1) % pointsCount]);
                Vector2 edgeMiddlePoint = (currentGlobalPoint + nextGlobalPoint) / 2;
                Vector2 boxCastDirection = (edgeMiddlePoint - (Vector2)polygonCollider.bounds.center).normalized;

                boxCastAndSnappingManager.ActivateBoxCastAndSnapping(edgeMiddlePoint, boxCastDirection, polygonCollider);
            }
        }
    }

    public void SnapToThePoint(Vector2 snapPoint) {
        transform.position = snapPoint;
    }

    public void ActivateSquareMovement() {
        SelectionManager.Instance.SetActiveObject(gameObject);
    }

    public void DeactivateSquareMovement() {
        SelectionManager.Instance.SetEveryObjectDeactive();
    }

}