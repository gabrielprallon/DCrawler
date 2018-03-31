using FeatherSword.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerDetection : MonoBehaviour {

    private bool m_HitPlayer = false;
    [SerializeField]
    private Transform m_Player;
    private PlayerController m_PlayerController;
    public Vector2 m_Dir = new Vector2(-1, 0);
    private void Start()
    {
        if(!m_Player)
            m_Player = GameObject.FindWithTag("Player").transform;
        if (m_Player)
            m_PlayerController = m_Player.GetComponent<PlayerController>();
    }
    public bool PlayerDetection(Vector2 direction, float rayRange)
    {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, rayRange);
        if (!m_Player)
            return false;
        if (hit.collider.CompareTag("Body")
            ||hit.collider.CompareTag("Player")
            ||hit.collider.CompareTag("Weapon"))
            m_HitPlayer = true;
        else
            m_HitPlayer = false;
        return m_HitPlayer;
    }

    Vector3 lastDirection = Vector3.zero;

    public bool PlayerDetection3(float rayRange)
    {
        if (!m_Player)
            return false;
        Vector3 targetDir = (m_Player.position + (m_PlayerController?(Vector3)m_PlayerController.PlayerCenterOffset:Vector3.zero) - transform.position).normalized;
        lastDirection = targetDir * rayRange;
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, targetDir, rayRange, m_Player.gameObject.layer);
        if (!hit) return false;
        if (targetDir.x < 0)
        {
            m_Dir = new Vector2(-1, 0);
        }
        else
        {
            m_Dir = new Vector2(1, 0);
        }
        return true;
    }
    public bool PlayerDetection2(float range)
    {
        Vector3 targetPos = transform.position + (m_PlayerController ? (Vector3)m_PlayerController.PlayerCenterOffset : Vector3.zero);
        float distanceToTarget = Vector3.Distance(targetPos, m_Player.position);
        Vector3 targetDir = m_Player.position - targetPos;
        if (distanceToTarget <= range)
        {
            if (targetDir.x < 0)
            {
                m_Dir = new Vector2(-1, 0);
            }
            else
            {
                m_Dir = new Vector2(1, 0);
            }
            return true;
        }else
            return false;
    }
    public Vector3 PlayerDistanceOnAxis()
    {
        Vector3 distance = m_Player.position - transform.position;
        return distance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(transform.position, transform.position + lastDirection);
    }
}
