using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine
{
    private PlayerState currentState;

    public PlayerState CurrentState
    {
        get
        {
            return currentState;
        }
    }

    public void IntializedState(PlayerState _state)
    {
        currentState = _state;
        currentState.Enter();
    }

    public void ChangeState(PlayerState _state)
    {
        currentState.Exit();
        currentState = _state;
        currentState.Enter();
    }
}
