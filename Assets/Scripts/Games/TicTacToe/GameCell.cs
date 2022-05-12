using System;
using System.Collections.Generic;
using Common.Graph;
using UnityEngine;

namespace Games.TicTacToe
{
    public class GameCell : MonoBehaviour
    {
        [SerializeField] private GameObject m_symbolO;
        [SerializeField] private GameObject m_symbolX;
        
        private Collider m_collider;
        
        [field:SerializeField]
        public int PositionX { get; private set; }
        
        [field:SerializeField]
        public int PositionY { get; private set; }

        public event Action<GraphCoordinates> ClickEvent;
        
        private void Awake()
        {
            m_collider = GetComponent<Collider>();
        }
        
        // Called via Unity Event
        public void OnClick() => ClickEvent?.Invoke(new GraphCoordinates(PositionX, PositionY));

        public void OnStateChanged(GameState state)
        {
            if (!state.Nodes.TryGetNodeAtCoordinates(new GraphCoordinates(PositionX, PositionY), out var node))
            {
                Debug.LogWarning($"Node at coordinates {PositionX}, {PositionY} not found.");
                return;
            }

            if (state.NodeOwners.IsVariety(node, GameCellOwner.O))
            {
                // TODO - Animate symbol intro
                m_collider.enabled = false;
                m_symbolO.SetActive(true);
                m_symbolX.SetActive(false);
                return;
            }
            
            if (state.NodeOwners.IsVariety(node, GameCellOwner.X))
            {
                // TODO - Animate symbol intro
                m_collider.enabled = false;
                m_symbolO.SetActive(false);
                m_symbolX.SetActive(true);
                return;
            }

            m_collider.enabled = true;
            m_symbolO.SetActive(false);
            m_symbolX.SetActive(false);
        }

        public void OnGameWon(GameCellOwner owner, List<GraphCoordinates> winningLine)
        {
            m_collider.enabled = false;
            m_symbolO.SetActive(false);
            m_symbolX.SetActive(false);
        }

        public void OnGameDraw()
        {
            m_collider.enabled = false;
            m_symbolO.SetActive(false);
            m_symbolX.SetActive(false);
        }
    }
}
