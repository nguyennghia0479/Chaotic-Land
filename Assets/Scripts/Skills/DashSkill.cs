using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashSkill : Skill
{
    [SerializeField] private int dashSpeed = 25;
    [SerializeField] private float dashDuration = .2f;

    /// <summary>
    /// Handles to create clone on starting dash.
    /// </summary>
    public void CreateCloneOnDash()
    {
        SkillManager.Instance.CloneSkill.CreateClone(player.transform, Vector2.zero);
    }

    public int DashSpeed
    {
        get { return dashSpeed; }
    }

    public float DashDuration
    {
        get { return dashDuration; }
    }
}
