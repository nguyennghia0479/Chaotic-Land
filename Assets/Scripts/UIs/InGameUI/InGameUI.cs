using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    [Header("Player info")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image levelBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private Slider staminaSlider;
    [Header("Skill cooldown info")]
    [SerializeField] private Image swordCooldownImg;
    [SerializeField] private Image crystalCooldownImg;
    [SerializeField] private Image dashCooldownImg;
    [SerializeField] private Image parryCooldownImg;
    [SerializeField] private Image ultimateCooldownImg;

    private SkillManager skillManager;
    private PlayerManager playerManager;
    private Player player;
    private PlayerController playerController;

    private void OnEnable()
    {
        if (playerStats != null)
        {
            playerStats.OnInitHealth += PlayerStats_OnInitHealth;
            playerStats.OnHealthChange += PlayerStats_OnHealthChange;
            playerStats.OnInitStamina += PlayerStats_OnInitStamina;
            playerStats.OnStaminaChange += PlayerStats_OnStaminaChange;
        }
   
        playerManager = PlayerManager.Instance;
        if (playerManager != null)
        {
            if (playerManager != null)
            {
                playerManager.OnUpdateExp += PlayerManager_OnUpdateExp;
            }
        }
    }

    private void OnDestroy()
    {
        if (playerStats != null)
        {
            playerStats.OnInitHealth -= PlayerStats_OnInitHealth;
            playerStats.OnHealthChange -= PlayerStats_OnHealthChange;
            playerStats.OnInitStamina -= PlayerStats_OnInitStamina;
            playerStats.OnStaminaChange -= PlayerStats_OnStaminaChange;
        }

        if (playerManager != null)
        {
            playerManager.OnUpdateExp -= PlayerManager_OnUpdateExp;
        }

        if (playerController != null)
        {
            playerController.OnAimActionEnd -= PlayerController_OnAimActionEnd;
            playerController.OnSpellCastAction -= PlayerController_OnSpellCastAction;
            playerController.OnDashAction -= PlayerController_OnDashAction;
            playerController.OnBlockActionEnd -= PlayerController_OnBlockActionEnd;
            playerController.OnUltimateAction -= PlayerController_OnUltimateAction;
        }

        if (skillManager != null)
        {
            skillManager.CrystalSkill.OnResetSkill -= CrystalSkill_OnResetSkill;
        }
    }

    private void Start()
    {
        skillManager = SkillManager.Instance;
        player = playerManager.Player;
        playerController = player.Controller;

        if (playerController != null)
        {
            playerController.OnAimActionEnd += PlayerController_OnAimActionEnd;
            playerController.OnSpellCastAction += PlayerController_OnSpellCastAction;
            playerController.OnDashAction += PlayerController_OnDashAction;
            playerController.OnBlockActionEnd += PlayerController_OnBlockActionEnd;
            playerController.OnUltimateAction += PlayerController_OnUltimateAction;
        }

        Invoke(nameof(InitCrystalResetSkillEvent), .1f);
    }

    private void Update()
    {
        UpdateImageCooldown(swordCooldownImg, skillManager.SwordSkill.Cooldown, skillManager.SwordSkill.IsRegularSwordUnlocked);
        UpdateImageCooldown(crystalCooldownImg, skillManager.CrystalSkill.Cooldown, skillManager.CrystalSkill.IsSpellCrystalUnlocked);
        UpdateImageCooldown(dashCooldownImg, skillManager.DashSkill.Cooldown, skillManager.DashSkill.IsDashUnlocked);
        UpdateImageCooldown(parryCooldownImg, skillManager.ParrySkill.Cooldown, skillManager.ParrySkill.IsParryUnlocked);
        UpdateImageCooldown(ultimateCooldownImg, skillManager.UltimateSkill.Cooldown, skillManager.UltimateSkill.IsUltimateUnlocked);
    }

    #region Player info
    public void UpdateHealthStat()
    {
        healthSlider.maxValue = playerStats.maxHealth.GetValueWithModify();
    }

    public void UpdateStaminaStat()
    {
        staminaSlider.maxValue = playerStats.maxStamina.GetValueWithModify();
        playerStats.DecreaseStamina(0);
    }

    private void PlayerManager_OnUpdateExp(object sender, System.EventArgs e)
    {
        levelText.text = playerManager.CurrentLevel.ToString();
        levelBar.fillAmount = (float)playerManager.CurrentExp / playerManager.LevelUpThreshold;
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

    private void PlayerStats_OnInitStamina(object sender, System.EventArgs e)
    {
        staminaSlider.maxValue = playerStats.CurrentStamina;
        staminaSlider.value = playerStats.CurrentStamina;
    }

    private void PlayerStats_OnStaminaChange(object sender, System.EventArgs e)
    {
        staminaSlider.value = playerStats.CurrentStamina;
    }
    #endregion

    #region Skill cooldown
    /// <summary>
    /// Handles to set image fill amount.
    /// </summary>
    /// <param name="_image"></param>
    private void SetImageFillAmount(Image _image)
    {
        if (_image.fillAmount <= 0)
        {
            _image.fillAmount = 1;
        }
    }

    /// <summary>
    /// Handles to update image fill amount by time.
    /// </summary>
    /// <param name="_image"></param>
    /// <param name="_cooldown"></param>
    private void UpdateImageCooldown(Image _image, float _cooldown, bool _isSkillUnlocked)
    {
        if (!_isSkillUnlocked) return;

        if (_image != null && _image.fillAmount > 0)
        {
            _image.fillAmount -= 1 / _cooldown * Time.deltaTime;
        }
    }

    private void PlayerController_OnAimActionEnd(object sender, System.EventArgs e)
    {
        SwordSkill swordSkill = skillManager.SwordSkill;
        if (playerStats.CurrentStamina < swordSkill.SkillStaminaAmount || !swordSkill.IsRegularSwordUnlocked || player.Sword != null) return;

        SetImageFillAmount(swordCooldownImg);
    }

    private void PlayerController_OnSpellCastAction(object sender, System.EventArgs e)
    {
        CrystalSkill crystalSkill = skillManager.CrystalSkill;
        if (playerStats.CurrentStamina < crystalSkill.SkillStaminaAmount || !crystalSkill.IsSpellCrystalUnlocked)
            return;

        if (crystalSkill.IsMultipleCrystalsUnlocked && crystalSkill.CrystalLeft.Count > 1)
            return;

        SetImageFillAmount(crystalCooldownImg);
    }

    private void PlayerController_OnDashAction(object sender, System.EventArgs e)
    {
        DashSkill dashSkill = skillManager.DashSkill;
        if (playerStats.CurrentStamina < dashSkill.SkillStaminaAmount || !dashSkill.IsDashUnlocked) return;

        SetImageFillAmount(dashCooldownImg);
    }

    private void PlayerController_OnBlockActionEnd(object sender, System.EventArgs e)
    {
        ParrySkill parrySkill = skillManager.ParrySkill;
        if (playerStats.CurrentStamina < parrySkill.SkillStaminaAmount || !parrySkill.IsParryUnlocked) return;

        SetImageFillAmount(parryCooldownImg);
    }

    private void PlayerController_OnUltimateAction(object sender, System.EventArgs e)
    {
        UltimateSkill ultimateSkill = skillManager.UltimateSkill;
        if (playerStats.CurrentStamina < ultimateSkill.SkillStaminaAmount || !ultimateSkill.IsUltimateUnlocked) return;

        if (ultimateSkill.Type == UltimateType.FireSpin && player.FireSpin != null) return;

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
    #endregion

    #region Getter
    public Image SwordSkillImg
    {
        get { return swordCooldownImg; }
    }

    public Image CrystalSkillImg
    {
        get { return crystalCooldownImg; }
    }

    public Image DashSkillImg
    {
        get { return dashCooldownImg; }
    }

    public Image ParrySkillImg
    {
        get { return parryCooldownImg; }
    }

    public Image UltimateImg
    {
        get { return ultimateCooldownImg; }
    }
    #endregion
}
