using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    [SerializeField]
    private float m_MaxHealth = 1;
    [SerializeField]
    private float m_CurHealth;

    [SerializeField]
    private PlayerController m_PC;
    [SerializeField]
    private Rigidbody2D m_RB;

    [SerializeField]
    private PolygonCollider2D m_BodyCollider;
	// Use this for initialization
	void Start () {

        m_CurHealth = m_MaxHealth;
        if(gameObject.tag == "Body")
        {
            m_PC = GetComponentInParent<PlayerController>();
            m_RB = GetComponentInParent<Rigidbody2D>();
        }

		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(m_CurHealth <= 0)
        {
            Die();
        }
	}

    public void Damage(float dmg)
    {
        m_CurHealth -= dmg;
        if(gameObject.tag == "Body")
        {
            m_RB.velocity = Vector2.zero;
            m_PC.m_Animator[0].SetTrigger("Damage");
            m_PC.m_Animator[1].SetTrigger("Damage");
        }
    }

    private void Die()
    {
        Destroy(gameObject);
        if(gameObject.tag == "Body")
        {
            
        }
    }

   


}
