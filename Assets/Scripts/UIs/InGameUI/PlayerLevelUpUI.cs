using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLevelUpUI : MonoBehaviour
{
    private Animator animator;
    private Player player;

    private const string LEVEL_UP = "LevelUp";

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player = GetComponentInParent<Player>();

        if (player != null )
        {
            player.OnFlipped += Player_OnFlipped;
        }
    }

    private void Player_OnFlipped(object sender, System.EventArgs e)
    {
        transform.Rotate(0, -180, 0);
    }

    /// <summary>
    /// Handles to set animation when player level up.
    /// </summary>
    public void ShowLevelUp()
    {
        animator.SetBool(LEVEL_UP, true);
    }

    /// <summary>
    /// Handles to set default when animation finished.
    /// </summary>
    private void AnimationFinished()
    {
        animator.SetBool(LEVEL_UP, false);
    }
}
