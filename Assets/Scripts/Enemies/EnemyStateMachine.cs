using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine
{
    private EnemyState currentState;

    public void InitializedState(EnemyState _state)
    {
        currentState = _state;
        currentState.Enter();
    }

    public void Changestate(EnemyState _state)
    {
        currentState.Exit();
        currentState = _state;
        currentState.Enter();
    }

    public EnemyState CurrentState
    {
        get { return currentState; }
    }
}
