using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions {

    public class PlayerMovement : ActionBase {

        public enum MovementType
        {
            GROUND,
            AIR,
            GROUND_AIR
        }
        public enum MovementControl
        {
            RBVELOCITY,
            FORCE
        }

        [SerializeField]
        private float m_PlayerSpeed = 10f;
        [SerializeField]
        private Rigidbody2D m_CachedRigidbody;
        [SerializeField]
        private MovementType m_MovementType = MovementType.GROUND;
        [SerializeField]
        private MovementControl m_MovementControl = MovementControl.RBVELOCITY;
        [SerializeField]
        private ForceMode2D m_ForceMode = ForceMode2D.Force;

        [SerializeField]
        private float m_Adjustment = 0.2f;
        
        private bool facingRight = true;
        private Vector2 m_MoveDirection = Vector2.zero;
        private PlayerController m_PC;
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
            Move(m_CachedRigidbody, m_PlayerSpeed, data);
        }
        private void Update()
        {
            if (m_MoveDirection.x >= m_Adjustment || m_MoveDirection.x <= -m_Adjustment)
            {
                m_PC.m_Animator[0].SetBool("IsRunning", true);
                m_PC.m_Animator[1].SetBool("IsRunning", true);
            }
            else
            {
                m_PC.m_Animator[0].SetBool("IsRunning", false);
                m_PC.m_Animator[1].SetBool("IsRunning", false);
            }
        }
        private bool IsAttacking()
        {
            return m_PC.m_Animator[0].GetCurrentAnimatorStateInfo(0).IsTag("Attack");
        }
        private bool IsBlocking()
        {
            return m_PC.m_Animator[0].GetCurrentAnimatorStateInfo(0).IsTag("Block");
        }

        public void Move(Rigidbody2D RB, float moveSpeed, float horizontalDirection) {
            
            
            if (!m_PC.IsGrounded && m_MovementType == MovementType.GROUND)
                return;
            if (m_PC.IsGrounded && m_MovementType == MovementType.AIR)
                return;
            
            if (!IsAttacking() && !IsBlocking()) 
            {
                m_MoveDirection = new Vector3(horizontalDirection, 0);
                
                m_MoveDirection = transform.TransformDirection(m_MoveDirection);
                m_MoveDirection *= moveSpeed;
                if (horizontalDirection > 0 && !facingRight || horizontalDirection < 0 && facingRight)
                {
                    facingRight = !facingRight;
                    Vector3 theScale = transform.localScale;
                    theScale.x *= -1;
                    transform.localScale = theScale;
                }
                if (m_MovementControl == MovementControl.RBVELOCITY)
                {
                    m_MoveDirection.y = RB.velocity.y;
                    RB.velocity = m_MoveDirection;
                }
                if (m_MovementControl == MovementControl.FORCE)
                {
                    RB.AddForce(m_MoveDirection, m_ForceMode);
                }
            }
            else
            {
                RB.velocity = Vector2.zero;
            }

            
           
        }
        
     
    }
}
