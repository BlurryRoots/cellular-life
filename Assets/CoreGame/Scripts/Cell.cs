using UnityEngine;
using System.Collections;

public class Cell {

    public enum CellStatus {
        Untouched,
        Alive,
        Dead,
    }
    public CellStatus Status {
        get
        {
            return this.status;
        }
        set
        {
            var prev = this.status;
            this.status = value;

            if (null != this.OnCellStatusChanges) {
                this.OnCellStatusChanges (this, prev);
            }
        }
    }

    public delegate void CellStatusChanged (Cell cell, CellStatus prev);
    public event CellStatusChanged OnCellStatusChanges;

    public int x;
    public int y;
    public int z;

    public override string ToString () {
        return string.Format ("cell{{x: {0}, y: {1}, z: {2}}}", this.x, this.y, this.z);
    }

    public Cell (int x, int y, int z) {
        this.x = x;
        this.y = y;
        this.z = z;
    }

    private CellStatus status;

}
