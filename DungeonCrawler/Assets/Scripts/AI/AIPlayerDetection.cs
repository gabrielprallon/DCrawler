using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerDetection : MonoBehaviour {

    private bool m_HitPlayer = false;
    [SerializeField]
    private Transform m_Player;
    public Vector2 m_Dir = new Vector2(-1, 0);
    private void Start()
    {
        if(!m_Player)
            m_Player = GameObject.FindWithTag("Player").transform;
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

    public bool PlayerDetection2(float range)
    {
        float distanceToTarget = Vector3.Distance(transform.position, m_Player.position);
        Vector3 targetDir = m_Player.position - transform.position;
        if (distanceToTarget <= range)
        {
            if (targetDir.x < 0)
                m_Dir = new Vector2(-1, 0);
            else
                m_Dir = new Vector2(1, 0);
            return true;
        }else
            return false;
    }
}
