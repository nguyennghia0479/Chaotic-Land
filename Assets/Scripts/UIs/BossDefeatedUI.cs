using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BossDefeatedUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI bossName;
    
    private Enemy boss;
    private Animator animator;

    private const string FADE = "Fade";

    private void Start()
    {
        boss = FindObjectOfType<EnemyManager>().FindBoss();
        if (boss != null)
        {
            bossName.text = boss.BossName;
            boss.OnDie += Boss_OnDie;
        }

        animator = GetComponent<Animator>();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        if (boss != null)
        {
            boss.OnDie -= Boss_OnDie;
        }
    }

    private void Boss_OnDie(object sender, System.EventArgs e)
    {
        gameObject.SetActive(true);

        if (boss.IsDead)
        {
            animator.SetTrigger(FADE);
        }
    }

    private void AnimationFinish()
    {
        gameObject.SetActive(false);
    }
}
