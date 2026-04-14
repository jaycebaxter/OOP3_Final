using UnityEngine;

public class Movement
{
    private int remainingMovement;

    // Unnecessary?
    // [SerializeField]
    // private int movesThisTurn;

    private int allowedMoves;

    // Passes movement from character class in as movementAmount
    public void SetAllowedMoves(int movementAmount) {
        allowedMoves = movementAmount;
        remainingMovement = movementAmount;
        movesThisTurn = 0;
    }

    public bool TryMove() {
        if (remainingMovement <=0) {
            return false
        }

        remainingMovement -= 1;
        return true;
    }

    public int GetRemainingMovement()
    {
        return remainingMovement;
    }

    public bool HasMovesLeft() {
        return remainingMovement > 0;
    }
    
}