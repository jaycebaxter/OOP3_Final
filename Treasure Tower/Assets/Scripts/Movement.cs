using UnityEngine;

public class Movement
{
    [SerializeField]
    private int remainingMovement;

    [SerializeField]
    private int movesThisTurn;

    [SerializeField]
    private int allowedMoves;

    // Passes movement from character class in as movementAmount
    public void SetAllowedMoves(int movementAmount) {
        allowedMoves = movementAmount;
        remainingMovement = movementAmount;
        movesThisTurn = 0;
    }
}