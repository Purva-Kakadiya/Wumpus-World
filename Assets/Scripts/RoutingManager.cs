using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public struct RoutePair {
    public Transform door;
    public Transform outerDoor;

    public RoutePair(Transform door, Transform outerDoor) {
        this.door = door;
        this.outerDoor = outerDoor;
    }
}

public class RoutingManager : MonoBehaviour {

    //public static RoutingManager Instance { get; private set; }

    [SerializeField] private List<RoutePair> routePairList = new List<RoutePair>();
    private Dictionary<Transform, Transform> routePairs = new Dictionary<Transform, Transform>();

    private void Awake() {
        //Instance = this;
    }

    private void Update() {
        //foreach(RoutePair pair in routePairList) {
        //    Debug.Log("InnerDoor is: " + pair.door.name + " OuterDoor is: " + pair.outerDoor.name);
        //}
        //foreach(Transform door in routePairs.Keys) {
        //    Debug.Log("door1 is: " + door.parent.name + ":" + door.name + " & door2 is: " + routePairs[door].parent.name + ":" + routePairs[door].name);
        //}
    }

    public void SetRoutePair(Transform door, Transform outerDoor) {
        routePairs[door] = outerDoor;
        routePairList.Add(new RoutePair(door, outerDoor));
    }

    public bool IsDoorInRoutePair(Transform door) {
        if (routePairs.ContainsKey(door)) {
            return true;
        }
        return false;
    }

}