using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FeatherSword.Actions { 
    public class PlayerAttack : ActionBase
    {
        private PlayerController m_PC;
        [SerializeField]
        private Animator m_BodyAnim;
        [SerializeField]
        private Animator m_WeaponAnim;

        private bool m_IsAttacking = false;
        private float m_AttackTimer = 0f;
        [SerializeField]
        private float m_ComboTimer = 0.5f;
        [SerializeField]
        private float m_AttackDelay = 1f;

        private int m_ComboCount = 0;

        private bool m_JustAttack = false;
        private bool m_RestartAttack = false;
        private bool m_AnimationFinished = false;
        private bool m_Anim3Time;
        


        public PolygonCollider2D m_AttackTrigger;

        private void Start()
        {
            m_PC = GetComponent<PlayerController>();
            if (m_PC)
                m_PC.RegisterUpdateAction(this);
            //m_AttackTimer = m_AttackCd;           
        }

        public override void DoAction(float data, bool status)
        {
            base.DoAction(data, status);
            Attack(status);
        }
        
        private bool IsAnimationAttack()
        {
            return (m_BodyAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack") ||
                    m_WeaponAnim.GetCurrentAnimatorStateInfo(0).IsTag("Attack"));
        }

        public void Attack(bool status)
        {
            ResetTriggers();
            if (status && !IsAnimationAttack())
            {
                if ((
                    !m_IsAttacking 
                    )
                    ||
                    m_IsAttacking &&
                    m_ComboCount == 3 &&
                    !m_JustAttack)
                    Attack1();
                if (m_IsAttacking && 
                    m_AttackTimer >= 0f && 
                    m_ComboCount == 1 && 
                    !m_JustAttack)
                    Attack2();
                if (m_IsAttacking && 
                    m_AttackTimer >= 0f && 
                    m_ComboCount == 2 && 
                    !m_JustAttack )
                    Attack3();
            }
        }

        private void Update()
        {
            if (m_IsAttacking && !IsAnimationAttack() && !m_AnimationFinished)
            {
                m_AnimationFinished = true;
                m_AttackTimer = m_ComboTimer;
            }
            
            if (m_AnimationFinished && m_AttackTimer <= 0f && m_ComboCount != 0)
                FinishAttack();
            if(m_AnimationFinished)
                m_AttackTimer -= Time.deltaTime;
        }

        private void Attack1()
        {
            Debug.Log("entrou attack1");
            m_IsAttacking = true;
            m_AttackTrigger.enabled = true;
            m_BodyAnim.SetTrigger("Attack1");
            m_WeaponAnim.SetTrigger("Attack1");
            m_AttackTimer = m_ComboTimer;
            m_ComboCount = 1;
            m_JustAttack = true;
            m_AnimationFinished = false;
        }

        private void Attack2()
        {
            Debug.Log("entrou attack2");
            m_BodyAnim.SetTrigger("Attack2");
            m_WeaponAnim.SetTrigger("Attack2");
            m_AttackTimer = m_ComboTimer;
            m_ComboCount = 2;
            m_JustAttack = true;
            m_AnimationFinished = false;
        }

        private void Attack3()
        {
            Debug.Log("entrou Attack3");
            m_BodyAnim.SetTrigger("Attack3");
            m_WeaponAnim.SetTrigger("Attack3");
            m_AttackTimer = m_ComboTimer;
            m_ComboCount = 3;
            m_JustAttack = true;
            m_AnimationFinished = false;
        }

        private void FinishAttack()
        {
            Debug.Log("finalizando attack");
            m_IsAttacking = false;
            m_AttackTrigger.enabled = false;
            m_AttackTimer = m_AttackDelay;
            m_ComboCount = 0;
            m_JustAttack = true;
            m_AnimationFinished = false;
        }

        private void ResetTriggers()
        {
            m_JustAttack = false;
            m_BodyAnim.ResetTrigger("Attack1");
            m_BodyAnim.ResetTrigger("Attack2");
            m_BodyAnim.ResetTrigger("Attack3");
            m_WeaponAnim.ResetTrigger("Attack1");
            m_WeaponAnim.ResetTrigger("Attack2");
            m_WeaponAnim.ResetTrigger("Attack3");
        }      
            
    }






    
}
