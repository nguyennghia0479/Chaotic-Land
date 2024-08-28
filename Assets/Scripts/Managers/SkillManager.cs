using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private SwordSkill swordSkill;
    private DashSkill dashSkill;
    private ParrySkill parrySkill;
    private CrystalSkill crystalSkill;
    private CloneSkill cloneSkill;
    private UltimateSkill ultimateSkill;
    private FireSpinSkill fireSpinSkill;
    private IceDrillSkill iceDrillSkill;

    private void Start()
    {
        swordSkill = GetComponent<SwordSkill>();
        dashSkill = GetComponent<DashSkill>();
        parrySkill = GetComponent<ParrySkill>();
        crystalSkill = GetComponent<CrystalSkill>();
        cloneSkill = GetComponent<CloneSkill>();
        ultimateSkill = GetComponent<UltimateSkill>();
        fireSpinSkill = GetComponent<FireSpinSkill>();
        iceDrillSkill = GetComponent<IceDrillSkill>();
    }

    public SwordSkill SwordSkill { get { return swordSkill; } }
    public DashSkill DashSkill { get { return dashSkill; } }
    public ParrySkill ParrySkill { get { return parrySkill; } }
    public CrystalSkill CrystalSkill { get { return crystalSkill; } }
    public CloneSkill CloneSkill { get { return cloneSkill; } }
    public UltimateSkill UltimateSkill { get { return ultimateSkill; } }
    public FireSpinSkill FireSpinSkill { get { return fireSpinSkill; } }
    public IceDrillSkill IceDrillSkill { get { return iceDrillSkill; } }
}
