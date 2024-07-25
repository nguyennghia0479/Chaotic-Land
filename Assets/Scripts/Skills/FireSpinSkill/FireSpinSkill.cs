using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSpinSkill : Skill
{
    [SerializeField] private GameObject fireSpinPrefab;
    [SerializeField] private float moveTime = 3;
    [SerializeField] private float moveSpeed = 5;
    [SerializeField] private float maxGrowSize = 5;
    [SerializeField] private float scaleSpeed = 2;
    [SerializeField] private float spinHitCooldown = .5f;

    /// <summary>
    /// Handles to create fire spin.
    /// </summary>
    public void CreateFireSpin()
    {
        Vector3 fireSpinPos = new(player.transform.position.x, player.transform.position.y + 2);

        GameObject newFireSpin = Instantiate(fireSpinPrefab, fireSpinPos, Quaternion.identity);
        newFireSpin.GetComponent<FireSpinSkillController>().SetupFireSpin(this);
        player.AssignFireSpin(newFireSpin);
    }

    #region
    public float MoveTime
    {
        get { return moveTime; }
    }

    public float MoveSpeed
    {
        get { return moveSpeed; }
    }

    public float MaxGrowSize
    {
        get { return maxGrowSize; }
    }

    public float ScaleSpeed
    {
        get { return scaleSpeed; }
    }

    public float SpinHitCooldown
    {
        get { return spinHitCooldown; }
    }
    #endregion
}
