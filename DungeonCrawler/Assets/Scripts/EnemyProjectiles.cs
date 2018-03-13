using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectiles : MonoBehaviour
{
    [SerializeField]
    private float m_Speed = 2f;
    [SerializeField]
    private Rigidbody2D m_RB;
    [SerializeField]
    AIPlayerDetection m_PDSys;
    private Vector3 m_Direction;
    [SerializeField]
    private float m_ProjectileDamage = 0.2f;
    [SerializeField]
    private Vector3 m_Adjustment = Vector3.zero;

    private void Start()
    {
        if (!m_PDSys)
            m_PDSys = GetComponent<AIPlayerDetection>();
        if (m_PDSys)
            GetDirection();
    }
    private void FixedUpdate()
    {

        m_RB.velocity = m_Direction * m_Speed;
    }
    
    private void GetDirection()
    {
        if (m_PDSys.PlayerDistanceOnAxis().y - transform.position.y > m_Adjustment.y)
            m_Direction.y = -1f;
        else
        {
            if (m_PDSys.PlayerDistanceOnAxis().y - transform.position.y < -m_Adjustment.y)
                m_Direction.y = 1f;
            else
                m_Direction.y = 0f;
        }
        if (m_PDSys.PlayerDistanceOnAxis().x - transform.position.x > m_Adjustment.x)
            m_Direction.x = -1f;
        else
        {
            if (m_PDSys.PlayerDistanceOnAxis().x - transform.position.x < -m_Adjustment.x)
                m_Direction.x = 1f;
            else
                m_Direction.x = 0;
        }
        m_Direction.z = 0f;
    }


    public void Blocked()
    {
        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HealthSystem hero = collision.gameObject.GetComponent<HealthSystem>();

        if (hero)
        {
            hero.Damage(m_ProjectileDamage);
            Destroy(gameObject);
        }
    }
}    

