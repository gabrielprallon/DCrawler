using FeatherSword.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions { 

    public class PlayerJump : ActionBase {
        [SerializeField]
        private Rigidbody2D m_CachedRigidbody;
        [SerializeField]
        private float m_JumpSpeed = 10f;
        private PlayerController m_PC;

        [SerializeField]
        private float m_JumpPrepTime = 0.2f;
        

        // Use this for initialization
        private void Start()
        {
            if (!m_CachedRigidbody)
                m_CachedRigidbody = GetComponent<Rigidbody2D>();
            m_PC = GetComponent<PlayerController>();
            if (m_PC)
            {
                m_PC.onHitGroundEvent += OnHitGround;
                m_PC.RegisterUpdateAction(this);
            }
        }

        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            Jump(m_CachedRigidbody, m_JumpSpeed, status);
        }

        private void Update()
        {
            m_PC.SetAnimatorFloat("VelocityY", m_CachedRigidbody.velocity.y);
        }

        public void OnHitGround()
        {
            if (m_PC.IsInAnimationTag("Jump"))
            {
                m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.EndJump);
            }
        }

        public void Jump(Rigidbody2D RB, float JumpSpeed, bool status)
        {
            if (m_PC.IsGrounded && status)
            {
                m_PC.SetAnimatorTrigger(PlayerController.AnimationTriggers.StartJump);
                StartCoroutine(StartJump(RB));
            }

        }

        private IEnumerator StartJump(Rigidbody2D RB)
        {
            yield return new WaitForSeconds(m_JumpPrepTime);
            ActualJump(RB);
        }

        private void ActualJump(Rigidbody2D RB)
        {
            RB.AddForce(new Vector2(0, m_JumpSpeed), ForceMode2D.Impulse);
        }
    }
}

