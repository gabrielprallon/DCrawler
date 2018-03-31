using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FeatherSword.Actions;
using FeatherSword.Player;

namespace FeatherSword.AI {
    public class AIController : MonoBehaviour
    {
        public class AnimationTriggers
        {
            public AnimationTriggers(string value) { Value = value; }

            public string Value;

            public static AnimationTriggers Damage { get { return new AnimationTriggers("Damage"); } }
            public static AnimationTriggers Attack { get { return new AnimationTriggers("Attack"); } }
            public static AnimationTriggers DoAttack { get { return new AnimationTriggers("DoAttack"); } }
            public static AnimationTriggers RangedAttack { get { return new AnimationTriggers("RangedAttack"); } }
            public static AnimationTriggers RangedAttack2 { get { return new AnimationTriggers("RangedAttack2"); } }
            public static AnimationTriggers RangedAttack3 { get { return new AnimationTriggers("RangedAttack3"); } }
            public static AnimationTriggers Die { get { return new AnimationTriggers("Death"); } }


        }

        [SerializeField]
        private List<ActionBase> m_UpdateActions = new List<ActionBase>();
        [SerializeField]
        private List<ActionBase> m_FixedUpdateActions = new List<ActionBase>();

        [SerializeField]
        private List<Animator> m_Animator = new List<Animator>();

        public Rigidbody2D m_RB;

        [Header("Player detection")]
        public AIPlayerDetection m_PDSys;
        [SerializeField]
        private PlayerController m_Player;

        [SerializeField]
        private float m_RayMoveRange = 20f;
        [SerializeField]
        private bool m_ShowRayMeleeDebug = true;
        [SerializeField]
        private float m_RayMeleeAttackRange = 3f;
        [SerializeField]
        private float m_RayRangedAttackRange = 6f;

        private bool[] m_ActivateAction = new bool[7];
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
            if (!m_RB)
                m_RB = GetComponent<Rigidbody2D>();
            if (!m_PDSys)
                m_PDSys = GetComponent<AIPlayerDetection>();
            if (!m_Player)
                m_Player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();


        }
        private void Update()
        {
            m_ActivateAction[4] = m_PDSys.PlayerDetection2(m_RayMeleeAttackRange);
            //m_ActivateAction[5] = m_PDSys.PlayerDetection(transform.localScale, m_RayRangedAttackRange);
            /*
             * Action ID 0 = Horizontal Movement
             * Action ID 1 = Vertical Movement
             * Action ID 2 = Jump
             * Action ID 3 = Dodge/Dash
             * Action ID 4 = Melee Attack
             * Action ID 5 = Ranged Attack
             * Action ID 6 = Block
            */

            CheckForGround();
            if (!PlayerGettingDamageOrDead())
            {
                foreach (ActionBase action in m_UpdateActions)
                {
                    action.CallAction(0, m_ActivateAction[0]);
                    action.CallAction(1);
                    action.CallAction(2);
                    action.CallAction(3);
                    action.CallAction(4, m_ActivateAction[4]);
                    action.CallAction(5);
                    action.CallAction(6);
                }
            }
        }
        private void FixedUpdate()
        {

            /*
             * Action ID 0 = Horizontal Movement
             * Action ID 1 = Vertical Movement
             * Action ID 2 = Jump
             * Action ID 3 = Dodge/Dash
             * Action ID 4 = Melee Attack
             * Action ID 5 = Ranged Attack
             * Action ID 6 = Block
            */
            m_ActivateAction[0] = m_PDSys.PlayerDetection3(m_RayMoveRange);
            if (!PlayerGettingDamageOrDead())
            {
                foreach (ActionBase action in m_FixedUpdateActions)
                {
                    action.CallAction(0, m_ActivateAction[0]);
                    action.CallAction(1);
                    action.CallAction(2);
                    action.CallAction(3);
                    action.CallAction(4, m_ActivateAction[4]);
                    action.CallAction(5);
                    action.CallAction(6);
                }
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

        protected bool PlayerGettingDamageOrDead()
        {
            return m_Player.IsInAnimationTag("Damage")
                || m_Player.IsInAnimationTag("Death");
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
            if (m_ShowRayMeleeDebug)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(transform.position - transform.right * m_RayMeleeAttackRange, 0.1f);
                Gizmos.DrawLine(transform.position, transform.position - transform.right * m_RayMeleeAttackRange);
            }
        }


    }

    }

