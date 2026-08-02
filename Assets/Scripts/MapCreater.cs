using UnityEngine;

public class MapCreater : MonoBehaviour {

    public Transform GetObjectTransform(Cell cell) {
        return cell.transform;
    }

    public void SetObjectTransform(Cell cell, Vector3 transformPosition, Vector3 transformRotation) {
        cell.transform.position = transformPosition;
        cell.transform.rotation = Quaternion.Euler(transformRotation);
    }

}