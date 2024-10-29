using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InGameUI : MonoBehaviour
{
    #region Variables
    [Header("Player info")]
    [SerializeField] private PlayerStats playerStats;
    [SerializeField] private Image levelBar;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private Slider healthSlider;
    [SerializeField] private RectTransform healthBar;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider staminaSlider;
    [SerializeField] private RectTransform staminaBar;

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
    private HealthBarShrinkUI healthBarShrinkUI;
    private float healthAdd;
    private bool isHealthUpgraded;
    private float healthTargetWidth;
    private bool isStamainaUpgraded;
    private float staminaTargetWidth;
    private readonly float smoothTime = .5f;
    private readonly int staminaMultiplier = 2;
    #endregion

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

        if (healthBar.sizeDelta.x < healthTargetWidth)
        {
            isHealthUpgraded = true;
        }

        if (staminaBar.sizeDelta.x < staminaTargetWidth)
        {
            isStamainaUpgraded = true;
        }
    }

    private void OnDisable()
    {
        isHealthUpgraded = false;
        isStamainaUpgraded = false;
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
        healthBarShrinkUI = GetComponentInChildren<HealthBarShrinkUI>();

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

        UpdateHealthBar();
        UpdateStaminaBar();
        UpdatePlayerHealing();
    }

    #region Player info
    /// <summary>
    /// Handles to update health stat when was upgraded.
    /// </summary>
    public void UpdateHealthStat()
    {
        healthSlider.maxValue = playerStats.maxHealth.GetValueWithModify();
        if (healthBarShrinkUI != null)
        {
            float healthBarAmount = playerStats.CurrentHealth / healthSlider.maxValue;
            healthBarShrinkUI.IncreaseHealth(healthBarAmount);
            healthSlider.value = playerStats.CurrentHealth;
        }

        isHealthUpgraded = true;
        healthTargetWidth = healthSlider.maxValue;
        healthText.text = healthSlider.value.ToString() + "/" + healthSlider.maxValue;
    }

    /// <summary>
    /// Handles to update stamina stat when was upgraded.
    /// </summary>
    public void UpdateStaminaStat()
    {
        staminaSlider.maxValue = playerStats.maxStamina.GetValueWithModify();
        playerStats.DecreaseStamina(0);
        isStamainaUpgraded = true;
        staminaTargetWidth = staminaSlider.maxValue * staminaMultiplier;
    }

    /// <summary>
    /// Handles to update player level UI.
    /// </summary>
    private void PlayerManager_OnUpdateExp(object sender, System.EventArgs e)
    {
        levelText.text = playerManager.CurrentLevel.ToString();
        levelBar.fillAmount = (float)playerManager.CurrentExp / playerManager.LevelUpThreshold;
    }

    /// <summary>
    /// Handles to init health UI.
    /// </summary>
    private void PlayerStats_OnInitHealth(object sender, System.EventArgs e)
    {
        healthSlider.maxValue = playerStats.CurrentHealth;
        healthSlider.value = playerStats.CurrentHealth;
        healthBar.sizeDelta = new Vector2(healthSlider.maxValue, healthBar.sizeDelta.y);
        healthText.text = healthSlider.value.ToString() + "/" + healthSlider.maxValue;
    }

    /// <summary>
    /// Handles to update health UI when it chanaged.
    /// </summary>
    private void PlayerStats_OnHealthChange(object sender, System.EventArgs e)
    {
        if (healthBarShrinkUI == null) return;

        float healthBarAmount = playerStats.CurrentHealth / healthSlider.maxValue;
        if (playerStats.CurrentHealth < healthSlider.value)
        {
            healthBarShrinkUI.DecreaseHealth(healthBarAmount);
            healthSlider.value = playerStats.CurrentHealth;
            healthText.text = healthSlider.value.ToString() + "/" + healthSlider.maxValue;
        }
        else
        {
            healthAdd = playerStats.CurrentHealth - healthSlider.value;
        }
    }

    /// <summary>
    /// Handles to init stamina UI.
    /// </summary>
    private void PlayerStats_OnInitStamina(object sender, System.EventArgs e)
    {
        staminaSlider.maxValue = playerStats.CurrentStamina;
        staminaSlider.value = playerStats.CurrentStamina;
        staminaBar.sizeDelta = new Vector2(staminaSlider.maxValue * staminaMultiplier, staminaBar.sizeDelta.y);
    }

    /// <summary>
    /// Handles to update stamina UI when it chanaged.
    /// </summary>
    private void PlayerStats_OnStaminaChange(object sender, System.EventArgs e)
    {
        staminaSlider.value = playerStats.CurrentStamina;
    }

    /// <summary>
    /// Handles to update player health when healed.
    /// </summary>
    private void UpdatePlayerHealing()
    {
        if (healthBarShrinkUI == null) return;

        if (healthAdd > 0)
        {
            healthSlider.value += 25 * Time.deltaTime;
            healthAdd -= 25 * Time.deltaTime;

            if (Mathf.Round(healthSlider.value) == playerStats.CurrentHealth)
            {
                healthBarShrinkUI.IncreaseHealth(playerStats.CurrentHealth / healthSlider.maxValue);
                healthSlider.value = playerStats.CurrentHealth;
                healthAdd = 0;
            }

            healthText.text = Mathf.Round(healthSlider.value).ToString() + "/" + healthSlider.maxValue;
        }
    }

    /// <summary>
    /// Handles to update health bar width when was upgraded.
    /// </summary>
    private void UpdateHealthBar()
    {
        if (isHealthUpgraded)
        {
            float newWidth = Mathf.Lerp(healthBar.sizeDelta.x, healthTargetWidth, Time.deltaTime / smoothTime);
            healthBar.sizeDelta = new Vector2(newWidth, healthBar.sizeDelta.y);

            if (Mathf.Abs(newWidth - healthTargetWidth) < .01f)
            {
                isHealthUpgraded = false;
                healthBar.sizeDelta = new Vector2(healthTargetWidth, healthBar.sizeDelta.y);
            }
        }
    }

    /// <summary>
    /// Handles to update stamina bar width when was upgraded.
    /// </summary>
    private void UpdateStaminaBar()
    {
        if (isStamainaUpgraded)
        {
            float newWidth = Mathf.Lerp(staminaBar.sizeDelta.x, staminaTargetWidth, Time.deltaTime / smoothTime);
            staminaBar.sizeDelta = new Vector2(newWidth, staminaBar.sizeDelta.y);

            if (Mathf.Abs(newWidth - staminaTargetWidth) < .01f)
            {
                isStamainaUpgraded = false;
                staminaBar.sizeDelta = new Vector2(staminaTargetWidth, staminaBar.sizeDelta.y);
            }
        }
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
