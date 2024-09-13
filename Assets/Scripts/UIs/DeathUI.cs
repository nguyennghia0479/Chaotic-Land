using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathUI : MonoBehaviour
{
    private Animator animator;
    private Player player;
    private const string FADE = "Fade";

    private void Start()
    {
        animator = GetComponent<Animator>();
        player = PlayerManager.Instance.Player;
        if (player != null)
        {
            player.OnDie += Player_OnDie;
        }

        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (player != null)
        {
            player.OnDie -= Player_OnDie;
        }
    }

    /// <summary>
    /// Handles to trigger player's death scene
    /// </summary>
    private void Player_OnDie(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        if (player.IsDead)
        {
            animator.SetTrigger(FADE);
        }
    }

    /// <summary>
    /// Handles to load current scene
    /// </summary>
    private void AnimationFinish()
    {
        LevelManager.Instance.LoadCurrentScene();
    }
}
