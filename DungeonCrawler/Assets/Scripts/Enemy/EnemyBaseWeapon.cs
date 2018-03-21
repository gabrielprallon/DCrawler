using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseWeapon : MonoBehaviour {
    [SerializeField]
    private float m_WeaponDamage = 0f;
    [SerializeField]
    private Vector2 m_PushForce = new Vector2(2f,20f);

	// Use this for initialization
	void Start () {
		
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
                hero.Damage(m_WeaponDamage);
                heroRB.AddForce(new Vector2(-hero.transform.localScale.x*m_PushForce.x, m_PushForce.y), ForceMode2D.Force);
            }
        }
    }
    private bool IsNotGettingDamage(PlayerController heroPc)
    {
        return heroPc.IsInAnimationTag("Damage")
            ||heroPc.IsInAnimationTag("Death");
    }
}
