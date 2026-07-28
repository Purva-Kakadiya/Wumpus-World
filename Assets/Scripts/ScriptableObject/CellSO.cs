using UnityEngine;

[CreateAssetMenu(fileName = "CellSO", menuName = "Scriptable Objects/CellSO")]
public class CellSO : ScriptableObject {

    public string cellName;
    public Transform cellPrefab;

}