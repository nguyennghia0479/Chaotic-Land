using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Image swordCooldownImg;
    [SerializeField] private Image crystalCooldownImg;
    [SerializeField] private Image dashCooldownImg;
    [SerializeField] private Image ultimateCooldownImg;

    private SkillManager skillManager;
    private PlayerController playerController;

    private void OnEnable()
    {
        if (playerStats != null)
        {
            playerStats.OnInitHealth += PlayerStats_OnInitHealth;
            playerStats.OnHealthChange += PlayerStats_OnHealthChange;
        }
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnInitHealth -= PlayerStats_OnInitHealth;
            playerStats.OnHealthChange -= PlayerStats_OnHealthChange;
        }

        if (playerController != null)
        {
            playerController.OnAimActionEnd -= PlayerController_OnAimActionEnd;
            playerController.OnSpellCastAction -= PlayerController_OnSpellCastAction;
            playerController.OnDashAction -= PlayerController_OnDashAction;
            playerController.OnUltimateAction -= PlayerController_OnUltimateAction;
        }

        if (skillManager != null)
        {
            skillManager.CrystalSkill.OnResetSkill -= CrystalSkill_OnResetSkill;
        }
    }

    private void Start()
    {
        playerController = PlayerManager.Instance.Player.Controller;
        if (playerController != null)
        {
            playerController.OnAimActionEnd += PlayerController_OnAimActionEnd;
            playerController.OnSpellCastAction += PlayerController_OnSpellCastAction;
            playerController.OnDashAction += PlayerController_OnDashAction;
            playerController.OnUltimateAction += PlayerController_OnUltimateAction;
        }

        skillManager = SkillManager.Instance;
        Invoke(nameof(InitCrystalResetSkillEvent), .1f);
    }

    private void Update()
    {
        UpdateImageCooldown(swordCooldownImg, skillManager.SwordSkill.Cooldown);
        UpdateImageCooldown(crystalCooldownImg, skillManager.CrystalSkill.Cooldown);
        UpdateImageCooldown(dashCooldownImg, skillManager.DashSkill.Cooldown);
        UpdateImageCooldown(ultimateCooldownImg, skillManager.FireSpinSkill.Cooldown);
    }

    /// <summary>
    /// Handles to set image fill amount.
    /// </summary>
    /// <param name="image"></param>
    private void SetImageFillAmount(Image image)
    {
        if (image.fillAmount <= 0)
        {
            image.fillAmount = 1;
        }
    }

    /// <summary>
    /// Handles to update image fill amount by time.
    /// </summary>
    /// <param name="image"></param>
    /// <param name="cooldown"></param>
    private void UpdateImageCooldown(Image image, float cooldown)
    {
        if (image != null && image.fillAmount > 0)
        {
            image.fillAmount -= 1 / cooldown * Time.deltaTime;
        }
    }

    private void PlayerStats_OnInitHealth(object sender, System.EventArgs e)
    {
        healthSlider.maxValue = playerStats.CurrentHealth;
        healthSlider.value = playerStats.CurrentHealth;
    }

    private void PlayerStats_OnHealthChange(object sender, System.EventArgs e)
    {
        healthSlider.value = playerStats.CurrentHealth;
    }

    private void PlayerController_OnAimActionEnd(object sender, System.EventArgs e)
    {
        SetImageFillAmount(swordCooldownImg);
    }

    private void PlayerController_OnSpellCastAction(object sender, System.EventArgs e)
    {
       SetImageFillAmount(crystalCooldownImg);
    }

    private void PlayerController_OnDashAction(object sender, System.EventArgs e)
    {
        SetImageFillAmount(dashCooldownImg);
    }

    private void PlayerController_OnUltimateAction(object sender, System.EventArgs e)
    {
        SetImageFillAmount(ultimateCooldownImg);
    }

    private void InitCrystalResetSkillEvent()
    {
        skillManager.CrystalSkill.OnResetSkill += CrystalSkill_OnResetSkill;
    }

    private void CrystalSkill_OnResetSkill(object sender, System.EventArgs e)
    {
        SetImageFillAmount(crystalCooldownImg);
    }
}
