using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Subclass of Attack used exclusively by bosses.
/// Handles tile-based attacks that affect areas of the board.
/// Boss tile attacks have no range limit — they can target anywhere on the playable grid.
/// When spreading, always expands outward in a square (all 8 neighbors).
/// </summary>
public class TileAttack : Attack
{
    /// <summary>
    /// How many turns this tile attack remains active on the board.
    /// </summary>
    [SerializeField]
    private int tileAttackDuration;

    /// <summary>
    /// How many tiles are affected when this attack is first placed.
    /// </summary>
    [SerializeField]
    private int initialTilesAffected;

    /// <summary>
    /// Whether this attack spreads to adjacent tiles each turn.
    /// </summary>
    [SerializeField]
    private bool doesSpread;

    /// <summary>
    /// The tiles currently affected by this attack.
    /// Stored as Vector2Int (x, y). Z is always 0 on our board.
    /// </summary>
    private List<Vector2Int> currentTiles = new List<Vector2Int>();

    /// <summary>
    /// When true, tiles are highlighted as a warning (shown the turn before the attack lands).
    /// Always initialized to true — the first turn is always a prediction.
    /// </summary>
    private bool showPrediction = true;

    /// <summary>
    /// Tracks whether this attack has already spread once this boss fight.
    /// Spread only happens once — after that the tile set stays fixed.
    /// </summary>
    private bool hasSpread = false;

    // --- Getters ---

    public int GetTileAttackDuration()
    {
        return this.tileAttackDuration;
    }

    public int GetInitialTilesAffected()
    {
        return this.initialTilesAffected;
    }

    public bool GetDoesSpread()
    {
        return this.doesSpread;
    }

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
    public bool AvailableTileAttackDuration()
    {
        this.tileAttackDuration -= 1;
        return this.tileAttackDuration > 0;
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
                    if (dx == 0 && dy == 0) continue;

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