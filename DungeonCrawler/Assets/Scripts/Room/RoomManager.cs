using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FeatherSword.Room
{
    public class RoomManager : MonoBehaviour
    {
        private static RoomManager m_Instance;
        
        public static RoomManager Instance {
            get { return m_Instance; }
        }

        public Room CurrentRoom
        {
            get { return m_CurrentRoom; }
        }

        [SerializeField]
        private List<GameObject> m_Rooms;
            
        private Room m_CurrentRoom;

        private void Awake()
        {
            m_Instance = this;
        }
    }
}
