using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    [SerializeField] protected float cooldown;
    [SerializeField] protected int skillStaminaAmount;

    protected Player player;
    protected float cooldownTimer;
    protected bool canUseSkill;

    protected virtual void Start()
    {
        player = PlayerManager.Instance.Player;
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    /// <summary>
    /// Handles to check and use skill.
    /// </summary>
    /// <returns>True if cooldown is less than zero. False if not.</returns>
    public virtual bool CanUseSkill()
    {
        if (cooldownTimer <= 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            canUseSkill = true;
            return true;
        }

        player.FX.PlayPopupText("On cooldown");
        canUseSkill = false;
        return false;
    }

    public virtual void UseSkill()
    {
      
    }

    public Player Player
    {
        get { return player;}
    }

    public float Cooldown
    {
        get { return cooldown; }
    }

    public int SkillStaminaAmount
    {
        get { return skillStaminaAmount; }
    }
}
