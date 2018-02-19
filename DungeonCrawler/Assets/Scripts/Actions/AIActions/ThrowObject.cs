using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.AI
{
    public class ThrowObject : MonoBehaviour
    {
        [SerializeField]
        private float m_throwCD = 3f;
        private float m_ThrowTime;
        private bool m_CanThrow;
        [SerializeField]
        private GameObject m_ThrowingObjectPF;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void FixedUpdate()
        {
            m_ThrowTime += Time.fixedDeltaTime;
            if (m_ThrowTime >= m_throwCD)
            {
                m_CanThrow = true;
                m_ThrowTime = 0;
            }
            if (m_CanThrow)
            {
                m_CanThrow = false;
                Instantiate(m_ThrowingObjectPF, transform.position, Quaternion.identity);
            }
        }
        

    }
}
