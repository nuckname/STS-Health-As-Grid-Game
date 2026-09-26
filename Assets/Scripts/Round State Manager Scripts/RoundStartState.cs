using UnityEngine;

// When the game begins, any setup we need to do, maybe picking the enemy type things like that?
public class RoundStartState : RoundBaseState
{
    public override void EnterState(RoundStateManager roundStateManager)
    {
        Debug.Log("Entering Round Start State");
        // Initialization logic for the round goes here
    }

    public override void ExitState(RoundStateManager roundStateManager)
    {
        
    }

    public override void UpdateState(RoundStateManager roundStateManager)
    {
        // Example transition
        // roundStateManager.SwitchState(roundStateManager.playerTurnState);
    }
}