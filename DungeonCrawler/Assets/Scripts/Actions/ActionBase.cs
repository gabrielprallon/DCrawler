using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Actions
{
    public class ActionBase : MonoBehaviour
    {
        [SerializeField]
        private int m_ActionID = -1;

        public virtual void CallAction(int ID, float data)
        {
            CallAction(ID, data, false);
        }

        public virtual void CallAction(int ID, bool status)
        {
            CallAction(ID, 0f, status);
        }

        public virtual void CallAction(int ID, float data, bool status)
        {
            if (ID != m_ActionID)
                return;
            CallAction(data, status);
        }

        public virtual void CallAction(float data, bool status)
        {
            PreAction(data, status);
            DoAction(data, status);
            PostAction(data, status);
        }

        public virtual void PreAction(float data, bool status)
        {
        }

        public virtual void DoAction(float data, bool status)
        {
        }

        public virtual void PostAction(float data, bool status)
        {
        }
    }
}