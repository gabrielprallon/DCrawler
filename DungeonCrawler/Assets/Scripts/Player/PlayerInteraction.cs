using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FeatherSword.Actions
{
    public class PlayerInteraction : ActionBase
    {
        [SerializeField]
        private Rigidbody2D m_CachedRigidbody;
        private PlayerController m_PC;
        private bool m_Door;
        private bool m_Chest;

        // Use this for initialization
        private void Start()
        {
            if (!m_CachedRigidbody)
                m_CachedRigidbody = GetComponent<Rigidbody2D>();
            m_PC = GetComponent<PlayerController>();
            if (m_PC)
                m_PC.RegisterFixedUpdateAction(this);
        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            Interaction(m_CachedRigidbody, status);

        }
        
        public void Interaction(Rigidbody2D RB, bool status)
        {

        }
        private void OnTriggerStay2D(Collider2D col)
        {
            if (col.gameObject.CompareTag("Door"))
                m_Door = true;                            
            if (col.CompareTag("Chest"))
                m_Chest = true;           
        }
    }
}