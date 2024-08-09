using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrightFireSkillController : MonoBehaviour
{
    [SerializeField] private AilmentType ailementType;

    private Player player;
    private Rigidbody2D rb;
    private readonly float timeToDestroy = 5;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        player = PlayerManager.Instance.Player;
    }

    private void FixedUpdate()
    {
        transform.up = -rb.velocity;
    }

    /// <summary>
    /// Handles to make magic damage on target.
    /// </summary>
    /// <remarks>
    /// If player has equip amulet can execute item effect.
    /// </remarks>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out EnemyStats enemy))
        {
            if (enemy.GetComponent<Enemy>().IsDead) return;

            player.Stats.DoMagicDamage(enemy, ailementType);
            GearSO amuletGear = player.InventoryManager.GetGearByGearType(GearType.Amulet);
            if (amuletGear != null)
            {
                amuletGear.ExecuteItemEffects(enemy.transform);
            }
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Handles to set up bright fire info.
    /// </summary>
    /// <param name="_fireSpinSize">The value to determine size of fire spin.</param>
    public void SetupBrightFire(float _fireSpinSize)
    {
        rb.velocity = new Vector3(Random.Range(-_fireSpinSize, _fireSpinSize), -_fireSpinSize);
        Destroy(gameObject, timeToDestroy);
    }
}
