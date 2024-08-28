using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum SwordType
{
    Regular, Bounce, Pierce, Spin
}

public class SwordSkill : Skill
{
    #region Variables
    [Header("Sword info")]
    [SerializeField] private SwordSkillDropdown swordSkillDropdown;
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float normalGravity = 2;
    [SerializeField] private float recallSpeed = 12;
    [SerializeField] private float swordAliveTime = 7;
    [SerializeField] private float swordCooldown = 0;
    [SerializeField] private float immobilizedDuration = 2;
    [SerializeField] private float vulnerableDuration = 4;
    [SerializeField] private float vulnerableRate = 1.5f;

    [Header("Bounce sword info")]
    [SerializeField] private int bounceAmount = 3;
    [SerializeField] private float bounceRadius = 5;
    [SerializeField] private float bounceSpeed = 10;
    [SerializeField] private float bounceGravity = 3;
    [SerializeField] private float bounceCooldown = 4;

    [Header("Pierce sword info")]
    [SerializeField] private int pierceAmount = 3;
    [SerializeField] private Vector2 pierceForce;
    [SerializeField] private float pierceGravity = .5f;
    [SerializeField] private float pierceCooldown = 2;

    [Header("Spin sword info")]
    [SerializeField] private float spinDuration = 2;
    [SerializeField] private float spinMaxDistance = 10;
    [SerializeField] private float spinHitCooldown = .5f;
    [SerializeField] private float spinHitSpeed = 1.5f;
    [SerializeField] private float spinHitRadius = 2;
    [SerializeField] private float spinGravity = .5f;
    [SerializeField] private float spinCooldown = 6;

    [Header("Aim sword info")]
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotParent;
    [SerializeField] private int numberOfDots;
    [SerializeField] private float spaceBetweenDot;

    [Header("Collision check info")]
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private TilemapCollider2D[] tilemapColliders;

    [Header("Skills unlock info")]
    [SerializeField] private SkillTreeUI regularSwordSkill;
    [SerializeField] private SkillTreeUI pierceSwordSkill;
    [SerializeField] private SkillTreeUI bounceSwordSkill;
    [SerializeField] private SkillTreeUI spinSwordSkill;
    [SerializeField] private SkillTreeUI immobilizedSkill;
    [SerializeField] private SkillTreeUI vulnerableSkill;

    private GameObject[] dots;
    private Vector2 finalDir;
    private float swordGravity;
    private float dotRadius;
    private SwordType swordType;
    private bool isRegularSwordUnlocked;
    private bool isPierceSwordUnlocked;
    private bool isBounceSwordUnlocked;
    private bool isSpinSwordUnlocked;
    private bool isImmobilizedUnlocked;
    private bool isVulnerableUnlocked;
    #endregion

    #region Skills unlocked
    private void OnEnable()
    {
        if (regularSwordSkill != null)
        {
            regularSwordSkill.OnUnlocked += RegularSwordSkill_OnUnlocked;
        }

        if (pierceSwordSkill != null)
        {
            pierceSwordSkill.OnUnlocked += PierceSwordSkill_OnUnlocked;
        }

        if (bounceSwordSkill != null)
        {
            bounceSwordSkill.OnUnlocked += BounceSwordSkill_OnUnlocked;
        }

        if (spinSwordSkill != null)
        {
            spinSwordSkill.OnUnlocked += SpinSwordSkill_OnUnlocked;
        }

        if (immobilizedSkill != null)
        {
            immobilizedSkill.OnUnlocked += ImmobilizedSkill_OnUnlocked;
        }

        if (vulnerableSkill != null)
        {
            vulnerableSkill.OnUnlocked += VulnerableSkill_OnUnlocked;
        }
    }

    private void OnDestroy()
    {
        if (regularSwordSkill != null)
        {
            regularSwordSkill.OnUnlocked -= RegularSwordSkill_OnUnlocked;
        }

        if (pierceSwordSkill != null)
        {
            pierceSwordSkill.OnUnlocked -= PierceSwordSkill_OnUnlocked;
        }

        if (bounceSwordSkill != null)
        {
            bounceSwordSkill.OnUnlocked -= BounceSwordSkill_OnUnlocked;
        }

        if (spinSwordSkill != null)
        {
            spinSwordSkill.OnUnlocked -= SpinSwordSkill_OnUnlocked;
        }

        if (immobilizedSkill != null)
        {
            immobilizedSkill.OnUnlocked -= ImmobilizedSkill_OnUnlocked;
        }

        if (vulnerableSkill != null)
        {
            vulnerableSkill.OnUnlocked -= VulnerableSkill_OnUnlocked;
        }
    }

    private void RegularSwordSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (regularSwordSkill != null && regularSwordSkill.IsUnlocked)
        {
            isRegularSwordUnlocked = true;
            swordSkillDropdown.AddOption(SwordType.Regular);
            GameManager.Instance.InGameUI.SwordSkillImg.fillAmount = 0;
        }
    }

    private void PierceSwordSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (pierceSwordSkill != null && pierceSwordSkill.IsUnlocked)
        {
            isPierceSwordUnlocked = true;
            swordSkillDropdown.AddOption(SwordType.Pierce);
        }
    }

    private void BounceSwordSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (bounceSwordSkill != null && bounceSwordSkill.IsUnlocked)
        {
            isBounceSwordUnlocked = true;
            swordSkillDropdown.AddOption(SwordType.Bounce);
        }
    }

    private void SpinSwordSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (spinSwordSkill != null && spinSwordSkill.IsUnlocked)
        {
            isSpinSwordUnlocked = true;
            swordSkillDropdown.AddOption(SwordType.Spin);
        }
    }

    private void ImmobilizedSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (immobilizedSkill != null && immobilizedSkill.IsUnlocked)
        {
            isImmobilizedUnlocked = true;
        }
    }

    private void VulnerableSkill_OnUnlocked(object sender, System.EventArgs e)
    {
        if (vulnerableSkill != null && vulnerableSkill.IsUnlocked)
        {
            isVulnerableUnlocked = true;
        }
    }
    #endregion

    protected override void Start()
    {
        base.Start();

        SetupSwordType();
        GenerateDots();
    }

    protected override void Update()
    {
        base.Update();

        if (!canUseSkill || player.Sword != null) return;

        if (player.IsAiming)
        {
            SetupAimToThrow();
        }

        if (player.HasThrown)
        {
            SetupThrowSword();
        }
    }

    public override bool CanUseSkill()
    {
        if (!isRegularSwordUnlocked) return false;

        return base.CanUseSkill();
    }

    #region Setup sword
    /// <summary>
    /// Handles create a sword and sword skill base on sword type.
    /// </summary>
    public void ThrowSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, Quaternion.identity);
        SetSwordControllerType(newSword);
        newSword.GetComponent<SwordSkillController>().SetupSword(this);
        player.AssignSword(newSword);
    }

    public void UpdateSwordSkill(SwordType _swordType)
    {
        swordType = _swordType;
        SetupSwordType();
        cooldownTimer = cooldown;
        GameManager.Instance.InGameUI.SwordSkillImg.fillAmount = 1;
    }

    /// <summary>
    /// Handle set sword info base on sword type.
    /// </summary>
    private void SetupSwordType()
    {
        if (swordType == SwordType.Regular)
        {
            swordGravity = normalGravity;
            cooldown = swordCooldown;
        }
        else if (swordType == SwordType.Bounce)
        {
            swordGravity = bounceGravity;
            cooldown = bounceCooldown;
        }
        else if (swordType == SwordType.Pierce)
        {
            swordGravity = pierceGravity;
            cooldown = pierceCooldown;
            launchForce = pierceForce;
        }
        else if (swordType == SwordType.Spin)
        {
            swordGravity = spinGravity;
            cooldown = spinCooldown;
        }
    }

    /// <summary>
    /// Handles set sword skill base on sword type.
    /// </summary>
    /// <param name="_newSword">A sword has been created.</param>
    private void SetSwordControllerType(GameObject _newSword)
    {
        SwordSkillController swordSkillController = _newSword.GetComponent<SwordSkillController>();

        if (swordType == SwordType.Pierce && isPierceSwordUnlocked)
        {
            swordSkillController.SetupPierceSword(pierceAmount);
        }
        else if (swordType == SwordType.Bounce && isBounceSwordUnlocked)
        {
            swordSkillController.SetupBounceSword(bounceAmount, bounceRadius, bounceSpeed);
        }
        else if (swordType == SwordType.Spin && isSpinSwordUnlocked)
        {
            swordSkillController.SetupSpinSword(spinDuration, spinMaxDistance, spinHitCooldown, spinHitSpeed, spinHitRadius);
        }
    }
    #endregion

    #region Aim Dot
    /// <summary>
    /// Handles to set up dots position when aiming.
    /// </summary>
    /// <remarks>
    /// It will disable composite collider in tilemap collider for detect collision.<br></br>
    /// Will hide surplus dots if detected collision.<br></br> 
    /// Also hide surplus dots if sword type is Spin if dot position is greater than spin max distance.
    /// </remarks>
    private void SetupAimToThrow()
    {
        SetTilemapCollider(false);
        for (int i = 0; i < dots.Length; i++)
        {
            Vector2 dotPosition = GetDotPosition(i * spaceBetweenDot);
            dots[i].transform.position = dotPosition;

            if (IsDetectCollision(dotPosition) || (swordType == SwordType.Spin && dots[i].transform.localPosition.x > spinMaxDistance))
            {
                for (int j = i + 1; j < dots.Length; j++)
                {
                    dots[j].SetActive(false);
                }
                break;
            }
            else
            {
                dots[i].SetActive(true);
            }
        }
    }

    /// <summary>
    /// Handles to set up final direction after thrown.
    /// </summary>
    /// <remarks>
    /// Hide dots and set using composite collider to true.
    /// </remarks>
    private void SetupThrowSword()
    {
        SetDotActiveStatus(false);
        Vector2 aimDir = GetAimDirectionNormalized();
        finalDir = new Vector2(aimDir.x * launchForce.x, aimDir.y * launchForce.y);
        cooldownTimer = cooldown;
        player.HasThrown = false;
        SetTilemapCollider(true);
    }

    /// <summary>
    /// Handles to genrerate dots then hide it at start.
    /// </summary>
    private void GenerateDots()
    {
        dots = new GameObject[numberOfDots];
        dotRadius = dotPrefab.transform.localScale.x * 0.5f;

        for (int i = 0; i < numberOfDots; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotParent);
            dots[i].SetActive(false);
        }
    }

    /// <summary>
    /// Handles to get aim direction normalized by mouse position.
    /// </summary>
    /// <returns>Normalized direction vector.</returns>
    private Vector2 GetAimDirectionNormalized()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 aimDir = mousePos - (Vector2)player.transform.position;
        return aimDir.normalized;
    }

    /// <summary>
    /// Handles to get dot position.
    /// </summary>
    /// <param name="_time">The value to determine time when aiming.</param>
    /// <returns>Position of dot.</returns>
    private Vector2 GetDotPosition(float _time)
    {
        Vector2 aimDir = GetAimDirectionNormalized();
        Vector2 launchVelocity = new(launchForce.x * aimDir.x, launchForce.y * aimDir.y);

        Vector2 currentPos = (Vector2)player.transform.position +
            (launchVelocity * _time) +
            (_time * _time) * .5f * (Physics2D.gravity * swordGravity);

        return currentPos;
    }

    /// <summary>
    /// Handles to detect collision like ground, slope and enemies.
    /// </summary>
    /// <param name="_position">Position of dot</param>
    /// <returns>True if detected. False if not</returns>
    private bool IsDetectCollision(Vector2 _position)
    {
        Vector2 aimDir = GetAimDirectionNormalized();
        RaycastHit2D[] hits = Physics2D.CircleCastAll(_position, dotRadius, aimDir, spaceBetweenDot, layerMask);

        return hits.Length > 0;
    }

    /// <summary>
    /// Handles to set active status of dots
    /// </summary>
    /// <param name="_isActive">The value to determine dots will active or not</param>
    private void SetDotActiveStatus(bool _isActive)
    {
        for (int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }

    /// <summary>
    /// Handles to set composite is used in tilemap collider.
    /// </summary>
    /// <param name="_isUsing">The value to determine composite is used or not.</param>
    private void SetTilemapCollider(bool _isUsing)
    {
        foreach (TilemapCollider2D tilemapCollider in tilemapColliders)
        {
            tilemapCollider.usedByComposite = _isUsing;
        }
    }
    #endregion

    #region Getter
    public Vector2 FinalDir
    {
        get { return finalDir; }
    }

    public float SwordGravity
    {
        get { return swordGravity; }
    }

    public float RecallSpeed
    {
        get { return recallSpeed; }
    }

    public float SwordAliveTime
    {
        get { return swordAliveTime; }
    }

    public bool IsRegularSwordUnlocked
    {
        get { return isRegularSwordUnlocked; }
    }

    public bool IsImmobilziedUnlocked
    {
        get { return isImmobilizedUnlocked; }
    }

    public bool IsVulnerableUnlocked
    {
        get { return isVulnerableUnlocked; }
    }

    public float ImmobilizedDuration
    {
        get { return immobilizedDuration; }
    }

    public float VulnerableDuration
    {
        get { return vulnerableDuration; }
    }

    public float VulnerableRate
    {
        get { return vulnerableRate; }
    }
    #endregion
}
