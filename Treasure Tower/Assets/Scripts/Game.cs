using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField]
    private Character playerCharacter;

    private Movement movement = new Movement();

    void Start()
    {
        StartTurn();
    }

    void StartTurn()
    {
        int movementAmount = playerCharacter.GetMovementAmt();
        movement.SetAllowedMoves(movementAmount);
    }
}
