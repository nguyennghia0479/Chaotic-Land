using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private DashSkill dashSkill;
    private SwordSkill swordSkill;
    private CloneSkill cloneSkill;
    private FireSpinSkill fireSpinSkill;
    private CrystalSkill crystalSkill;

    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        swordSkill = GetComponent<SwordSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        fireSpinSkill = GetComponent<FireSpinSkill>();
        crystalSkill = GetComponent<CrystalSkill>();
    }

    public DashSkill DashSkill { get { return dashSkill; } }
    public SwordSkill SwordSkill { get { return swordSkill; } }
    public CloneSkill CloneSkill { get { return cloneSkill; } }
    public FireSpinSkill FireSpinSkill { get { return fireSpinSkill; } }
    public CrystalSkill CrystalSkill { get { return crystalSkill; } }
}
