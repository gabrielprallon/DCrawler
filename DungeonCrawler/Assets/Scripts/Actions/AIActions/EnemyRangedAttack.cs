using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FeatherSword.Actions.AIActions
{
    public class EnemyRangedAttack : ActionBase
    {
        [SerializeField]
        private AIController m_AIC;
        [SerializeField]
        private float m_throwCD = 3f;
        private float m_ThrowTime;
        private bool m_CanThrow;
        [SerializeField]
        private GameObject m_ThrowingObjectPF;
        [SerializeField]
        private bool m_MultipleShotHeights = false;
        [SerializeField]
        private float m_ShotAdjustment = 0f;

        // Use this for initialization
        void Start()
        {
            if (!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterUpdateAction(this);
        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            AIRangedAttack(status);
        }
        private bool IsDoingSomething()
        {
            return m_AIC.IsInAnimationTag("Attack")
                || m_AIC.IsInAnimationTag("Block")
                || m_AIC.IsInAnimationTag("Jump")
                || m_AIC.IsInAnimationTag("Dodge")
                || m_AIC.IsInAnimationTag("Damage")
                || m_AIC.IsInAnimationTag("Die");
        }

        private void AIRangedAttack(bool status)
        {
            m_ThrowTime += Time.deltaTime;
            if (!IsDoingSomething())
            {
                if (status)
                {
                    if (!m_MultipleShotHeights)
                        m_AIC.SetAnimatorTrigger(AIController.AnimationTriggers.RangedAttack);
                    else
                        MultipleShotsHeights();
                    if (CanShot())
                        SummonProjectile();
                }
            }
        }
        private void SummonProjectile()
        {
            Instantiate(m_ThrowingObjectPF, transform.position, Quaternion.identity);
            m_ThrowTime = 0;
        }
        private bool CanShot()
        {
            if (m_ThrowTime >= m_throwCD)
                return true;
            return false;
        }
        private void MultipleShotsHeights()
        {
            if (m_AIC.m_PDSys.PlayerDistanceOnAxis().y > m_ShotAdjustment)
                m_AIC.SetAnimatorTrigger(AIController.AnimationTriggers.RangedAttack2);
            if(m_AIC.m_PDSys.PlayerDistanceOnAxis().y<-m_ShotAdjustment)
                m_AIC.SetAnimatorTrigger(AIController.AnimationTriggers.RangedAttack3);
            if(m_AIC.m_PDSys.PlayerDistanceOnAxis().y>=-m_ShotAdjustment &&
               m_AIC.m_PDSys.PlayerDistanceOnAxis().y <= m_ShotAdjustment)
                m_AIC.SetAnimatorTrigger(AIController.AnimationTriggers.RangedAttack);

        }
    }
}
