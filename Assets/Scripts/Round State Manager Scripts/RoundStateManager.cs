using UnityEngine;

public class RoundStateManager : MonoBehaviour
{
    public RoundBaseState currentState;
    public RoundStartState startState = new RoundStartState();
    public PlayerTurnState playerTurnState = new PlayerTurnState();
    public EnemyTurnState enemyTurnState = new EnemyTurnState();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentState = startState;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        if (currentState != null)
        {
            currentState.UpdateState(this);
        }
    }

    public void SwitchState(RoundBaseState state)
    {
        currentState.ExitState(this);
        currentState = state;
        currentState.EnterState(this);
    }
}