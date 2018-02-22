using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayerDetection : MonoBehaviour {

    private bool m_HitPlayer = false;

    public bool PlayerDetection(Vector2 direction, float rayRange)
    {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, direction, rayRange);
        if (hit.collider.CompareTag("Body"))
            m_HitPlayer = true;
        else
            m_HitPlayer = false;
        return m_HitPlayer;
    }
}
