using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>
{
    private DashSkill dashSkill;
    private SwordSkill swordSkill;

    private void Start()
    {
        dashSkill = GetComponent<DashSkill>();
        swordSkill = GetComponent<SwordSkill>();
    }

    public DashSkill DashSkill { get { return dashSkill; } }
    public SwordSkill SwordSkill { get { return swordSkill; } }
}
