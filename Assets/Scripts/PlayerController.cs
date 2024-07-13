using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnJumpAction;
    public event EventHandler OnDashAction;
    public event EventHandler OnAttackAction;

    private InputControl inputControl;

    private void Awake()
    {
        inputControl = new InputControl();
    }

    private void OnEnable()
    {
        if (inputControl != null)
        {
            inputControl.Player.Enable();
            inputControl.Player.Jump.performed += ctx => JumpPerformed();
            inputControl.Player.Dash.performed += ctx => DashPerformed();
            inputControl.Player.Attack.performed += ctx => AttackPerform();
        }
    }

    private void OnDestroy()
    {
        if (inputControl != null)
        {
            inputControl.Player.Jump.performed -= ctx => JumpPerformed();
            inputControl.Player.Dash.performed -= ctx => DashPerformed();
            inputControl.Player.Attack.performed -= ctx => AttackPerform();
            inputControl.Dispose();
        }
    }

    /// <summary>
    /// Returns the normalized direction vector based on player input.
    /// </summary>
    /// <returns>Normalized direction vector.</returns>
    public Vector2 GetMoveDirNormalized()
    {
        Vector2 moveDir = inputControl.Player.Move.ReadValue<Vector2>();

        return moveDir.normalized;
    }

    /// <summary>
    /// Handles the jump action when performed by the player.
    /// </summary>
    private void JumpPerformed()
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles the dash action when performed by the player.
    /// </summary>
    private void DashPerformed()
    {
        OnDashAction?.Invoke(this, EventArgs.Empty);
    }

    private void AttackPerform()
    {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }
}
