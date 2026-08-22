using UnityEditor.Rendering;
using UnityEngine;
using System.Collections;

public class BoxCastManager : MonoBehaviour {

    [SerializeField] private float boxCastLength = 0.1f;
    [SerializeField] private float boxCastWidth = 0.1f;
    [SerializeField] private float extraDistance = 0.1f;
    [SerializeField] private float distanceBetweenCell = 0.2f;
    [SerializeField] private LayerMask castHitLayer;
    [SerializeField] private Transform[] boxCastOriginPointsArray;
    [SerializeField] private Transform[] cellSnapPointsArray;
    [SerializeField] private Transform cellCenter;

    private Cell cell;
    private Cell lastHitCell;
    private PolygonCollider2D polygonCollider;
    private bool isSnapped = false;
    private bool atleastOneBoxCastHit = false;


    private void Awake() {
        cell = GetComponent<Cell>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update() {
        if(!cell.IsObjectSelected()) {
            ActivateBoxCast();
        }

        if (lastHitCell != null) {
            if (atleastOneBoxCastHit == false) {
                lastHitCell.SetSnapVisualInactive();
                lastHitCell = null;
            }
        }

    }
        

    private void ActivateBoxCast() {
        for (int i = 0; i < boxCastOriginPointsArray.Length; i++) {

            if (cell.BoxCastInRouteManager(boxCastOriginPointsArray[i])) {
                continue;
            }

            Vector2 boxCastOriginGlobalPoint = boxCastOriginPointsArray[i].position;
            Vector2 boxCastSize = new Vector2(boxCastLength, boxCastWidth);
            Vector2 boxCastDirectionNormalized = (boxCastOriginGlobalPoint - (Vector2)cellCenter.position).normalized;
            float boxCastRotation = Mathf.Atan2(boxCastDirectionNormalized.y, boxCastDirectionNormalized.x) * Mathf.Rad2Deg + 90f;

            RaycastHit2D hit = Physics2D.BoxCast(boxCastOriginGlobalPoint, boxCastSize, boxCastRotation, boxCastDirectionNormalized, extraDistance, castHitLayer);

            //DrawBoxCast(boxCastOriginGlobalPoint, boxCastSize,boxCastRotation, boxCastDirectionNormalized);

            if ((hit.collider != null) && (hit.collider != polygonCollider)) {
                if (hit.collider.TryGetComponent<Cell>(out Cell targetCell)) {
                    if (targetCell.IsObjectSelected()) {
                        PolygonCollider2D targetCollider2D = targetCell.GetCollider();
                        Vector2 targetColliderCenter = targetCollider2D.bounds.center;

                        atleastOneBoxCastHit = true;
                        lastHitCell = targetCell;
                        targetCell.SnapAtPoint(cellSnapPointsArray[i].position, boxCastDirectionNormalized, boxCastOriginPointsArray[i]);
                        return;
                    } else {
                        targetCell.SetRoute(boxCastDirectionNormalized, boxCastOriginPointsArray[i], cell);
                    }
                }
            }
        }

        atleastOneBoxCastHit = false;
    }

    //private void DrawBoxCast(Vector2 boxCastOrigin, Vector2 boxSize, float boxCastRotation, Vector2 direction) {
    //    Vector2 halfSize = boxSize * 0.5f;
    //    Quaternion rotation = Quaternion.Euler(0, 0, boxCastRotation);

    //    Vector2 topLeft = boxCastOrigin + (Vector2)(rotation * new Vector2(-halfSize.x, halfSize.y));
    //    Vector2 topRight = boxCastOrigin + (Vector2)(rotation * new Vector2(halfSize.x, halfSize.y));
    //    Vector2 bottomLeft = boxCastOrigin + (Vector2)(rotation * new Vector2(-halfSize.x, -halfSize.y));
    //    Vector2 bottomRight = boxCastOrigin + (Vector2)(rotation * new Vector2(halfSize.x, -halfSize.y));

    //    // Box at start
    //    Debug.DrawLine(topLeft, topRight, Color.red);
    //    Debug.DrawLine(topRight, bottomRight, Color.red);
    //    Debug.DrawLine(bottomRight, bottomLeft, Color.red);
    //    Debug.DrawLine(bottomLeft, topLeft, Color.red);

    //    // Box at end of the cast
    //    Vector2 offset = direction * extraDistance;
    //    Debug.DrawLine(topLeft + offset, topRight + offset, Color.red);
    //    Debug.DrawLine(topRight + offset, bottomRight + offset, Color.red);
    //    Debug.DrawLine(bottomRight + offset, bottomLeft + offset, Color.red);
    //    Debug.DrawLine(bottomLeft + offset, topLeft + offset, Color.red);

    //    // Connect start box to end box (shows the swept path)
    //    Debug.DrawLine(topLeft, topLeft + offset, Color.red);
    //    Debug.DrawLine(topRight, topRight + offset, Color.red);
    //    Debug.DrawLine(bottomLeft, bottomLeft + offset, Color.red);
    //    Debug.DrawLine(bottomRight, bottomRight + offset, Color.red);

    //}

    private void SetLastBoxCastHitCell(Cell lastHitCell) {
        this.lastHitCell = lastHitCell;
    }

    public bool IsSnappingActive() {
        return isSnapped;
    }

    public Transform GetBoxCastOriginPoint(int index) {
        return boxCastOriginPointsArray[index];
    }

}