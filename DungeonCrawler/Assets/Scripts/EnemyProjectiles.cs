using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 2f;
    [SerializeField]
    private Rigidbody2D m_RB;
    private Vector2 m_Direction;
    [SerializeField]
    private float m_ProjectileDamage = 0.2f;
    private void Start()
    {
        m_Direction = Vector2.right;
    }
    private void FixedUpdate()
    {
        m_RB.velocity = m_Direction * m_Speed;
    }

    public void Blocked()
    {
        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        HealthSystem hero = other.gameObject.GetComponent<HealthSystem>();

        if (hero)
        {
            hero.Damage(m_ProjectileDamage);
            Destroy(gameObject);
        }

    }
}    

