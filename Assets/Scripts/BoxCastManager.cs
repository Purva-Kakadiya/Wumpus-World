using UnityEngine;

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
    private Cell lastCell;
    private PolygonCollider2D polygonCollider;
    private bool isSnapped = false;

    private void Awake() {
        cell = GetComponent<Cell>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update() {
        if(!cell.IsObjectSelected()) {
            ActivateBoxCast();
        }
        if(isSnapped == false && lastCell != null) {
            lastCell.SetSnapVisualInactive();
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

                        isSnapped = true;
                        lastCell = targetCell;
                        targetCell.SnapAtPoint(cellSnapPointsArray[i].position, boxCastDirectionNormalized);
                        //targetCell.SnapAtPoint(boxCastOriginGlobalPoint + (boxCastDirectionNormalized * targetCollider2D.bounds.extents) + (boxCastDirectionNormalized * distanceBetweenCell));
                    }
                }
            }
        }
    }

    public bool IsSnappingActive() {
        return isSnapped;
    }

}