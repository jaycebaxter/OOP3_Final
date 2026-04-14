using UnityEngine;

public class Movement : MonoBehaviour
{
    private int remainingMovement;

    private bool canMove = false;

    [SerializeField]
    private Game game;


// Deprecated
    // public bool TryMove() {
    //     if (remainingMovement <=0) {
    //         return false;
    //     }

    //     remainingMovement -= 1;
    //     return true;
    // }

    public int GetRemainingMovement()
    {
        return remainingMovement;
    }

    public bool HasMovesLeft() {
        return remainingMovement > 0;
    }

    void Update() {
        if (!canMove || remainingMovement <= 0)
            return;

        Vector2Int direction = Vector2Int.zero;


        // Lets player move in wasd directions
        if (Input.GetKeyDown(KeyCode.W)) direction = new Vector2Int(0, 1);
        else if (Input.GetKeyDown(KeyCode.S)) direction = new Vector2Int(0, -1);
        else if (Input.GetKeyDown(KeyCode.A)) direction = new Vector2Int(-1, 0);
        else if (Input.GetKeyDown(KeyCode.D)) direction = new Vector2Int(1, 0);
        else return;

        Vector2Int currentTile = game.GetPlayerTile();
        Vector2Int targetTile = currentTile + direction;


        // Only lets player move if the tile is in walkable list
        if (!game.IsTileWalkable(targetTile))
            return;

        // Reduces move allowance by 1, moves player
        remainingMovement -= 1;
        game.MovePlayerTo(targetTile);

        // Snaps player position to middle of tile because unity handles it weirdly
        transform.position = new Vector3(targetTile.x + 0.5f, targetTile.y + 0.5f, 0f);

        if (remainingMovement <= 0)
        {
            canMove = false;
            game.OnMovementFinished();
        }
    }

    // Called at the beginning of every player turn
    public void StartTurn(int movementAllowance)
    {
        remainingMovement = movementAllowance;
        canMove = true;
    }

    // Called by Game if the turn ends early while still has moves available
    public void EndTurn()
    {
        canMove = false;
        remainingMovement = 0;
    }
}