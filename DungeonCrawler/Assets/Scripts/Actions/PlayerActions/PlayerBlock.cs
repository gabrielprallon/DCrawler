using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions
{
    public class PlayerBlock : ActionBase
    {
        private PlayerController m_PC;
        [SerializeField]
        private Rigidbody2D m_CachedRigidbody;

        private bool m_ActivateBlock = false;
        private bool m_IsBlocking = false;
        private float m_BlockTimer = 0f;
        [SerializeField]
        private float m_BlockCd = 2f;
        

        private void Start()
        {
            if (!m_CachedRigidbody)
                m_CachedRigidbody = GetComponent<Rigidbody2D>();
            m_PC = GetComponent<PlayerController>();
            if (m_PC)
                m_PC.RegisterUpdateAction(this);
            m_BlockTimer = m_BlockCd;
        }

        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            Block(m_CachedRigidbody, status);
        }

        public void Block(Rigidbody2D RB, bool status)
        {            
            if (status && !m_IsBlocking)
            {
                ToBlockPosition(RB);

            }
            if(!status && m_IsBlocking)
            {
                ToStopBlockingPos();
            }
        }

        private void ToBlockPosition(Rigidbody2D RB)
        {
            m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.StartBlock);
            m_IsBlocking = true;
            m_ActivateBlock = false;
            
            
        }
        private void ToStopBlockingPos()
        {
            m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.EndBlock);
            m_IsBlocking = false;
            m_BlockTimer = m_BlockCd;
            m_ActivateBlock = false;
        }
        public void ObjectBlocked()
        {
            m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.ReactBlock);
        }
        
    }
}
