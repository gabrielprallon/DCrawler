using FeatherSword.AI;
using FeatherSword.Player;
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

    [SerializeField]
    private GameObject m_DeadBody;

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
            if (!IsDying())
            {
                m_PC.Die();
                if (m_DeadBody)
                    StartCoroutine(SpawnDeadBody());
                else
                    m_PC.Respawn();
            }
        }
    }

    private IEnumerator SpawnDeadBody()
    {
        while (!m_PC.IsInAnimationTag("Dead")) { 
            yield return null;
        }
        GameObject go = Instantiate(m_DeadBody);
        m_DeadBody.transform.position = m_PC.transform.position;
        m_PC.Respawn();
        m_CurHealth = m_MaxHealth;
    }

    private bool IsDying()
    {
        return m_PC.IsInAnimationTag("Death");
    }




}
