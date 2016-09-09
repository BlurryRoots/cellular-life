using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using BlurryRoots;

public class GameManager : BlurryBehaviour {

    public int width;
    public int height;
    public int depth;
    public float offset;

    public GameObject cellPrefab;

    public bool isPaused;
    public float intervall;
    public int generation;
    public int population;

    public ColorOption[] colors;

    public GameObject cellRootObject;

    protected override void OnStart () {
        if (null == this.colors || 0 == this.colors.Length) {
            throw new UnityException ("Specify colors!");
        }

        this.generation = 0;
        this.population = 0;

        this.world = new World (this.width, this.height, this.depth);

        var colorOptions = new Dictionary<Cell.CellStatus, Color> ();
        foreach (var o in this.colors) {
            colorOptions.Add (o.status, o.color);
        }

        this.worldView = new WorldView (this.world, this.cellPrefab, this, colorOptions, this.offset);
        this.worldView.OnCellBirth += this.OnCellBirth;
        this.worldView.OnCellDeath += this.OnCellDeath;

        this.world.cells[0, 0, 0].Status = Cell.CellStatus.Alive;
        this.world.cells[0, 2, 0].Status = Cell.CellStatus.Alive;
        this.world.cells[0, 0, 1].Status = Cell.CellStatus.Alive;
        this.world.cells[2, 2, 0].Status = Cell.CellStatus.Alive;

        StartCoroutine (this.EvoleGenerationCoroutine ());
	}

    protected override void OnUpdate () {
        if (Input.GetKeyDown (KeyCode.Space)) {
            this.isPaused = !this.isPaused;
        }
    }

    private void EvolveGeneration () {
        var cellChangeList = new List<CellChangeData> ();

        for (var x = 0; x < this.world.width; ++x) {
            for (var y = 0; y < this.world.height; ++y) {
                for (var z = 0; z < this.world.depth; ++z) {
                    var cell = this.world.cells[x, y, z];
                    var isAlive = Cell.CellStatus.Alive == cell.Status;
                    var aliveNeighbours = this.world.GetAliveNeighbours (x, y, z);

                    if (isAlive && (0 == aliveNeighbours || 2 < aliveNeighbours)) {
                        cellChangeList.Add (new CellChangeData () {
                            state = Cell.CellStatus.Dead,
                            x = x, y = y, z = z
                        });
                    }
                    else if (2 == aliveNeighbours) {
                        cellChangeList.Add (new CellChangeData () {
                            state = Cell.CellStatus.Alive,
                            x = x, y = y, z = z
                        });
                    }
                }
            }
        }

        for (var i = 0; i < cellChangeList.Count; ++i) {
            var change = cellChangeList[i];
            var cell = this.world.cells[change.x, change.y, change.z];
            cell.Status = change.state;
        }

        ++this.generation;
    }

    private IEnumerator EvoleGenerationCoroutine () {
        yield return new WaitForSeconds (this.intervall);

        while (true) {
            if (!this.isPaused) {
                this.EvolveGeneration ();
                yield return new WaitForSeconds (this.intervall);
            }

            yield return null;
        }
    }

    private void OnCellDeath (Cell c) {
        --this.population;
    }

    private void OnCellBirth (Cell c) {
        ++this.population;
    }

    private World world;
    private WorldView worldView;

}

[System.Serializable]
public class ColorOption {

    public Cell.CellStatus status;
    public Color color;

}

public class CellChangeData {

    public int x, y, z;
    public Cell.CellStatus state;

}
