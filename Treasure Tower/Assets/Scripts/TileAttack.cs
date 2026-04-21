using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

/// <summary>
/// Subclass of Attack used exclusively by bosses.
/// Handles tile-based attacks that affect areas of the board.
/// Boss tile attacks have no range limit — they can target anywhere on the playable grid.
/// </summary>
public class TileAttack : Attack
{
    /// <summary>
    /// How many turns this tile attack remains active on the board.
    /// </summary>
    [SerializeField]
    private int totalTileAttackLifetime;

    /// <summary>
    /// how many turns the tile attack can affect any specific tile
    /// </summary>
    [SerializeField]
    private int tileAttackLifetime;

    /// <summary>
    /// How many tiles are affected when this attack is first placed.
    /// </summary>
    [SerializeField]
    private int initialTilesAffected;

    /// <summary>
    /// Whether this attack spreads to adjacent tiles each turn.
    /// </summary>
    [SerializeField]
    private bool doesSpread = false;

    /// <summary>
    /// The tiles currently affected by this attack.
    /// Stored as Vector2Int (x, y). Z is always 0 on our board.
    /// </summary>
    // DELETE - handled by BossRoom update
    private List<Vector2Int> currentTiles = new List<Vector2Int>();

    /// <summary>
    /// the shape of the attack (how it spreads/starts)
    /// by default, it has no shape and only exists as a singular tile (static)
    /// acceptable terms: plus, square, circle, row, double row, wave, double wave
    /// </summary>
    private string shape = "static";

    /// <summary>
    /// the visual sprite
    /// </summary>
    [SerializeField]
    private Tile tile;

    // --- Getters ---

    public Tile GetTileSprite()
    {
        return tile;
    }

    public int GetLifetime()
    {
        return this.tileAttackLifetime;
    }

    public int GetTotalLifetime()
    {
        return this.totalTileAttackLifetime;
    }

    public string GetShape()
    {
        return this.shape;
    }

    public int GetInitialTilesAffected()
    {
        return this.initialTilesAffected;
    }

    public bool GetDoesSpread()
    {
        return this.doesSpread;
    }

    // --- Core Methods --

    /// <summary>
    /// generates the initial location for the attack
    /// </summary>
    public List<Vector2Int> PlaceAttack(
        List<Vector2Int> allowableTiles,
        int firstRow,
        int gridLength
    )
    {
        List<Vector2Int> affectedTiles = new List<Vector2Int>();
        // this later checks if something is in the allowable tile list
        // because items in allowable are removed over time, it also prevents duplication
        List<Vector2Int> possibleAffectedTiles = new List<Vector2Int>();
        int amountToPlace = initialTilesAffected;

        // quick check so nothing is broken
        if (amountToPlace < allowableTiles.Count)
        {
            amountToPlace = System.Convert.ToInt32(allowableTiles.Count / 2);
        }

        // rows
        // these only need our starting row, and how wide the grid is
        if (shape == "row" || shape == "double row")
        {
            for (int x = 0; x < gridLength; x++)
            {
                Vector2Int location = new Vector2Int(x, firstRow);
                if (allowableTiles.Contains(location))
                {
                    affectedTiles.Add(location);
                }
                if (shape == "double row")
                {
                    location = new Vector2Int(x, firstRow + 1);
                    if (allowableTiles.Contains(location))
                    {
                        affectedTiles.Add(location);
                    }
                }
            }
        }

        // WIP - waves
        // does nothing currently except revert to default
        if (shape == "wave" || shape == "double wave")
        {
            shape = "static";
        }

        // other shapes
        for (int loopNum = 0; loopNum < amountToPlace; loopNum++)
        {
            // stop the loop if the list is now empty
            if (allowableTiles.Count == 0)
            {
                break;
            }

            // get a random index, then delete that possibility from the list
            int index = Random.Range(0, allowableTiles.Count);
            Vector2Int newTile = allowableTiles[index];

            affectedTiles.Add(newTile);
            allowableTiles.RemoveAt(index);

            // the x and y of the initial starting tile for the shape
            // in the case of static, we skip everything
            int x = newTile.x;
            int y = newTile.y;

            // now we're drawing shapes!
            switch (shape)
            {
                case "static":
                    return affectedTiles;
                case "plus":
                    // plus shape
                    // up, right, down, left
                    possibleAffectedTiles.Add(new Vector2Int(x, y - 1));
                    possibleAffectedTiles.Add(new Vector2Int(x + 1, y));
                    possibleAffectedTiles.Add(new Vector2Int(x, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y));
                    break;
                case "circle":
                    // circle, duh
                    // drawing a square, then a bit extra
                    // up/down - left, middle, right + same one row above
                    // right/left - side, then side 3 in same x value
                    // up
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x + 1, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y + 2));
                    possibleAffectedTiles.Add(new Vector2Int(x, y + 2));
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y + 2));
                    // right
                    possibleAffectedTiles.Add(new Vector2Int(x + 1, y));
                    possibleAffectedTiles.Add(new Vector2Int(x + 2, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x + 2, y));
                    possibleAffectedTiles.Add(new Vector2Int(x + 2, y - 1));
                    // down
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y - 1));
                    possibleAffectedTiles.Add(new Vector2Int(x, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x + 1, y - 1));
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y - 2));
                    possibleAffectedTiles.Add(new Vector2Int(x, y - 2));
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y - 2));
                    // left
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y));
                    possibleAffectedTiles.Add(new Vector2Int(x - 2, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x - 2, y));
                    possibleAffectedTiles.Add(new Vector2Int(x - 2, y - 1));
                    break;
                case "square":
                    // wow
                    // left, middle, and right placement for up and down
                    // up
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x, y + 1));
                    possibleAffectedTiles.Add(new Vector2Int(x + 1, y + 1));
                    // right
                    possibleAffectedTiles.Add(new Vector2Int(x + 1, y));
                    // down
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y - 1));
                    possibleAffectedTiles.Add(new Vector2Int(x, y - 1));
                    possibleAffectedTiles.Add(new Vector2Int(x + 1, y - 1));
                    // left
                    possibleAffectedTiles.Add(new Vector2Int(x - 1, y));
                    break;
                default:
                    return affectedTiles;
            }
            if (possibleAffectedTiles.Count == 0)
            {
                continue;
            }
            foreach (Vector2Int location in possibleAffectedTiles)
            {
                if (allowableTiles.Contains(location))
                {
                    allowableTiles.Remove(location);
                    affectedTiles.Add(location);
                }
                // if this tile isn't able to be affected, nothing happens
            }
            // delete all of the stuff we tried placing to avoid duplication on next loop
            possibleAffectedTiles.Clear();
        }

        return affectedTiles;
    }

    /// <summary>
    /// calculates the spreading of the tiles and assigns new lifetimes to them
    /// pass in the tile itself, the current lifetime on the tile, and the available tiles to spread to
    /// also needs the shape and spread direction
    /// </summary>
    public (List<Vector2Int>, List<int>) DoTileAttackSpread(
        Vector2Int tile,
        int currentLifetime,
        List<Vector2Int> availableTiles
    )
    {
        List<Vector2Int> newTiles = new List<Vector2Int>();
        List<int> newTilesLifetime = new List<int>();

        return (newTiles, newTilesLifetime);
    }
}
