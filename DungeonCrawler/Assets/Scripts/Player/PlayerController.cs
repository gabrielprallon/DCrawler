using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FeatherSword.Actions;
using FeatherSword.Input;

public class PlayerController : MonoBehaviour {
    
    [SerializeField]
    private List<ActionBase> m_UpdateActions = new List<ActionBase>();
    [SerializeField]
    private List<ActionBase> m_FixedUpdateActions = new List<ActionBase>();
    
    public List<Animator> m_Animator = new List<Animator>(); //0 body, 1 Weapon
    
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

    private bool[] buttonInput = new bool[5];
    public int m_PlayerID = 0;

    /*public bool IsGrounded {
        get { return m_IsGrounded; }
        set { m_IsGrounded = value; }
    }*/

    public List<Animator> Animator
    {
        get { return m_Animator; }
        set { m_Animator = value; }
    }

    public bool IsGrounded
    {
        get { return m_IsGrounded; }
    }

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

    void FixedUpdate() {
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
        if (!m_UpdateActions.Contains(action)) {
            m_UpdateActions.Add(action);
        }
    }

    public void RegisterFixedUpdateAction(ActionBase action)
    {
        if (!m_FixedUpdateActions.Contains(action)) {
            m_FixedUpdateActions.Add(action);
        }
    }

    public bool CheckForGround()
    {
        Vector2 Position = new Vector2(transform.position.x + m_GroundDetectionOffset.x, transform.position.y + m_GroundDetectionOffset.y);
        m_IsGrounded = Physics2D.CircleCast(Position, m_CircleCastRadius, Vector2.down, m_GroundCheckDist, m_GroundLayer);
        m_Animator[0].SetBool("IsGrounded", m_IsGrounded);
        m_Animator[1].SetBool("IsGrounded", m_IsGrounded);
        return IsGrounded;
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
