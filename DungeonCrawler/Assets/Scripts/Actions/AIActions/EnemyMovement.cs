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
        private Vector2 m_MoveDirection;
        [SerializeField]
        private float m_RayRange = 20f;

        private bool m_PlayerHit;

        private Transform m_Player;


        // Use this for initialization
        private void Start()
        {
            if(!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterFixedUpdateAction(this);
            m_MoveDirection = new Vector2(-1f * Mathf.Sign(transform.localScale.x), 0f);
        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            AIMove(status);
        }
        // Update is called once per frame

        private bool IsDoingSomething()
        {
            return m_AIC.IsInAnimationTag("MeleeAttack")
                || m_AIC.IsInAnimationTag("RangedAttack")
                || m_AIC.IsInAnimationTag("Block")
                || m_AIC.IsInAnimationTag("Jump")
                || m_AIC.IsInAnimationTag("Dodge")
                || m_AIC.IsInAnimationTag("Damage");
        }
        public void AIMove(bool status)
        {
            
            if (!IsDoingSomething())
            {
                if (status)
                {
                    m_AIC.m_RB.velocity = m_AIC.m_PDSys.m_Dir * m_MoveSpeed;
                }
                if (m_AIC.m_PDSys.m_Dir.x < 0f)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x = 1;
                    transform.localScale = theScale;
                }
                else
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x = -1;
                    transform.localScale = theScale;
                }
            }
        }
    }
}
