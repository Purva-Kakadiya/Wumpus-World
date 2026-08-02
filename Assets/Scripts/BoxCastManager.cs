using UnityEngine;

public class BoxCastManager : MonoBehaviour {

    [SerializeField] private float boxCastLength = 0.1f;
    [SerializeField] private float boxCastWidth = 0.1f;
    [SerializeField] private float extraDistance = 0.1f;
    [SerializeField] private float distanceBetweenCell = 0.2f;
    [SerializeField] private LayerMask castHitLayer;
    [SerializeField] private Transform[] boxCastOriginPointsArray;
    [SerializeField] private Transform cellCenter;

    private Cell cell;
    private PolygonCollider2D polygonCollider;

    private void Awake() {
        cell = GetComponent<Cell>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update() {
        if(!cell.IsObjectSelected()) {
            ActivateBoxCast();
        }
    }
        

    private void ActivateBoxCast() {

        for (int i = 0; i < boxCastOriginPointsArray.Length; i++) {
            Vector2 boxCastOriginGlobalPoint = boxCastOriginPointsArray[i].position;
            Vector2 boxCastSize = new Vector2(boxCastLength, boxCastWidth);
            Vector2 boxCastDirectionNormalized = (boxCastOriginGlobalPoint - (Vector2)cellCenter.position).normalized;

            RaycastHit2D hit = Physics2D.BoxCast(boxCastOriginGlobalPoint, boxCastSize, 0f, boxCastDirectionNormalized,extraDistance, castHitLayer);

            if ((hit.collider != null) && (hit.collider != polygonCollider)) {
                if (hit.collider.TryGetComponent<Cell>(out Cell targetCell)) {
                    if (targetCell.IsObjectSelected()) {
                        PolygonCollider2D targetCollider2D = targetCell.GetCollider();
                        Vector2 targetColliderCenter = targetCollider2D.bounds.center;
                        targetCell.SnapAtPoint(boxCastOriginGlobalPoint + (boxCastDirectionNormalized * targetCollider2D.bounds.extents) + (boxCastDirectionNormalized * distanceBetweenCell));
                    }
                }
            }
        }

        //for (int pathIndex = 0; pathIndex < polygonCollider.pathCount; pathIndex++) {
        //    Vector2[] pathPoints = polygonCollider.GetPath(pathIndex);
        //    int pointsCount = pathPoints.Length;

        //    for (int i = 0; i < pointsCount; i++) {
        //        Vector2 currentGlobalPoint = polygonCollider.transform.TransformPoint(pathPoints[i]);
        //        Vector2 nextGlobalPoint = polygonCollider.transform.TransformPoint(pathPoints[(i + 1) % pointsCount]);
        //        Vector2 boxCastOrigin = (currentGlobalPoint + nextGlobalPoint) / 2;
        //        Vector2 boxCastDirection = (boxCastOrigin - cellCenter).normalized;

        //        Vector2 boxCastSize = new Vector2(boxCastLength, boxCastWidth);
        //        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin + (boxCastDirection * extraDistance), boxCastSize, 0f, boxCastDirection, extraDistance, castHitLayer);

        //        Debug.Log(boxCastDirection);
        //        if ((hit.collider != null) && (hit.collider != polygonCollider)) {
        //        if (hit.collider.TryGetComponent<Cell>(out Cell targetCell)) {
        //          if (targetCell.IsObjectSelected()) {
        //               PolygonCollider2D targetCollider2D = targetCell.GetCollider();
        //                Vector2 targetColliderCenter = targetCollider2D.bounds.center;
        //                targetCell.SnapAtPoint(boxCastOrigin + (boxCastDirection * targetCollider2D.bounds.extents) + (boxCastDirection * distanceBetweenCell));
        //          }
        //       }
        //}
        //    }
        //}
    }

}