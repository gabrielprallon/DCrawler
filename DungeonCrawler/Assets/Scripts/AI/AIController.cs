using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions
{
    public class AIController : ActionBase
    {
        public class AnimationTriggers
        {
            public AnimationTriggers(string value) { Value = value; }

            public string Value;


        }

        [SerializeField]
        private List<ActionBase> m_UpdateActions = new List<ActionBase>();
        [SerializeField]
        private List<ActionBase> m_FixedUpdateActions = new List<ActionBase>();

        [SerializeField]
        private List<Animator> m_Animator = new List<Animator>();
        [SerializeField]
        private Rigidbody2D m_RB;

        public int m_EnemyID = 0;

        [Header("Ground detection")]
        private bool m_IsGrounded = false;
        [SerializeField]
        private bool m_ShowGroundDetectionDebug = true;
        [SerializeField]
        private LayerMask m_GroundLayer;
        [SerializeField]
        private float m_GroundCheckDist;
        [SerializeField]
        private float m_CircleCastRadius;
        [SerializeField]
        private Vector3 m_GroundDetectionOffset;
        [SerializeField]
        private Collider2D m_GroundStableCollider;

        // simple events
        public delegate void OnHitGroundEvent();
        public delegate void OnLeaveGroundEvent();

        public event OnHitGroundEvent onHitGroundEvent;
        public event OnLeaveGroundEvent onLeaveGroundEvent;
        //

        public bool IsGrounded
        {
            get { return m_IsGrounded; }
        }
        // Use this for initialization
        private void Start()
        {
            if(!m_RB)
                m_RB = GetComponent<Rigidbody2D>();

        }
        private void Update()
        {

            /*
             * Action ID 0 = Horizontal Movement
             * Action ID 1 = Jump
             * Action ID 2 = Dodge/Dash
             * Action ID 3 = Attack
             * Action ID 4 = Block
            */
            CheckForGround();
            foreach (ActionBase action in m_UpdateActions)
            {
                action.CallAction(0);
                action.CallAction(1);
                action.CallAction(2);
                action.CallAction(3);
                action.CallAction(4);
            }
        }
        private void FixedUpdate()
        {

            /*
             * Action ID 0 = Horizontal Movement
             * Action ID 1 = Jump
             * Action ID 2 = Dodge/Dash
             * Action ID 3 = Attack
             * Action ID 4 = Block
            */
            CheckForGround();
            foreach (ActionBase action in m_FixedUpdateActions)
            {
                action.CallAction(0);
                action.CallAction(1);
                action.CallAction(2);
                action.CallAction(3);
                action.CallAction(4);
            }
        }
        public void RegisterUpdateAction(ActionBase action)
        {
            if (!m_UpdateActions.Contains(action))
            {
                m_UpdateActions.Add(action);
            }
        }

        public void RegisterFixedUpdateAction(ActionBase action)
        {
            if (!m_FixedUpdateActions.Contains(action))
            {
                m_FixedUpdateActions.Add(action);
            }
        }

        public bool IsInAnimationTag(string tag)
        {
            foreach (Animator anim in m_Animator)
                if (anim.GetCurrentAnimatorStateInfo(0).IsTag(tag))
                    return true;
            return false;
        }
        public bool IsInAnimationState(string state)
        {
            foreach (Animator anim in m_Animator)
                if (anim.GetCurrentAnimatorStateInfo(0).IsName(state))
                    return true;
            return false;
        }

        public void SetAnimatorTrigger(AnimationTriggers trigger)
        {
            foreach (Animator anim in m_Animator)
                anim.SetTrigger(trigger.Value);
        }
        public void SetAnimatorFloat(string parameter, float value)
        {
            foreach (Animator anim in m_Animator)
                anim.SetFloat(parameter, value);
        }

        public bool CheckForGround()
        {
            Vector2 Position = new Vector2(transform.position.x + m_GroundDetectionOffset.x, transform.position.y + m_GroundDetectionOffset.y);
            var grounded = Physics2D.CircleCast(Position, m_CircleCastRadius, Vector2.down, m_GroundCheckDist, m_GroundLayer);
            if (!m_IsGrounded && grounded)
            {
                OnHitGround();
            }
            if (m_IsGrounded && !grounded)
            {
                OnLeaveGround();
            }
            m_IsGrounded = grounded;
            return IsGrounded;
        }

        protected void OnHitGround()
        {
            if (m_GroundStableCollider)
                m_GroundStableCollider.enabled = true;
            if (onHitGroundEvent != null)
            {
                onHitGroundEvent();
            }
        }

        protected void OnLeaveGround()
        {
            if (m_GroundStableCollider)
                m_GroundStableCollider.enabled = false;
            if (onLeaveGroundEvent != null)
            {
                onLeaveGroundEvent();
            }
        }
        void OnDrawGizmos()
        {
            if (m_ShowGroundDetectionDebug)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position + m_GroundDetectionOffset, m_CircleCastRadius);
                Gizmos.DrawWireSphere(transform.position + m_GroundDetectionOffset + Vector3.down * m_GroundCheckDist, m_CircleCastRadius);
                Gizmos.DrawLine(transform.position + m_GroundDetectionOffset, transform.position + m_GroundDetectionOffset + Vector3.down * m_GroundCheckDist);
            }
        }


    }
}
