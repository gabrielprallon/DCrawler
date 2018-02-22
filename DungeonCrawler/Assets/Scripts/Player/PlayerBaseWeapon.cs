using FeatherSword.Actions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBaseWeapon : MonoBehaviour {

    [SerializeField]
    private float m_WeaponDamage = 1f;

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
            enemy.Damage(m_WeaponDamage);
        }

        EnemyProjectiles projectile = other.gameObject.GetComponent<EnemyProjectiles>();
        if (projectile)
        {
            projectile.Blocked();
            if (m_BlockAction) m_BlockAction.ObjectBlocked();
        }
    }
}
