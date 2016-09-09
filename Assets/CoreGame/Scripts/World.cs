using UnityEngine;
using System.Collections;

public class World {

    public Cell[,,] cells;
    public int width;
    public int height;
    public int depth;

    public int GetAliveNeighbours (int x, int y, int z) {
        var aliveCounter = 0;

        var xStart = x - 1 < 0 ? 0 : x - 1;
        var xEnd = x + 1 >= this.width ? this.width - 1 : x + 1;

        var yStart = y - 1 < 0 ? 0 : y - 1;
        var yEnd = y + 1 >= this.height ? this.height - 1 : y + 1;

        var zStart = z - 1 < 0 ? 0 : z - 1;
        var zEnd = z + 1 >= this.depth ? this.depth - 1 : z + 1;

        var centerCell = this.cells[x, y, z];
        for (var xi = xStart; xi <= xEnd; ++xi) {
            for (var yi = yStart; yi <= yEnd; ++yi) {
                for (var zi = zStart; zi <= zEnd; ++zi) {
                    var cell = this.cells[xi, yi, zi];

                    if (cell != centerCell && Cell.CellStatus.Alive == cell.Status) {
                        ++aliveCounter;
                    }
                }
            }
        }

        return aliveCounter;
    }

    public World (int width, int height, int depth) {
        this.width = width;
        this.height = height;
        this.depth = depth;

        this.cells = new Cell[width, height, depth];
        for (var x = 0; x < this.width; ++x) {
            for (var y = 0; y < this.height; ++y) {
                for (var z = 0; z < this.depth; ++z) {
                    this.cells[x, y, z] = new Cell (x, y, z);
                }
            }
        }
    }

}
