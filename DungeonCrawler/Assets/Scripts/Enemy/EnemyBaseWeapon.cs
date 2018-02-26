using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseWeapon : MonoBehaviour {
    [SerializeField]
    private float m_WeaponDamage = 0f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnCollisionEnter2D(Collision2D other)
    {
        HealthSystem Hero = other.gameObject.GetComponent<HealthSystem>();
        if (Hero)
        {
            Hero.Damage(m_WeaponDamage);
        }
    }
}
