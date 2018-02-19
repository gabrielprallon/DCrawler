using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthSystem : MonoBehaviour {

    [SerializeField]
    private float m_MaxHealth = 1;
    [SerializeField]
    private float m_CurHealth;

    [SerializeField]
    private PolygonCollider2D m_BodyCollider;
	// Use this for initialization
	void Start () {

        m_CurHealth = m_MaxHealth;
        

		
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
    }

    private void Die()
    {
        Destroy(gameObject);
    }

   


}
