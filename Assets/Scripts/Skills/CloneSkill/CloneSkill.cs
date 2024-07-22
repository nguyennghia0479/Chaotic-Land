using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloneSkill : Skill
{
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration = 1;
    [SerializeField] private float losingSpeed = .5f;
    [SerializeField] private float chanceToMulti = 50;

    /// <summary>
    /// Handles to create clone of the character.
    /// </summary>
    /// <param name="clonePos">The value to determine position of clone.</param>
    /// <param name="offset">The value to determine offset position of clone.</param>
    public void CreateClone(Transform clonePos, Vector2 offset)
    {
        GameObject newClone = Instantiate(clonePrefab);
        newClone.GetComponent<CloneSkillController>().SetupClone(this, clonePos, offset);
    }

    public float CloneDuration
    {
        get { return cloneDuration; }
    }

    public float LosingSpeed
    {
        get { return losingSpeed; }
    }

    public float ChanceToMulti
    {
        get { return chanceToMulti; }
    }
}
