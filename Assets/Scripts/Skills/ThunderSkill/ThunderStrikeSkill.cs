using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderStrikeSkill : MonoBehaviour
{
    [SerializeField] private GameObject thunderStrikePrefab;
    [SerializeField] private int moveSpeed = 30;
    [SerializeField] private float raduis = 10;

    private Transform target;

    private void Update()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        float moveDelta = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, moveDelta);
        transform.right = transform.position - target.position;

        if (Vector2.Distance(transform.position, target.position) < .1f)
        {
            GameObject newThunderStrike = Instantiate(thunderStrikePrefab, target.position, Quaternion.identity);
            Destroy(newThunderStrike, .12f);
            Destroy(gameObject);
        }
    }

    public void SetupThunder(Transform _target)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, raduis);
        float closetTarget = Mathf.Infinity;
        foreach (Collider2D collider in colliders)
        {
            if (collider.TryGetComponent(out Enemy enemy) && Vector2.Distance(transform.position, collider.transform.position) > 1)
            {
                if (enemy.IsDead) return;

                float distance = Vector2.Distance(transform.position, collider.transform.position);

                if (distance < closetTarget)
                {
                    closetTarget = distance;
                    target = enemy.transform;
                }
            }
        }

        if (target == null)
        {
            target = _target;
        }

        Destroy(gameObject, 2);
    }

}
