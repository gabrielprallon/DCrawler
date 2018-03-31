using FeatherSword.Player;
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


        private bool m_IsBlocking = false;
        
        

        private void Start()
        {
            if (!m_CachedRigidbody)
                m_CachedRigidbody = GetComponent<Rigidbody2D>();
            m_PC = GetComponent<PlayerController>();
            if (m_PC)
                m_PC.RegisterUpdateAction(this);
        }


        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            Block(m_CachedRigidbody, status);
        }

        public void Block(Rigidbody2D RB, bool status)
        {
            if (!m_PC.IsGrounded) return;
            if (status && !m_IsBlocking)
            {
                ToBlockPosition(RB);
                return;
            }
            if(!status && m_IsBlocking)
            {
                ToStopBlockingPos();
            }
        }

        private void ToBlockPosition(Rigidbody2D RB)
        {
            m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.StartBlock);
            RB.velocity = new Vector2(0f, RB.velocity.y);
            m_IsBlocking = true;          
        }

        private void ToStopBlockingPos()
        {
            m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.EndBlock);
            m_IsBlocking = false;
        }
        public void ObjectBlocked()
        {
            m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.ReactBlock);
        }
        
    }
}
