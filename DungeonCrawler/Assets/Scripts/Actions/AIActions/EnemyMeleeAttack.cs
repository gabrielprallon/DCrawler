using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FeatherSword.Actions.AIActions
{
    public class EnemyMeleeAttack : ActionBase
    {
        protected AIController m_AIC;
        [SerializeField]
        protected float m_HoldAttackTime;
        // Use this for initialization
        private void Start()
        {
            if (!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterUpdateAction(this);

        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            AIMeleeAttack(status);
        }
        protected bool IsDoingSomething()
        {
            return m_AIC.IsInAnimationTag("Attack")
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
                    StartCoroutine(WaitToAttack());
                }
            }
        }
        protected IEnumerator WaitToAttack()
        {
            yield return new WaitForSeconds(m_HoldAttackTime);
            m_AIC.SetAnimatorTrigger(AIController.AnimationTriggers.DoAttack);
        }
    }
}