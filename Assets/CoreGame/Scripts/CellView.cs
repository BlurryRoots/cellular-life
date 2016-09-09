using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellView {

    public CellView (Cell cell, GameObject cellPrefab, GameManager gm, Dictionary<Cell.CellStatus, Color> colors, Vector3 offset) {
        this.cell = cell;
        this.cell.OnCellStatusChanges += this.OnCellStatusChanges;

        this.gm = gm;

        this.cellVisualization = GameObject.Instantiate (cellPrefab);
        // attach this to game manager so the scene does not get messed up
        this.cellVisualization.transform.SetParent (this.gm.cellRootObject.transform);
        this.cellVisualization.name = this.cell.ToString ();

        this.colors = colors;
        this.UpdateColor ();

        var mr = this.cellVisualization.GetComponent<MeshRenderer> ();
        this.cellVisualization.transform.position = new Vector3 (
            mr.bounds.extents.x + offset.x + this.cell.x,
            mr.bounds.extents.y + offset.y + this.cell.y,
            mr.bounds.extents.z + offset.z + this.cell.z
        );
    }

    private void OnCellStatusChanges (Cell c, Cell.CellStatus prev) {
        if (!this.colors.ContainsKey (c.Status)) {
            throw new UnityException ("Missing color for state " + c.Status);
        }

        this.UpdateColor ();
    }

    private void UpdateColor () {
        var material = this.cellVisualization.GetComponent<MeshRenderer> ().material;
        material.color = this.colors[this.cell.Status];
    }

    private Cell cell;
    private GameManager gm;
    private Dictionary<Cell.CellStatus, Color> colors;
    private GameObject cellVisualization;

}
