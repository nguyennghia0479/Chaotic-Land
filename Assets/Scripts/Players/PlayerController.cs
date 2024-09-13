using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public event EventHandler OnJumpAction;
    public event EventHandler OnDashAction;
    public event EventHandler OnAttackAction;
    public event EventHandler OnBlockActionStart;
    public event EventHandler OnBlockActionEnd;
    public event EventHandler OnAimActionStart;
    public event EventHandler OnAimActionEnd;
    public event EventHandler OnUltimateAction;
    public event EventHandler OnSpellCastAction;
    public event EventHandler OnUseFlaskAction;
    public event EventHandler OnTabAction;
    public event EventHandler OnPauseAction;
    public event EventHandler OnInteractAction;

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
            inputControl.Player.Attack.performed += ctx => AttackPerformed();
            inputControl.Player.Block.performed += ctx => BlockPerformend();
            inputControl.Player.Block.canceled += ctx => BlockCanceled();
            inputControl.Player.Aim.performed += ctx => AimPerformed();
            inputControl.Player.Aim.canceled += ctx => AimCanceled();
            inputControl.Player.Ultimate.performed += ctx => UltimatePerformed();
            inputControl.Player.SpellCast.performed += ctx => SpellCastPerformed();
            inputControl.Player.UseFlask.performed += ctx => UseFlaskPerformed();
            inputControl.Player.Tab.performed += ctx => TabPerformed();
            inputControl.Player.Pause.performed += ctx => PausePerformed();
            inputControl.Player.Interact.performed += ctx => InteractPerformed();
        }
    }

    private void OnDestroy()
    {
        if (inputControl != null)
        {
            inputControl.Player.Jump.performed -= ctx => JumpPerformed();
            inputControl.Player.Dash.performed -= ctx => DashPerformed();
            inputControl.Player.Attack.performed -= ctx => AttackPerformed();
            inputControl.Player.Block.performed -= ctx => BlockCanceled();
            inputControl.Player.Block.canceled -= ctx => BlockCanceled();
            inputControl.Player.Aim.performed -= ctx => AimPerformed();
            inputControl.Player.Aim.canceled -= ctx => AimCanceled();
            inputControl.Player.Ultimate.performed -= ctx => UltimatePerformed();
            inputControl.Player.SpellCast.performed -= ctx => SpellCastPerformed();
            inputControl.Player.UseFlask.performed -= ctx => UseFlaskPerformed();
            inputControl.Player.Tab.performed -= ctx => TabPerformed();
            inputControl.Player.Pause.performed -= ctx => PausePerformed();
            inputControl.Player.Interact.performed -= ctx => InteractPerformed();
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

    #region Action events
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

    /// <summary>
    /// Handles the attack when performed by the player.
    /// </summary>
    private void AttackPerformed()
    {
        OnAttackAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles the block when performed by the player.
    /// </summary>
    private void BlockPerformend()
    {
        OnBlockActionStart?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles to stop block when canceled by the player.
    /// </summary>
    private void BlockCanceled()
    {
        OnBlockActionEnd?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles aim action when performed by the playr.
    /// </summary>
    private void AimPerformed()
    {
        OnAimActionStart?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles to stop aim when canceled by the player.
    /// </summary>
    private void AimCanceled()
    {
        OnAimActionEnd?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles ultimate action when performed by the player.
    /// </summary>
    private void UltimatePerformed()
    {
        OnUltimateAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles spell cast action when performed by the player.
    /// </summary>
    private void SpellCastPerformed()
    {
        OnSpellCastAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles use flask action when performed by the player.
    /// </summary>
    private void UseFlaskPerformed()
    {
        OnUseFlaskAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles use tab menu when performed by the player.
    /// </summary>
    private void TabPerformed()
    {
        OnTabAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles to pause game when performed by the player.
    /// </summary>
    private void PausePerformed()
    {
        OnPauseAction?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    /// Handles interact object when performed by the player.
    /// </summary>
    private void InteractPerformed()
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
    #endregion
}
