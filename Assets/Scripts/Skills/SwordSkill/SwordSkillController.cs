using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillController : MonoBehaviour
{
    #region Variables
    private Rigidbody2D rb;
    private CapsuleCollider2D capsuleCollider;
    private Animator animator;
    private TrailRenderer trailRenderer;
    private Player player;
    private bool canMoving;
    private bool isRecalling;
    private float recallSpeed;
    private float swordAliveTime;
    private float swordAliveTimer;
    private bool isImmobilizedUnlocked;
    private bool isVulnerableUnlocked;
    private float spinSwordTimer;
    private readonly float spinSwordTimerMax = 2;
    private AudioSource spinSwordAudio;

    [Header("Bounce sword info")]
    private bool isBounceSword;
    private List<Transform> enemyTargets;
    private int enemyIndex;
    private int bounceAmount;
    private float bounceRadius;
    private float bounceSpeed;

    [Header("Pierce sword info")]
    private int pierceAmount;

    [Header("Spin sword info")]
    private bool isSpinSword;
    private bool isSpinMoving;
    private float spinDir;
    private float spinTimer;
    private float spinHitTimer;
    private float spinDuration;
    private float spinMaxDistance;
    private float spinHitCooldown;
    private float spinHitRadius;
    private float spinHitSpeed;
    private float immobilizedDuration;
    private float vulnerableDuration;
    private float vulnerableRate;

    private const string ROTATE = "Rotate";
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        animator = GetComponentInChildren<Animator>();
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        trailRenderer.enabled = false;
    }

    private void Update()
    {
        swordAliveTimer -= Time.deltaTime;
        if (swordAliveTimer < 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (canMoving)
        {
            transform.right = rb.velocity;
        }

        HandleRecallSword();
        BounceSwordSkill();
        SpinSwordSkill();
    }

    /// <summary>
    /// Handles to trigger of sword.
    /// </summary>
    /// <param name="collision">The value to determine object is triggered</param>
    /// <remarks>
    /// If collision is enemy will set knock back and set enemy target for bounce sword skill if sword type is Bounce.<br></br>
    /// Will call HitCollision method.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        bool hitEnemy = false;
        if (collision.TryGetComponent(out Enemy enemy))
        {
            hitEnemy = true;
            SetTargetForBounceSwordSkill();

            if (!isSpinSword)
            {
                if (enemy.IsDead) return;

                SwordSkillDamage(enemy);
            }
        }

        HitCollision(collision);
        PlayHitTargetSound(hitEnemy);
    }

    #region Setup sword
    /// <summary>
    /// Handles to setup base sword info.
    /// </summary>
    /// <param name="_swordSkill">The value that store info from sword skill.</param>
    public void SetupSword(SwordSkill _swordSkill)
    {
        canMoving = true;
        rb.velocity = _swordSkill.FinalDir;
        rb.gravityScale = _swordSkill.SwordGravity;
        player = _swordSkill.Player;
        recallSpeed = _swordSkill.RecallSpeed;
        swordAliveTime = _swordSkill.SwordAliveTime;
        swordAliveTimer = swordAliveTime;
        isImmobilizedUnlocked = _swordSkill.IsImmobilziedUnlocked;
        immobilizedDuration = _swordSkill.ImmobilizedDuration;
        isVulnerableUnlocked = _swordSkill.IsVulnerableUnlocked;
        vulnerableDuration = _swordSkill.VulnerableDuration;
        vulnerableRate = _swordSkill.VulnerableRate;

        if (isBounceSword || isSpinSword)
        {
            PlaySwordAnimator(true);
            trailRenderer.enabled = true;
        }

        if (isSpinSword)
        {
            spinDir = Mathf.Clamp(rb.velocity.x, -1, 1);
        }
    }

    /// <summary>
    /// Handles to set up for bounce sword skill.
    /// </summary>
    /// <param name="_bounceAmount">The value to determine number of bounces.</param>
    /// <param name="_bounceRadius">The value to determine radius bonuce.</param>
    /// <param name="_bounceSpeed">The value to determine speed of bounce</param>
    public void SetupBounceSword(int _bounceAmount, float _bounceRadius, float _bounceSpeed)
    {
        isBounceSword = true;
        enemyTargets = new();
        enemyIndex = 0;
        bounceAmount = _bounceAmount;
        bounceRadius = _bounceRadius;
        bounceSpeed = _bounceSpeed;
    }

    /// <summary>
    /// Handles to set up for pierce sword skill.
    /// </summary>
    /// <param name="_pierceAmout">The value to determine number of pierce.</param>
    public void SetupPierceSword(int _pierceAmout)
    {
        pierceAmount = _pierceAmout;
    }

    /// <summary>
    /// Handles to set up for spin sword skill.
    /// </summary>
    /// <param name="_spinTime">The value to determine time of spin</param>
    /// <param name="_spinMaxDistance">The value to determine max distance can move.</param>
    /// <param name="_spinHitCooldown">The value to determine time to hit enemy.</param>
    /// <param name="_spinHitSpeed">The value to determine speed of spin after hit or execeed max distance.</param>
    /// <param name="_spinHitRadius">The value to determine radius enemy can be hit.</param>
    public void SetupSpinSword(float _spinTime, float _spinMaxDistance, float _spinHitCooldown, float _spinHitSpeed, float _spinHitRadius)
    {
        isSpinSword = true;
        isSpinMoving = true;
        spinDuration = _spinTime;
        spinMaxDistance = _spinMaxDistance;
        spinHitCooldown = _spinHitCooldown;
        spinHitSpeed = _spinHitSpeed;
        spinHitRadius = _spinHitRadius;
    }
    #endregion

    #region Recall sword
    /// <summary>
    /// Handles to set up sword info when recalled.
    /// </summary>
    public void RecallSword()
    {
        PlaySwordAnimator(true);
        trailRenderer.enabled = true;
        isRecalling = true;
        canMoving = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        capsuleCollider.enabled = false;
        transform.parent = null;
    }

    /// <summary>
    /// Handles to recall sword. After close enough to player will destroy sword game object.
    /// </summary>
    private void HandleRecallSword()
    {
        if (isRecalling)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, recallSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < 1)
            {
                StopPlaySound();
                isRecalling = false;
                player.CatchSword();
                return;
            }

            PlaySpinSwordSound();
        }
    }
    #endregion

    #region Sword skill
    /// <summary>
    /// Handles to perform bounce sword skill.
    /// </summary>
    /// <remarks>
    /// It will hit enemy from enemy targets list.
    /// </remarks>
    private void BounceSwordSkill()
    {
        if (isBounceSword && enemyTargets.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTargets[enemyIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTargets[enemyIndex].position) < .1f)
            {
                if (enemyTargets[enemyIndex].TryGetComponent(out Enemy enemy))
                {
                    SwordSkillDamage(enemy);
                    if (enemy.IsDead)
                    {
                        enemyTargets.Remove(enemyTargets[enemyIndex]);
                    }
                }
                enemyIndex++;
                bounceAmount--;
            }

            if (bounceAmount <= 0 || enemyTargets.Count <= 0)
            {
                isBounceSword = false;
                RecallSword();
            }

            if (enemyIndex > enemyTargets.Count - 1)
            {
                enemyIndex = 0;
            }
        }
    }

    /// <summary>
    /// Handles to add enemy for bounce sword skill.
    /// </summary>
    private void SetTargetForBounceSwordSkill()
    {
        if (isBounceSword && enemyTargets.Count <= 0)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, bounceRadius);
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Enemy enemy))
                {
                    if (enemy.IsDead) return;

                    enemyTargets.Add(enemy.transform);
                }
            }
        }
    }

    /// <summary>
    /// Handles to perform spin sword skill.
    /// </summary>
    /// <remarks>
    /// If spin hit collision or exceed spin max distance will stop moving and change to spin hit.
    /// In spin duration, it will hit enemy by spin hit cooldown time.
    /// </remarks>
    private void SpinSwordSkill()
    {
        if (isSpinSword)
        {
            if (isSpinMoving && Vector2.Distance(player.transform.position, transform.position) > spinMaxDistance)
            {
                StopSpinSwordMove();
            }

            if (!isSpinMoving && spinTimer > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDir, transform.position.y), spinHitSpeed * Time.deltaTime);

                spinTimer -= Time.deltaTime;
                if (spinTimer < 0)
                {
                    RecallSword();
                    isSpinSword = false;
                }

                spinHitTimer -= Time.deltaTime;
                if (spinHitTimer < 0)
                {
                    spinHitTimer = spinHitCooldown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, spinHitRadius);
                    foreach (Collider2D collider in colliders)
                    {
                        if (collider.TryGetComponent(out Enemy enemy))
                        {
                            if (enemy.IsDead) return;

                            SwordSkillDamage(enemy);
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// Handles to stop spin sword move.
    /// </summary>
    private void StopSpinSwordMove()
    {
        if (isSpinMoving)
        {
            spinTimer = spinDuration;
        }

        isSpinMoving = false;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }
    #endregion

    /// <summary>
    /// Handles to set up sword info after hit collision.
    /// </summary>
    /// <param name="_collision">The value to determine object is triggered</param>
    private void HitCollision(Collider2D _collision)
    {
        swordAliveTimer = swordAliveTime;

        if (pierceAmount > 0 && _collision.TryGetComponent(out Enemy _))
        {
            pierceAmount--;
            return;
        }

        if (isSpinSword)
        {
            StopSpinSwordMove();
            return;
        }

        canMoving = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        capsuleCollider.enabled = false;

        if (isBounceSword && enemyTargets.Count > 0) return;

        transform.parent = _collision.transform;
        PlaySwordAnimator(false);
    }

    /// <summary>
    /// Handles to set animator of sword.
    /// </summary>
    /// <param name="_isPlay">The value to determine animator is play or not.</param>
    private void PlaySwordAnimator(bool _isPlay)
    {
        if (animator != null)
        {
            animator.SetBool(ROTATE, _isPlay);
        }
    }

    /// <summary>
    /// Handles to do physics damage by sword.
    /// </summary>
    /// <param name="_enemy"></param>
    private void SwordSkillDamage(Enemy _enemy)
    {
        if (isImmobilizedUnlocked)
        {
            _enemy.ImmobilizedEffect(immobilizedDuration);
        }

        if (isVulnerableUnlocked)
        {
            _enemy.Stats.MakeVulnerable(vulnerableDuration, vulnerableRate);
        }

        player.Stats.DoPhysicalDamage(_enemy.Stats);
    }

    #region Play sound
    /// <summary>
    /// Handles to play hit target sound.
    /// </summary>
    /// <param name="_isHitEnemy"></param>
    private void PlayHitTargetSound(bool _isHitEnemy)
    {
        if (!_isHitEnemy && SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayAttackSound(transform.position);
        }
    }

    /// <summary>
    /// Handles to play spin sword sound.
    /// </summary>
    private void PlaySpinSwordSound()
    {
        if (SoundManager.Instance != null)
        {
            spinSwordTimer -= Time.deltaTime;
            if (spinSwordTimer < 0)
            {
                spinSwordTimer = spinSwordTimerMax;
                SoundManager.Instance.PlaySpinSwordSound(transform.position);
                spinSwordAudio = SoundManager.Instance.AudioSource;
            } 
        }
    }

    /// <summary>
    /// Handles to stop play sound.
    /// </summary>
    private void StopPlaySound()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopPlaySound(spinSwordAudio);
        }
    }
    #endregion
}
