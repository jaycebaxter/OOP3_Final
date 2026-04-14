using UnityEngine;

public class TurnManager
{
    public enum TurnState
    {
        PlayerTurn,
        BossTurn
    }

    public TurnState CurrentTurn { get; private set; }
    private int m_TurnCount;

    // public event Action OnTick;

    public TurnManager()
    {
        m_TurnCount = 1;
        CurrentTurn = TurnState.PlayerTurn;
    }

    public void Tick()
    {
        m_TurnCount += 1;

        if (CurrentTurn == TurnState.PlayerTurn)
        {
            CurrentTurn = TurnState.BossTurn;
        }
        else
        {
            CurrentTurn = TurnState.PlayerTurn;
        }

        UnityEngine.Debug.Log($"Turn {m_TurnCount} — {CurrentTurn}");
    }

    public bool IsPlayerTurn()
    {
        return CurrentTurn == TurnState.PlayerTurn;
    }
}