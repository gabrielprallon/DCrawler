using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FeatherSword.Actions.AIActions
{
    public class EnemyMeleeAttack : ActionBase
    {
        private AIController m_AIC;
        private Animator m_Anim;
        // Use this for initialization
        private void Start()
        {
            if (!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterUpdateAction(this);
            m_Anim = GetComponentInChildren<Animator>();
        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            AIMeleeAttack(status);
        }
        private bool IsDoingSomething()
        {
            return m_AIC.IsInAnimationTag("MeleeAttack")
                || m_AIC.IsInAnimationTag("RangedAttack")
                || m_AIC.IsInAnimationTag("Block")
                || m_AIC.IsInAnimationTag("Jump")
                || m_AIC.IsInAnimationTag("Dodge")
                || m_AIC.IsInAnimationTag("Damage")
                || m_AIC.IsInAnimationTag("Die");
        }
        public void AIMeleeAttack(bool status)
        {
            if (!IsDoingSomething())
            {
                if (status)
                {
                    m_AIC.SetAnimatorTrigger(AIController.AnimationTriggers.Attack);
                }
            }
        }
    }
}