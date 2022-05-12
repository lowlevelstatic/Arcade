using System;
using System.Collections.Generic;
using System.Linq;
using Common.Graph;
using UnityEngine;

namespace Games.TicTacToe
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private GameDifficulty m_difficulty = GameDifficulty.Hard;
        
        private GameState m_state = GameState.Empty;
        private Dictionary<GraphCoordinates, GameCell> m_cells;

        public event Action<GameState> StateChangedEvent;
        public event Action<GameCellOwner, List<GraphCoordinates>> GameWonEvent;
        public event Action GameDrawEvent;
        public event Action GameResetEvent;

        private void Start()
        {
            var banner = FindObjectOfType<GameBanner>();
            banner.ClickEvent += ResetGame;
            GameWonEvent += banner.OnGameWon;
            GameDrawEvent += banner.OnGameDraw;
            GameResetEvent += banner.OnGameReset;
            
            m_cells = FindObjectsOfType<GameCell>()
                .ToDictionary(cell => new GraphCoordinates(cell.PositionX, cell.PositionY), cell => cell);
            
            Debug.Log($"Found {m_cells.Count} cells");
            
            foreach (var cell in m_cells.Values)
            {
                cell.ClickEvent += OnCellClicked;
                StateChangedEvent += cell.OnStateChanged;
                GameWonEvent += cell.OnGameWon;
                GameDrawEvent += cell.OnGameDraw;
            }
            
            GameUtils.AddCells(m_state, m_cells.Keys, UpdateState);
        }

        private void OnCellClicked(GraphCoordinates coordinates)
        {
            SetOwner(coordinates, GameCellOwner.X);

            if (GameUtils.TryFindOwnerWin(m_state, GameCellOwner.X, out var winningLineX))
            {
                Debug.Log("Player X has won!");
                GameWonEvent?.Invoke(GameCellOwner.X, winningLineX);
                return;
            }

            if (GameUtils.IsBoardFull(m_state))
            {
                Debug.Log("Draw Game!");
                GameDrawEvent?.Invoke();
                return;
            }

            if (!GameAI.TryPickCell(m_state, GameCellOwner.O, m_difficulty, out var aiCell))
            {
                Debug.Log("Failed to complete AI turn.");
                return;
            }
            
            SetOwner(aiCell, GameCellOwner.O);
            
            if (GameUtils.TryFindOwnerWin(m_state, GameCellOwner.O, out var winningLineO))
            {
                Debug.Log("Player O has won!");
                GameWonEvent?.Invoke(GameCellOwner.O, winningLineO);
                return;
            }

            if (GameUtils.IsBoardFull(m_state))
            {
                Debug.Log("Draw Game!");
                GameDrawEvent?.Invoke();
                return;
            }
        }

        private void SetOwner(GraphCoordinates coordinates, GameCellOwner owner)
        {
            if (GameUtils.TryAddOwner(m_state, coordinates, owner, out var newState))
            {
                UpdateState(newState);
            }
        }

        private void ResetGame()
        {
            GameUtils.ClearAllOwners(m_state, UpdateState);
            GameResetEvent?.Invoke();
        }

        private void UpdateState(GameState state)
        {
            Debug.Log("Updating Game State");
            m_state = state;
            StateChangedEvent?.Invoke(state);
        }
    }
}
