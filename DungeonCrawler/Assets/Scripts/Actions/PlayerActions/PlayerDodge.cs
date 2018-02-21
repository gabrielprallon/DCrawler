﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FeatherSword.Actions
{
    public class PlayerDodge : ActionBase
    {
        [SerializeField]
        private Rigidbody2D m_CachedRigidbody;
        private bool m_IsGrounded;
        private PlayerController m_PC;
        [SerializeField]
        private Vector2 m_RollForce = new Vector2 (50f,0);
        


        // Use this for initialization
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
            Dodge(m_CachedRigidbody, status);
        }
        public void Dodge(Rigidbody2D RB, bool status)
        {
            if (status && !m_PC.IsInAnimationState("BodyDodge"))
            {
                m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.Dodge);
                RB.AddForce(m_RollForce * transform.localScale.x, ForceMode2D.Impulse);
            }
        }
    }
}
