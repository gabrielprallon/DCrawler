using FeatherSword.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions.AIActions
{
    public class AIMoveOnPathWay : EnemyMovement
    {
        [SerializeField]
        private bool m_PathIsRandom = false;
        [SerializeField]
        private GameObject m_PathWay;
        [SerializeField]
        private List<Transform> m_WayPoint = new List<Transform>();

        private int m_WayPointAux;
        
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            MoveOnPathway(status);
        }
        // Update is called once per frame
        public void MoveOnPathway(bool status)
        {
            if (status)
            {
                if (m_PathIsRandom)
                {
                    m_WayPointAux = Random.Range(0, m_WayPoint.Count);
                }
                MoveOnPath(m_WayPointAux);
                if (!m_PathIsRandom)
                {
                    m_WayPointAux++;
                    if (m_WayPointAux >= m_WayPoint.Count)
                    m_WayPointAux = 0;
                }
            }
            DirectionAdjustment();
        }

        private void MoveOnPath(int nextPoint)
        {
            Vector3 dir = MoveDirOnPath(nextPoint);
            m_AIC.m_RB.velocity = dir * m_MoveSpeed;
        }
        private Vector3 MoveDirOnPath(int nextPoint)
        {
            Vector3 holder = transform.position - m_WayPoint[nextPoint].position;
            holder.z = 0;
            return holder.normalized;
        }
    }
}
