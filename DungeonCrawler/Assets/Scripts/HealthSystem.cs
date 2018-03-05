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
    private AIController m_AIC;
    [SerializeField]
    private PolygonCollider2D m_BodyCollider;
    [SerializeField]
    private PolygonCollider2D m_CritCollider;
	// Use this for initialization
	void Start () {

        m_CurHealth = m_MaxHealth;
        m_PC = GetComponent<PlayerController>();
        m_AIC = GetComponent<AIController>();
        m_RB = GetComponent<Rigidbody2D>();

    }
	
	// Update is called once per frame
	private void Update () {
		if(m_CurHealth <= 0)
        {
            Die();
        }
	}

    public void Damage(float dmg)
    {
        m_CurHealth -= dmg;
        if(m_PC)
        {
            m_RB.velocity = Vector2.zero;
            if(!IsDying())
                m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.Damage);
        }
        if(m_AIC)
        {
            m_RB.velocity = Vector2.zero;
            m_AIC.SetAnimatorTrigger(AIController.AnimationTriggers.Damage);
        }

    }

    private void Die()
    {
        if(m_PC)
        {
            if(!IsDying())
                m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.Death);    
        }
    }
    private bool IsDying()
    {
        return m_PC.IsInAnimationTag("Die");
    }




}
