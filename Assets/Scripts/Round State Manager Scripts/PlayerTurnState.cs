using UnityEngine;

public class PlayerTurnState : RoundBaseState
{
    public override void EnterState(RoundStateManager roundStateManager)
    {
        Debug.Log("Entering Player Turn");
        // Enable player controls, UI, etc.
    }

    public override void ExitState(RoundStateManager roundStateManager)
    {
        // Disable player controls
    }

    public override void UpdateState(RoundStateManager roundStateManager)
    {
        // Example transition
        // if (playerFinishedTurn) 
        //     roundStateManager.SwitchState(roundStateManager.enemyTurnState);
    }
}