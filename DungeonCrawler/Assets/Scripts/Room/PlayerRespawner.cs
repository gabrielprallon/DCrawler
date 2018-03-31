using FeatherSword.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FeatherSword.Room
{
    public class PlayerRespawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject m_PlayerPrefab;
        [SerializeField]
        private Transform m_SpawnPoint;

        private GameObject m_CurrentPlayer;

        public GameObject CurrentPlayer
        {
            get { return m_CurrentPlayer; }
            set { m_CurrentPlayer = value; }
        }

        public void SpawnPlayer()
        {
            if (CurrentPlayer)
            {
                RespawnPlayer();
            }
        }

        public void RespawnPlayer()
        {
            PlayerController player = m_CurrentPlayer.GetComponent<PlayerController>();
            if (!player)
            {
                Debug.LogError("Player has no player controller");
                return;
            }
        }

        private void OnDrawGizmosSelected()
        {
            if (m_SpawnPoint)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawSphere(m_SpawnPoint.position, 0.6f);
            }
        }
    }
}
