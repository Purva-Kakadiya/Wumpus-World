using NUnit.Framework;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using UnityEngine;

//[System.Serializable]
//public struct RoutePair {
//    private Transform door1;
//    private Transform door2;

//    public RoutePair(Transform door1, Transform door2) {
//        this.door1 = door1;
//        this.door2 = door2;
//    }
//}

public class RoutingManager : MonoBehaviour {

    public static RoutingManager Instance { get; private set; }

    [SerializeField] private List<(Transform door1, Transform door2)> routePairList = new List<(Transform, Transform)>();

    private void Awake() {
        Instance = this;
    }

    private void Update() {
        foreach((Transform door1, Transform door2) pair in routePairList) {
            Debug.Log("door1 is: " + pair.door1.position + " door2 is: " + pair.door2.position);
        }
    }

    public void SetRountePair(Transform door1, Transform door2) {
        routePairList.Add((door1, door1));
    }

}