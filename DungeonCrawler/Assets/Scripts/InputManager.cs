using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


namespace FeatherSword.Input
{
    public class InputManager 
    {

        private static InputManager m_Instance;

        public static InputManager Instance
        {
            get
            {
                if (m_Instance == null) m_Instance = new InputManager();
                return m_Instance;
            }
        }

        public Player m_Player;
        public Vector3 m_MoveVector;
        public bool m_KeyDown;
        public bool m_KeyUp;

        public virtual void GetPlayer(int ID)
        {
            m_Player = ReInput.players.GetPlayer(ID);
        }
        public virtual float GetHorizontalInput(int ID)
        {
            GetPlayer(ID);
            m_MoveVector.x = m_Player.GetAxis("MoveHorizontal");
            return m_MoveVector.x;
        }
        public virtual bool GetJumpInput(int ID)
        {
            GetPlayer(ID);
            m_KeyDown = m_Player.GetButtonDown("Jump");
            if (m_KeyDown)
            {
                return true;
            }

            return false;
        }
        public virtual bool GetDodgeInput(int ID)
        {
            GetPlayer(ID);
            m_KeyDown = m_Player.GetButtonDown("Dodge");
            if (m_KeyDown)
            {
                return true;
            }

            return false;
        }
        public virtual bool GetAttackInput(int ID)
        {
            GetPlayer(ID);
            m_KeyDown = m_Player.GetButtonDown("Attack");
            if (m_KeyDown)
            {
                return true;
            }

            return false;
        }
        public virtual bool GetBlockInput(int ID)
        {
            GetPlayer(ID);
            m_KeyDown = m_Player.GetButtonDown("Block");
            
            if (m_Player.GetButtonSinglePressHold("Block")){
                return true;
            }
            else
            {
                return false;
            }
            
            
        }
        public virtual bool GetInteractionInput(int ID)
        {
            GetPlayer(ID);
            m_KeyDown = m_Player.GetButtonDown("Interaction");
            m_MoveVector.y = m_Player.GetAxis("UpInteraction"); 
            if (m_KeyDown||m_MoveVector.y!=0)
            {
                return true;
            }

            return false;
        }



    }
}
