using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WorldView {

    public delegate void CellAliveChange (Cell c);
    public event CellAliveChange OnCellDeath;
    public event CellAliveChange OnCellBirth;

    public WorldView (World world, GameObject cellPrefab, GameManager gm, Dictionary<Cell.CellStatus, Color> colorOptions, float offset) {
        this.world = world;
        this.gm = gm;

        this.colorOptions = colorOptions;
        this.cellViews = new CellView[this.world.width, this.world.height, this.world.depth];
        for (var x = 0; x < this.world.width; ++x) {
            for (var y = 0; y < this.world.height; ++y) {
                for (var z = 0; z < this.world.depth; ++z) {
                    // register for state change
                    var cell = this.world.cells[x, y, z];
                    cell.OnCellStatusChanges += this.OnCellStatusChanges;
                    // create view
                    var offsetVector = offset * new Vector3 (x, y, z);
                    this.cellViews[x, y, z] = new CellView (cell, cellPrefab, this.gm, this.colorOptions, offsetVector);
                }
            }
        }
    }

    private void OnCellStatusChanges (Cell c, Cell.CellStatus prev) {
        if (Cell.CellStatus.Alive != prev && Cell.CellStatus.Alive == c.Status) {
            if (null != this.OnCellBirth) {
                this.OnCellBirth (c);
            }
        }
        else if (Cell.CellStatus.Alive == prev && Cell.CellStatus.Alive != c.Status) {
            if (null != this.OnCellDeath) {
                this.OnCellDeath (c);
            }
        }
    }

    private World world;
    private GameManager gm;
    private Dictionary<Cell.CellStatus, Color> colorOptions;
    private CellView[,,] cellViews;

}
