using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FeatherSword.Room {
    public class Room : MonoBehaviour {

        [SerializeField]
        private PlayerRespawner m_RoomRespawner;

        [SerializeField]
        private Animator m_EntryDoor;
        [SerializeField]
        private Animator m_ExitDoor;

        public void InitializeRoom()
        {
            
        }

        public void ActivateRoom()
        {

        }

        public Vector3 StartRespawn()
        {
            m_EntryDoor.SetTrigger("Open");
            return m_RoomRespawner.GetNextSpawnPoint();
        }

        public void EndRespawn()
        {
            m_EntryDoor.SetTrigger("Close");
        }
    }
}
