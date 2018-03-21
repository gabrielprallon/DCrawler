using FeatherSword.Actions;
using FeatherSword.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseWeapon : MonoBehaviour {

    [SerializeField]
    private float m_WeaponDamage = 1f;
    [SerializeField]
    private float m_CriticalDamage = 2f;

    private PlayerBlock m_BlockAction;


    private void Awake()
    {
        m_BlockAction = GetComponentInParent<PlayerBlock>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        HealthSystem enemy = other.gameObject.GetComponent<HealthSystem>();
        if (enemy)
        {
            if (!IsNotGettingDamage(other.gameObject))
            {
                if (enemy.tag == ("EnemyCriticalArea"))
                    enemy.Damage(m_CriticalDamage);
                else
                    enemy.Damage(m_WeaponDamage);
            }
        }

        EnemyProjectiles projectile = other.gameObject.GetComponent<EnemyProjectiles>();
        if (projectile)
        {
            projectile.Blocked();
            if (m_BlockAction) m_BlockAction.ObjectBlocked();
        }
    }
    private bool IsNotGettingDamage(GameObject enemy)
    {
        AIController aic = enemy.GetComponent<AIController>();
        return aic.IsInAnimationTag("Damage");
    }
}
