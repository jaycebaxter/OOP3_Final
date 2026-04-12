using System.Collections.Generic;
using UnityEngine;

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
    /// When true, tiles are highlighted as a warning (shown the turn before the attack lands).
    /// Always initialized to true — the first turn is always a prediction.
    /// </summary>
    // DELETE - handled by individual tiles instead
    private bool showPrediction = true;

    /// <summary>
    /// Tracks whether this attack has already spread once this boss fight.
    /// Spread only happens once — after that the tile set stays fixed.
    /// </summary>
    // DELETE - handled by BossRoom
    private bool hasSpread = false;

    /// <summary>
    /// the shape of the attack (how it spreads/starts)
    /// by default, it has no shape and only exists as a singular tile (static)
    /// acceptable terms: plus, square, circle, row, double row, wave, double wave
    /// </summary>
    private string shape = "static";

    // --- Getters ---

    public int GetTileAttackDuration()
    {
        return this.tileAttackLifetime;
    }

    public int GetInitialTilesAffected()
    {
        return this.initialTilesAffected;
    }

    public bool GetDoesSpread()
    {
        return this.doesSpread;
    }

    // DELETE - all 3 handled elsewhere
    public bool GetShowPrediction()
    {
        return this.showPrediction;
    }

    public void SetShowPrediction(bool value)
    {
        this.showPrediction = value;
    }

    public List<Vector2Int> GetCurrentTiles()
    {
        return this.currentTiles;
    }

    // --- Core Methods ---

    /// <summary>
    /// Reduces tile attack duration by 1 each turn.
    /// Returns true if the attack is still active, false if it should be destroyed.
    /// Called by Game before SpreadAttack each turn.
    /// </summary>
    // DELETE - lifetimes handled by tiles instead
    public bool AvailableTileAttackDuration()
    {
        this.tileAttackLifetime -= 1;
        return this.tileAttackLifetime > 0;
    }

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
    /// Updates and returns the list of affected tiles.
    ///
    /// If showPrediction is true:
    ///   Picks initialTilesAffected random tiles from all playable tiles.
    ///   These are highlighted orange as a warning — no damage yet.
    ///
    /// If showPrediction is false and doesSpread is false:
    ///   Returns the same tile list with no changes.
    ///
    /// If showPrediction is false and doesSpread is true:
    ///   Expands currentTiles outward one step in a square (all 8 neighbors).
    /// </summary>
    /// <param name="playableTiles">All valid playable tiles on the board, to avoid placing on impassable tiles.</param>
    public List<Vector2Int> SpreadAttack(List<Vector2Int> playableTiles)
    {
        if (this.showPrediction)
        {
            this.currentTiles.Clear();

            // all playable tiles are valid candidates — no range restriction
            List<Vector2Int> candidates = new List<Vector2Int>(playableTiles);

            // pick initialTilesAffected at random from the candidates
            // if there aren't enough candidates just use all of them
            int count = Mathf.Min(this.initialTilesAffected, candidates.Count);
            for (int i = 0; i < count; i++)
            {
                int randomIndex = Random.Range(i, candidates.Count);

                // swap so we don't repeat picks (partial Fisher-Yates)
                Vector2Int temp = candidates[i];
                candidates[i] = candidates[randomIndex];
                candidates[randomIndex] = temp;

                this.currentTiles.Add(candidates[i]);
            }

            return this.currentTiles;
        }

        // prediction is done — attack is now landing
        if (!this.doesSpread)
        {
            // no spread, same tiles as last turn
            return this.currentTiles;
        }

        // spread outward one step in a square (all 8 neighbors)
        List<Vector2Int> newTiles = new List<Vector2Int>(this.currentTiles);

        foreach (Vector2Int tile in this.currentTiles)
        {
            for (int dx = -1; dx <= 1; dx++)
            {
                for (int dy = -1; dy <= 1; dy++)
                {
                    if (dx == 0 && dy == 0)
                        continue;

                    Vector2Int neighbor = new Vector2Int(tile.x + dx, tile.y + dy);

                    // only spread to playable tiles we haven't already hit
                    if (playableTiles.Contains(neighbor) && !newTiles.Contains(neighbor))
                    {
                        newTiles.Add(neighbor);
                    }
                }
            }
        }

        this.currentTiles = newTiles;
        return this.currentTiles;
    }
}
