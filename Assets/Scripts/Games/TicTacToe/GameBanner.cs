using System;
using System.Collections.Generic;
using Common.Graph;
using UnityEngine;

namespace Games.TicTacToe
{
    public class GameBanner : MonoBehaviour
    {
        [SerializeField] private GameObject m_symbolO;
        [SerializeField] private GameObject m_symbolX;
        [SerializeField] private GameObject[] m_wonText;
        [SerializeField] private GameObject[] m_tieText;

        private Collider m_collider;
        
        public event Action ClickEvent;
        
        private void Awake()
        {
            m_collider = GetComponent<Collider>();
        }

        // Called via Unity Event
        public void OnClick() => ClickEvent?.Invoke();
        
        public void OnGameWon(GameCellOwner owner, List<GraphCoordinates> winningLine)
        {
            m_collider.enabled = true;
            
            m_symbolO.SetActive(owner == GameCellOwner.O);
            m_symbolX.SetActive(owner == GameCellOwner.X);

            foreach (var letter in m_wonText)
            {
                letter.SetActive(true);
            }
            
            foreach (var letter in m_tieText)
            {
                letter.SetActive(false);
            }
        }

        public void OnGameDraw()
        {
            m_collider.enabled = true;
            
            m_symbolO.SetActive(false);
            m_symbolX.SetActive(false);

            foreach (var letter in m_wonText)
            {
                letter.SetActive(false);
            }
            
            foreach (var letter in m_tieText)
            {
                letter.SetActive(true);
            }
        }

        public void OnGameReset()
        {
            m_collider.enabled = false;
            
            m_symbolO.SetActive(false);
            m_symbolX.SetActive(false);

            foreach (var letter in m_wonText)
            {
                letter.SetActive(false);
            }
            
            foreach (var letter in m_tieText)
            {
                letter.SetActive(false);
            }
        }
    }
}
