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
    private bool isBoxCastActive = false;


    private void Awake() {
        cell = GetComponent<Cell>();
        polygonCollider = GetComponent<PolygonCollider2D>();
    }

    private void Update() {
        if(!cell.IsObjectSelected() && isBoxCastActive == false) {

            StartCoroutine(WaitForFewSeconds());
        }

        if(isBoxCastActive == true) {
            ActivateBoxCast();
        }

        if (lastHitCell != null) {
            if (atleastOneBoxCastHit == false) {
                lastHitCell.SetSnapVisualInactive();
                lastHitCell = null;
            }
        }

    }

    IEnumerator WaitForFewSeconds() {
        isBoxCastActive = true;

        yield return new WaitForSeconds(2f);
    }
        

    private void ActivateBoxCast() {
        for (int i = 0; i < boxCastOriginPointsArray.Length; i++) {

            if (cell.BoxCastInRouteManager(boxCastOriginPointsArray[i])) {
                continue;
            }

            Vector2 boxCastOriginGlobalPoint = boxCastOriginPointsArray[i].position;
            Vector2 boxCastSize = new Vector2(boxCastLength, boxCastWidth);
            Vector2 boxCastDirectionNormalized = (boxCastOriginGlobalPoint - (Vector2)cellCenter.position).normalized;

            RaycastHit2D hit = Physics2D.BoxCast(boxCastOriginGlobalPoint, boxCastSize, 0f, boxCastDirectionNormalized,extraDistance, castHitLayer);

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
                        //Debug.Log("boxCast cell is: " + transform.name + " hit cell is: " + targetCell.name);
                        targetCell.SetRoute(boxCastDirectionNormalized, boxCastOriginPointsArray[i], cell);
                    }
                }
            }
        }

        atleastOneBoxCastHit = false;
    }

    //private void SetLastBoxCastHitCell(Cell lastHitCell) {
    //    this.lastHitCell = lastHitCell;
    //}

    public bool IsSnappingActive() {
        return isSnapped;
    }

    public Transform GetBoxCastOriginPoint(int index) {
        return boxCastOriginPointsArray[index];
    }

}