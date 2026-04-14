using System.Collections.Generic;
using UnityEngine;

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

    [SerializeField]
    private GameObject bossObject;

    private Boss boss;

    private bool hasAttackedThisTurn = false;

    void Start()
    {
        movement = playerObject.GetComponent<Movement>();
        boss = bossObject.GetComponent<Boss>();
        turnManager = new TurnManager();

        boss.Init(turnManager);

        // spawns player at start tile
        Vector2Int startTile = room.GetPlayerLocation();
        SnapPlayerToTile(startTile);

        StartPlayerTurn();
    }

    void Update()
    {
        if (turnManager == null)
            return;
        if (!turnManager.IsPlayerTurn())
            return;
        if (hasAttackedThisTurn)
            return;
        if (!turnManager.IsPlayerTurn())
            return;
        if (hasAttackedThisTurn)
            return;

        int attackIndex = -1;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            attackIndex = 0;
        else if (Input.GetKeyDown(KeyCode.Alpha2))
            attackIndex = 1;
        else if (Input.GetKeyDown(KeyCode.Alpha3))
            attackIndex = 2;
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            attackIndex = 3;

        if (attackIndex == -1)
            return;

        Attack[] attacks = playerCharacter.GetAttacks();
        if (attackIndex >= attacks.Length)
        {
            Debug.Log($"No attack in slot {attackIndex + 1}.");
            return;
        }

        Attack selectedAttack = attacks[attackIndex];
        Vector2Int playerTile = room.GetPlayerLocation();
        Vector2Int bossTile = boss.GetGridPosition();

        if (!selectedAttack.IsInRange(playerTile, bossTile))
        {
            Debug.Log($"{selectedAttack.GetName()} not in range.");
            return;
        }

        int baseDamage = selectedAttack.GetDamage();
        float variance = Random.Range(0.8f, 1.2f);
        int rolledDamage = Mathf.RoundToInt(baseDamage * variance);

        Debug.Log($"Player used {selectedAttack.GetName()} for {rolledDamage} base damage.");
        boss.TakeDamage(rolledDamage);

        hasAttackedThisTurn = true;

        // end movement and turn after attacking
        movement.EndTurn();
        OnMovementFinished();
    }

    void StartPlayerTurn()
    {
        hasAttackedThisTurn = false;
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
        room.UpdatePlayerLocation(tile.x, tile.y);
        room.RefreshRoom();

        // check for tile damage on every step
        int damage = room.GetDamage(tile);
        if (damage > 0)
        {
            playerCharacter.ChangeHealth(damage);
            Debug.Log(
                $"Stepped on lava! Damage: {damage}. Remaining health: {playerCharacter.GetHealth()}"
            );
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
