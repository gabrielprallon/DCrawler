using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions.AIActions
{
    public class EnemyMovement : ActionBase{

        [SerializeField]
        private AIController m_AIC;
        [SerializeField]
        private float m_MoveSpeed = 5f;
        [SerializeField]
        private bool m_FacingRight = false;
        [SerializeField]
        private bool m_MoveOnPath;
        [SerializeField]
        private GameObject m_PathWay;
        [SerializeField]
        private bool m_PathIsRandom = false;
        [SerializeField]
        private List<Transform> m_Path = new List<Transform>();

        private int m_PathCounter=0;

        // Use this for initialization
        private void Start()
        {
            if(!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterFixedUpdateAction(this);
        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            AIMove(status);
        }
        // Update is called once per frame

        private bool IsDoingSomething()
        {
            return m_AIC.IsInAnimationTag("Attack")
                || m_AIC.IsInAnimationTag("Block")
                || m_AIC.IsInAnimationTag("Jump")
                || m_AIC.IsInAnimationTag("Dodge")
                || m_AIC.IsInAnimationTag("Damage")
                || m_AIC.IsInAnimationTag("Death");
        }
        public void AIMove(bool status)
        {
            
            if (!IsDoingSomething())
            {
                if (status)
                {
                    if(!m_MoveOnPath)
                        MoveToPlayer();
                    else
                    {
                        if (m_PathIsRandom)
                        {
                            m_PathCounter = Random.Range(0, m_Path.Count);
                        }
                        else
                        {
                            m_PathCounter++;
                            if (m_PathCounter >= m_Path.Count)
                            {
                                m_PathCounter = 0;
                            }
                        }
                        MoveOnPath(m_PathCounter);
                    }
                }
                DirectionAdjustment();
            }
        }

        private void MoveToPlayer()
        {
            m_AIC.m_RB.velocity = m_AIC.m_PDSys.m_Dir * m_MoveSpeed;
            
        }

        private void MoveOnPath(int nextPoint)
        {
            Vector3 dir = MoveDirOnPath(nextPoint);
            m_AIC.m_RB.velocity = dir * m_MoveSpeed;
        }
        private Vector3 MoveDirOnPath(int nextPoint)
        {
            Vector3 dir = Vector3.zero;
            Vector3 holder = transform.position - m_Path[nextPoint].position;
            if (holder.x > 0)
            {
                dir.x = -1;
            }
            if (holder.x < 0)
            {
                dir.x = 1;
            }
            if (holder.x == 0)
            {
                dir.x = 0;
            }
            if (holder.y > 0)
            {
                dir.y = -1;
            }
            if (holder.y < 0)
            {
                dir.y = 1;
            }
            if (holder.y == 0)
            {
                dir.y = 0;
            }
            dir.z = 0;
            return dir;
        }

        private void DirectionAdjustment()
        {
            if (!m_FacingRight)
            {
                if (m_AIC.m_PDSys.m_Dir.x < 0f)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x = 1;
                    transform.localScale = theScale;
                }
                else
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x = -1;
                    transform.localScale = theScale;
                }
            }
            else
            {
                if (m_AIC.m_PDSys.m_Dir.x < 0f)
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x = -1;
                    transform.localScale = theScale;
                }
                else
                {
                    Vector3 theScale = transform.localScale;
                    theScale.x = 1;
                    transform.localScale = theScale;
                }
            }
        }
    }
}
