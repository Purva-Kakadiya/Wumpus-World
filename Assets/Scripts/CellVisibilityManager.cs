using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class CellVisibilityManager : MonoBehaviour {

    [SerializeField] private GameObject cloudPrefab;

    private Level level;
    private ParticleSystem cloudParticleSystem;

    private void Awake() {
        level = GetComponent<Level>();
    }

    public void SpawnCloudForUnknownCells() {
        List<Cell> cells = level.GetComponentsInChildren<Cell>().ToList();
        foreach(Cell cell in cells) {
            if(cell.GetCellVisibilityState() == CellVisibilityState.Unknown) {
                cell.SetCloudParticleActivity(true);

                //GameObject cloudParticle = Instantiate(cloudPrefab, cell.transform);
                //cloudParticle.transform.localPosition = Vector3.zero;

                //cloudParticleSystem = cloudPrefab.GetComponentInChildren<ParticleSystem>();
                //cloudParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

                //Vector2 cloudShapeScale = cell.GetCloudParticleShapeScale();
                //Debug.Log($"{cell.name}: wants: {cloudShapeScale}, ParticleSystem found: {cloudParticleSystem != null}");

                //var cloudParticleShape = cloudParticleSystem.shape;
                //cloudParticleShape.scale = new Vector3(cloudShapeScale.x, cloudShapeScale.y, 1f);
                //cloudParticleSystem.Play();

                //Debug.Log($"{cell.name}: has scale of: {cloudParticleSystem.shape.scale}");
            }
        }
    }

    public void SetVisitedCell(Cell visitingCell) {
        visitingCell.SetCellVisibilityState(CellVisibilityState.Visited);
        visitingCell.SetCloudParticleActivity(false);
    }

    public void SetVisitableCell(Cell visitingCell) {
        List<Cell> visitableCells = visitingCell.GetVisitableCells();
        foreach(Cell visitableCell in visitableCells) {
            visitableCell.SetCellVisibilityState(CellVisibilityState.Visitable);
            visitableCell.SetCloudParticleActivity(false);
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