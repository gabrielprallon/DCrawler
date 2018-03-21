using FeatherSword.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseWeapon : MonoBehaviour {

    [SerializeField]
    private float m_WeaponDamage = 0f;
    [SerializeField]
    private float m_WeaponExtraDamageWhenAttacking = 1f;
    [SerializeField]
    private Vector2 m_PushForce = new Vector2(2f,20f);
    private AIController m_AIController;
	// Use this for initialization
	void Start () {
        m_AIController = gameObject.GetComponentInParent<AIController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D other)
    {
        HealthSystem hero = other.gameObject.GetComponent<HealthSystem>();
        
        if (hero)
        {
            PlayerController heroPc = hero.gameObject.GetComponent<PlayerController>();
            Rigidbody2D heroRB = hero.gameObject.GetComponent<Rigidbody2D>();
            if (!IsNotGettingDamage(heroPc))
            {
                hero.Damage(m_AIController?(m_AIController.IsInAnimationTag("Attack")?m_WeaponExtraDamageWhenAttacking:m_WeaponDamage):m_WeaponDamage);
                
                heroRB.AddForce(new Vector2(Mathf.Sign(hero.transform.position.x-transform.position.x) * m_PushForce.x, m_PushForce.y), ForceMode2D.Impulse);
            }
        }
    }
    private bool IsNotGettingDamage(PlayerController heroPc)
    {
        return heroPc.IsInAnimationTag("Damage")
            ||heroPc.IsInAnimationTag("Death");
    }
}
