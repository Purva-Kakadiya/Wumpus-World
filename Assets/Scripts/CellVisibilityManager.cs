using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class CellVisibilityManager : MonoBehaviour {

    private Level level;

    private void Awake() {
        level = GetComponent<Level>();
    }

    public void SetVisitedCell(Cell visitingCell) {
        visitingCell.SetCellVisibilityState(CellVisibilityState.Visited);
    }

    public void SetVisitableCell(Cell visitingCell) {
        List<Cell> visitableCells = visitingCell.GetVisitableCells();
        foreach(Cell visitableCell in visitableCells) {
            visitableCell.SetCellVisibilityState(CellVisibilityState.Visitable);
        }
    }

    public void SetUnvisitedCells(Cell currentCell) {
        List<Cell> unvisitedCells = currentCell.GetVisitableCells();
        foreach(Cell unvisitedCell in unvisitedCells) {
            if(unvisitedCell.GetCellVisibilityState() != CellVisibilityState.Visited) {
                unvisitedCell.SetCellVisibilityState(CellVisibilityState.Unvisitable);
            } 
        }
    }

}