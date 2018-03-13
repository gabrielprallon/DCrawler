using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FeatherSword.Actions
{
    public class DoingSomeAction : MonoBehaviour
    {
        private PlayerController m_PC;
        private AIController m_AIC;

        private void Start()
        {
            m_PC = gameObject.GetComponent<PlayerController>();
            m_AIC = gameObject.GetComponent<AIController>();
        }

        public bool IsDoingSomething()
        {
            if (m_PC)
            {
                return m_PC.IsInAnimationTag("Attack")
                || m_PC.IsInAnimationTag("Block")
                || m_PC.IsInAnimationTag("Jump")
                || m_PC.IsInAnimationTag("Dodge")
                || m_PC.IsInAnimationTag("Damage")
                || m_PC.IsInAnimationTag("Death");
            }
            else
            {
                if (m_AIC)
                {
                    return m_AIC.IsInAnimationTag("Attack")
                    || m_AIC.IsInAnimationTag("Block")
                    || m_AIC.IsInAnimationTag("Jump")
                    || m_AIC.IsInAnimationTag("Dodge")
                    || m_AIC.IsInAnimationTag("Damage")
                    || m_AIC.IsInAnimationTag("Death");
                }
                else { return false; }
            }

        }



    }
}
