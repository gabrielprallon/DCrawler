using FeatherSword.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions.AIActions
{
    public class EnemyMovement : ActionBase{

        [SerializeField]
        protected AIController m_AIC;
        [SerializeField]
        protected float m_MoveSpeed = 5f;
        [SerializeField]
        protected bool m_FacingRight = false;

        protected virtual void Start()
        {
            if(!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterFixedUpdateAction(this);
        }

        protected virtual bool IsDoingSomething()
        {
            return m_AIC.IsInAnimationTag("Attack")
                || m_AIC.IsInAnimationTag("Block")
                || m_AIC.IsInAnimationTag("Jump")
                || m_AIC.IsInAnimationTag("Dodge")
                || m_AIC.IsInAnimationTag("Damage")
                || m_AIC.IsInAnimationTag("Death");
        }
        protected virtual void DirectionAdjustment()
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
