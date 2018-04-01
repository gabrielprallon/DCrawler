using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FeatherSword.Actions;
using FeatherSword.Input;
using FeatherSword.Room;

namespace FeatherSword.Player
{
    public class PlayerController : MonoBehaviour
    {
        public class AnimationTriggers
        {
            public AnimationTriggers(string value) { Value = value; }

            public string Value;

            public static AnimationTriggers Jump { get { return new AnimationTriggers("Jumping"); } }
            public static AnimationTriggers Attack1 { get { return new AnimationTriggers("Attack1"); } }
            public static AnimationTriggers Attack2 { get { return new AnimationTriggers("Attack2"); } }
            public static AnimationTriggers Attack3 { get { return new AnimationTriggers("Attack3"); } }
            public static AnimationTriggers StartBlock { get { return new AnimationTriggers("ToBlockPos"); } }
            public static AnimationTriggers EndBlock { get { return new AnimationTriggers("StopBlocking"); } }
            public static AnimationTriggers ReactBlock { get { return new AnimationTriggers("Blocked"); } }
            public static AnimationTriggers Dodge { get { return new AnimationTriggers("Dodge"); } }
            public static AnimationTriggers Idle { get { return new AnimationTriggers("Idle"); } }
            public static AnimationTriggers Damage { get { return new AnimationTriggers("Damage"); } }
            public static AnimationTriggers StartRun { get { return new AnimationTriggers("Run"); } }
            public static AnimationTriggers StartJump { get { return new AnimationTriggers("Jumping"); } }
            public static AnimationTriggers EndJump { get { return new AnimationTriggers("Landing"); } }
            public static AnimationTriggers Death { get { return new AnimationTriggers("Death"); } }
            public static AnimationTriggers Respawn { get { return new AnimationTriggers("Spawn"); } }
            public static AnimationTriggers LeaveRoom { get { return new AnimationTriggers("Leave"); } }
            public static AnimationTriggers LandingDeath { get { return new AnimationTriggers("LandingDeath"); } }
        }

        [SerializeField]
        private List<ActionBase> m_UpdateActions = new List<ActionBase>();
        [SerializeField]
        private List<ActionBase> m_FixedUpdateActions = new List<ActionBase>();

        [SerializeField]
        private List<Animator> m_Animator = new List<Animator>(); //0 body, 1 Weapon

        private bool m_IsGrounded = false;
        [Header("Ground detection")]
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
        [Header("PlayerDetection")]
        [SerializeField]
        private Vector2 m_PlayerCenterOffset;

        private bool[] buttonInput = new bool[5];
        public int m_PlayerID = 0;

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

        public Vector2 PlayerCenterOffset
        {
            get
            {
                return m_PlayerCenterOffset;
            }

            set
            {
                m_PlayerCenterOffset = value;
            }
        }

        public bool WantsToRun = false;

        private void Update()
        {
            float horizontalMovement = InputManager.Instance.GetHorizontalInput(m_PlayerID);

            buttonInput[0] = (InputManager.Instance.GetJumpInput(m_PlayerID));
            buttonInput[1] = (InputManager.Instance.GetDodgeInput(m_PlayerID));
            buttonInput[2] = (InputManager.Instance.GetAttackInput(m_PlayerID));
            buttonInput[3] = (InputManager.Instance.GetBlockInput(m_PlayerID));
            buttonInput[4] = (InputManager.Instance.GetInteractionInput(m_PlayerID));

            /* 
             * Action ID 0 = Horizontal Movement
             * Action ID 1 = Jump
             * Action ID 2 = Dodge
             * Action ID 3 = Attack
             * Action ID 4 = Block
             * Action ID 5 = Interaction
             */
            foreach (ActionBase action in m_UpdateActions)
            {
                action.CallAction(0, horizontalMovement);
                action.CallAction(1, buttonInput[0]);
                action.CallAction(2, buttonInput[1]);
                action.CallAction(3, buttonInput[2]);
                action.CallAction(4, buttonInput[3]);
                action.CallAction(5, buttonInput[4]);
            }
        }

        void FixedUpdate()
        {
            float horizontalMovement = InputManager.Instance.GetHorizontalInput(m_PlayerID);

            buttonInput[0] = (InputManager.Instance.GetJumpInput(m_PlayerID));
            buttonInput[1] = (InputManager.Instance.GetDodgeInput(m_PlayerID));
            buttonInput[2] = (InputManager.Instance.GetAttackInput(m_PlayerID));
            buttonInput[3] = (InputManager.Instance.GetBlockInput(m_PlayerID));
            buttonInput[4] = (InputManager.Instance.GetInteractionInput(m_PlayerID));

            /* 
             * Action ID 0 = Horizontal Movement
             * Action ID 1 = Jump
             * Action ID 2 = Dodge
             * Action ID 3 = Attack
             * Action ID 4 = Block
             * Action ID 5 = Interaction
             */
            CheckForGround();

            foreach (ActionBase action in m_FixedUpdateActions)
            {
                action.CallAction(0, horizontalMovement);
                action.CallAction(1, buttonInput[0]);
                action.CallAction(2, buttonInput[1]);
                action.CallAction(3, buttonInput[2]);
                action.CallAction(4, buttonInput[3]);
                action.CallAction(5, buttonInput[4]);
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

        public void Die()
        {
            SetAnimatorTrigger(PlayerController.AnimationTriggers.Death);
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
            if (grounded && IsInAnimationState("DeathFallingLoop"))
            {
                SetAnimatorTrigger(AnimationTriggers.LandingDeath);
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

        public bool IsDying()
        {
            return IsInAnimationTag("Death") || IsInAnimationTag("Dead") || IsInAnimationTag("Respawn");
        }
        public void Respawn()
        {
            StartCoroutine(RespawnPlayer());
        }

        IEnumerator RespawnPlayer()
        {

            SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();
            foreach (SpriteRenderer sr in renderers)
            {
                sr.enabled = false;
            }
            transform.position = RoomManager.Instance.CurrentRoom.StartRespawn();
            yield return new WaitForSeconds(1f);
            SetAnimatorTrigger(AnimationTriggers.Respawn);
            foreach (SpriteRenderer sr in renderers)
            {
                sr.enabled = true;
            }
            yield return new WaitForSeconds(1f);
            RoomManager.Instance.CurrentRoom.EndRespawn();
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