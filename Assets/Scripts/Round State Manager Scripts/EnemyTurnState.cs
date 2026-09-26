using UnityEngine;

public class EnemyTurnState : RoundBaseState
{
    public override void EnterState(RoundStateManager roundStateManager)
    {
        Debug.Log("Entering Enemy Turn");
        // Trigger AI logic
    }

    public override void ExitState(RoundStateManager roundStateManager)
    {
        
    }

    public override void UpdateState(RoundStateManager roundStateManager)
    {
        // Example transition
        // if (enemyFinishedTurn) 
        //     roundStateManager.SwitchState(roundStateManager.playerTurnState); // Or startState for a new round
    }
}