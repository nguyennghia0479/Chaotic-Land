using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone skill basic info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1;
    [SerializeField] private float losingSpeed = .5f;
    [SerializeField] private float cloneDamageRate = .4f;
    [Header("Aggressive clone info")]
    [SerializeField] private float aggressiveDamageRate = .6f;
    [Header("Multiple clones info")]
    [SerializeField] private float chanceToMulti = 50;
    [SerializeField] private int maxClones = 5;
    [Header("Skills unlocked info")]
    [SerializeField] private SkillTreeUI aggressiveCloneSkill;
    [SerializeField] private SkillTreeUI multipleCloneSkill;

    private bool isMultipleClonesUnlocked;

    #region Skills unlocked
    private void OnEnable()
    {
        if (aggressiveCloneSkill != null)
        {
            aggressiveCloneSkill.OnUnlocked += AggressiveCloneSkill_OnUnlocked;
        }

        if (multipleCloneSkill != null)
        {
            multipleCloneSkill.OnUnlocked += MultipleClonesSkill_OnUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (aggressiveCloneSkill != null)
        {
            aggressiveCloneSkill.OnUnlocked -= AggressiveCloneSkill_OnUnlocked;
        }

        if (multipleCloneSkill != null)
        {
            multipleCloneSkill.OnUnlocked -= MultipleClonesSkill_OnUnlocked;
        }
    }

    private void AggressiveCloneSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (aggressiveCloneSkill != null && aggressiveCloneSkill.IsUnlocked)
        {
            cloneDamageRate = aggressiveDamageRate;
        }
    }

    private void MultipleClonesSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (multipleCloneSkill != null && multipleCloneSkill.IsUnlocked)
        {
            isMultipleClonesUnlocked = true;
        }
    }
    #endregion

    /// <summary>
    /// Handles to create clone of the character.
    /// </summary>
    /// <param name="clonePos">The value to determine position of clone.</param>
    /// <param name="offset">The value to determine offset position of clone.</param>
    public void CreateClone(Transform clonePos, Vector2 offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(this, clonePos, offset, isMultipleClonesUnlocked);
    }

    public float CloneDuration
    {
        get { return cloneDuration; }
    }

    public float LosingSpeed
    {
        get { return losingSpeed; }
    }

    public float CloneDamageRate
    {
        get { return cloneDamageRate; }
    }

    public float ChanceToMulti
    {
        get { return chanceToMulti; }
    }

    public int MaxClones
    {
        get { return maxClones; }
    }
}
