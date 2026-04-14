using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Character playerCharacter;

    [SerializeField]
    private BossRoom room;

    [SerializeField]
    private GameObject playerObject;

    [SerializeField]
    private Movement movement;

    [SerializeField]
    private TurnManager turnManager;

    void Start()
    {
        movement = playerObject.GetComponent<Movement>();
        turnManager = new TurnManager();

        // spawns player at start tile
        Vector2Int startTile = room.GetPlayerLocation();
        SnapPlayerToTile(startTile);

        // debugging ignore
        var walkable = room.GetWalkableTiles();
        Debug.Log($"Walkable tile count: {walkable.Count}");

        StartPlayerTurn();
    }

    void StartPlayerTurn()
    {
        room.RefreshRoom();
        movement.StartTurn(playerCharacter.GetMovementAmt());
    }

    public void OnMovementFinished()
    {
        // // apply tile damage if the player is standing on a dangerous tile
        // Vector2Int currentTile = room.GetPlayerLocation();
        // int damage = room.GetDamage(currentTile);
        // if (damage > 0)
        // {
        //     playerCharacter.ChangeHealth(damage);
        //     Debug.Log($"Tile damage: {damage}. Player health: {playerCharacter.GetHealth()}");
        // }

        // apply room status to player if they're standing on a status tile
        // if (room.TileHasStatus(currentTile) && !playerCharacter.HasStatus())
        // {
        //     // status application will be wired up once the full turn loop is in place
        //     Debug.Log("Player is on a status tile.");
        // }

        turnManager.Tick();
        Debug.Log("Player turn ended. Boss turn would begin here.");

        // boss turn logic goes here later
        // for now, immediately give the player their next turn so movement can be tested
        turnManager.Tick();
        StartPlayerTurn();
    }


    public Vector2Int GetPlayerTile()
    {
        return room.GetPlayerLocation();
    }


    public bool IsTileWalkable(Vector2Int tile)
    {
        return room.GetWalkableTiles().Contains(tile);
    }

    public void MovePlayerTo(Vector2Int tile)
    {
        // remove later
        Debug.Log($"Target tile: {tile}. Walkable list contains it: {room.GetWalkableTiles().Contains(tile)}. Total walkable: {room.GetWalkableTiles().Count}");
        Debug.Log($"Moving to tile {tile}. Is walkable: {room.GetWalkableTiles().Contains(tile)}");

        room.UpdatePlayerLocation(tile.x, tile.y);
        room.RefreshRoom();

        // check for tile damage on every step
        int damage = room.GetDamage(tile);
        if (damage > 0)
        {
            playerCharacter.ChangeHealth(damage);
            Debug.Log($"Stepped on lava! Damage: {damage}. Remaining health: {playerCharacter.GetHealth()}");
        }

        if (room.TileHasStatus(tile) && !playerCharacter.HasStatus())
        {
            Debug.Log("Stepped on a status tile.");
        }
    }

    private void SnapPlayerToTile(Vector2Int tile)
    {
        playerObject.transform.position = new Vector3(tile.x + 0.5f, tile.y + 0.5f, 0f);
    }

}
