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

        public PolygonCollider2D m_BlockTrigger;

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
            m_PC.m_Animator[0].SetTrigger("ToBlockPos");
            m_PC.m_Animator[1].SetTrigger("ToBlockPos");
            m_PC.m_Animator[0].SetBool("IdleBlocking", true);
            m_PC.m_Animator[1].SetBool("IdleBlocking", true);
            m_PC.m_Animator[0].ResetTrigger("Blocked");
            m_PC.m_Animator[1].ResetTrigger("Blocked");
            m_IsBlocking = true;
            m_BlockTrigger.enabled = true;
            m_ActivateBlock = false;
            
            
        }
        private void ToStopBlockingPos()
        {
            m_PC.m_Animator[0].SetBool("IdleBlocking", false);
            m_PC.m_Animator[1].SetBool("IdleBlocking", false);
            m_PC.m_Animator[0].SetTrigger("StopBlocking");
            m_PC.m_Animator[1].SetTrigger("StopBlocking");
            m_PC.m_Animator[0].ResetTrigger("ToBlockPosition");
            m_PC.m_Animator[1].ResetTrigger("ToBlockPosition");
            m_PC.m_Animator[0].ResetTrigger("Blocked");
            m_PC.m_Animator[1].ResetTrigger("Blocked");
            m_IsBlocking = false;
            m_BlockTrigger.enabled = false;
            m_BlockTimer = m_BlockCd;
            m_ActivateBlock = false;
        }
        public void ObjectBlocked()
        {
            m_PC.m_Animator[0].SetTrigger("Blocked");
            m_PC.m_Animator[1].SetTrigger("Blocked");
                   
        }
        
    }
}
