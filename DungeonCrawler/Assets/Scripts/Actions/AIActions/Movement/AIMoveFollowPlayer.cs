using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions.AIActions
{
    public class AIMoveFollowPlayer : EnemyMovement
    {
        private void Start()
        {
            if (!m_AIC)
                m_AIC = GetComponent<AIController>();
            if (m_AIC)
                m_AIC.RegisterFixedUpdateAction(this);
        }
        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            MoveToPlayer(status);
        }
        public void MoveToPlayer(bool status)
        {
            if (!IsDoingSomething())
            {
                if (status)
                {
                    m_AIC.m_RB.velocity = m_AIC.m_PDSys.m_Dir * m_MoveSpeed;
                }
                if (m_AIC.m_PDSys.m_Dir.x > 0)
                {
                    transform.localScale =  new Vector3 (-1,1,0);
                }
                else
                {
                    transform.localScale = new Vector3(1, 1, 0);
                }
                
                
            }
        }
    }
}
