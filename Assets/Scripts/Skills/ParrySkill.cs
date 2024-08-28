using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParrySkill : Skill
{
    [Header("Restore health info")]
    [SerializeField][Range(0f, 1f)] private float restoreHealthRate = .05f;
    [Header("Clone on parry info")]
    [SerializeField] private float cloneOffset = 1.5f;
    [SerializeField] private float cloneDelay = .5f;
    [Header("Skills unlocked info")]
    [SerializeField] private SkillTreeUI parrySkill;
    [SerializeField] private SkillTreeUI restoreHealthSkill;
    [SerializeField] private SkillTreeUI cloneOnParrySkill;

    private bool isParryUnlocked;
    private bool isRestoreHealthUnlocked;
    private bool isCloneOnParryUnlocked;

    #region Skills unlocked
    private void OnEnable()
    {
        if (parrySkill != null)
        {
            parrySkill.OnUnlocked += ParrySkill_OnUnlocked;
        }

        if (restoreHealthSkill != null)
        {
            restoreHealthSkill.OnUnlocked += RestoreHealthSkill_OnUnlocked;
        }

        if (cloneOnParrySkill != null)
        {
            cloneOnParrySkill.OnUnlocked += CloneOnParrySkill_OnUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (parrySkill != null)
        {
            parrySkill.OnUnlocked -= ParrySkill_OnUnlocked;
        }

        if (restoreHealthSkill != null)
        {
            restoreHealthSkill.OnUnlocked -= RestoreHealthSkill_OnUnlocked;
        }

        if (cloneOnParrySkill != null)
        {
            cloneOnParrySkill.OnUnlocked -= CloneOnParrySkill_OnUnlocked;
        }
    }

    private void ParrySkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (parrySkill != null && parrySkill.IsUnlocked)
        {
            isParryUnlocked = true;
            GameManager.Instance.InGameUI.ParrySkillImg.fillAmount = 0;
        }
    }

    private void RestoreHealthSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (restoreHealthSkill != null && restoreHealthSkill.IsUnlocked)
        {
            isRestoreHealthUnlocked = true;
        }
    }

    private void CloneOnParrySkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (cloneOnParrySkill != null && cloneOnParrySkill.IsUnlocked)
        {
            isCloneOnParryUnlocked = true;
        }
    }
    #endregion

    public override bool CanUseSkill()
    {
        if (!isParryUnlocked) return false;

        return base.CanUseSkill();
    }

    public void RestoreHealth()
    {
        if (isRestoreHealthUnlocked)
        {
            PlayerStats playerStats = player.GetComponent<PlayerStats>();
            int restoreHealthAmount = Mathf.RoundToInt(playerStats.maxHealth.GetValueWithModify() * restoreHealthRate);
            playerStats.IncreaseHealth(restoreHealthAmount);
        }
    }

    public void CanCreateClone(Transform _target)
    {
        if (isCloneOnParryUnlocked)
        {
            float offset = cloneOffset * player.FacingDir;
            StartCoroutine(CreateCloneWithDelayRoutine(_target, new Vector2(offset, 0)));
        }
    }

    private IEnumerator CreateCloneWithDelayRoutine(Transform _target, Vector2 _offset)
    {
        yield return new WaitForSeconds(cloneDelay);
        SkillManager.Instance.CloneSkill.CreateClone(_target, _offset);
    }

    public bool IsParryUnlocked
    {
        get { return isParryUnlocked; }
    }
}
