using UnityEngine;

public class BoxCastAndSnappingManager : MonoBehaviour {

    [SerializeField] private LayerMask castHitLayer;
    [SerializeField] private float distanceBetweenCell = 0.25f;
    [SerializeField] private float extraDistance = 0.1f;
    [SerializeField] private float boxCastLength = 0.05f;
    [SerializeField] private float boxCastWidth = 1f;

    public void ActivateBoxCastAndSnapping(Vector2 boxCastOrigin, Vector2 direction, PolygonCollider2D originCollider2D) {
        Vector2 boxCastSize = new Vector2(boxCastLength, boxCastWidth);
        RaycastHit2D hit = Physics2D.BoxCast(boxCastOrigin + (direction * extraDistance), boxCastSize, 0f, direction, extraDistance, castHitLayer);


        if ((hit.collider != null) && (hit.collider != originCollider2D)) {
            if (hit.collider.TryGetComponent<Square>(out Square targetSquare)) {
                PolygonCollider2D targetCollider2D = targetSquare.GetCollider();
                Vector2 targetColliderCenter = targetCollider2D.bounds.center;
                targetSquare.SnapToThePoint(boxCastOrigin + (direction * targetCollider2D.bounds.extents) + (direction * distanceBetweenCell));
            }
        }
    }

}