using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnJumpAction;

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
        }
    }

    private void OnDestroy()
    {
        if (inputControl != null)
        {
            inputControl.Player.Jump.performed -= ctx => JumpPerformed();
            inputControl.Dispose();
        }
    }

    /// <summary>
    /// Handles the jump action when performed by the player.
    /// </summary>
    private void JumpPerformed()
    {
        OnJumpAction?.Invoke(this, EventArgs.Empty);
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
}
