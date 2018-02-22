using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions.AIActions
{
    public class EnemyMovement : ActionBase{

        [SerializeField]
        private AIController m_AIC;
        [SerializeField]
        private float m_MoveSpeed = 5f;

        private bool facingRight = false;
        private Vector2 m_MoveDirection = new Vector2(-1, 0);
        [SerializeField]
        private AIPlayerDetection m_PDSys;
        [SerializeField]
        private float m_RayRange = 20f;

        private bool m_PlayerHit;
    

        // Use this for initialization
        private void Start()
        {
            if(!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterFixedUpdateAction(this);
            if (!m_PDSys)
                m_PDSys = GetComponent<AIPlayerDetection>();
        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            AIMove();
        }
        // Update is called once per frame
        void Update()
        {
            m_PlayerHit = m_PDSys.PlayerDetection(m_MoveDirection, m_RayRange);

        }
        private bool IsDoingSomething()
        {
            return m_AIC.IsInAnimationTag("Attack")
                || m_AIC.IsInAnimationTag("Block")
                || m_AIC.IsInAnimationTag("Jump")
                || m_AIC.IsInAnimationTag("Dodge")
                || m_AIC.IsInAnimationTag("Damage");
        }
        public void AIMove()
        {
            
            if (!IsDoingSomething())
            {
                if (m_PlayerHit)
                {
                    m_AIC.m_RB.velocity = m_MoveDirection * m_MoveSpeed;
                }
                else
                {
                    m_MoveDirection *= -1;
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                }
            }
        }
    }
}
