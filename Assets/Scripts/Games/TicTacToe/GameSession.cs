using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Common.Graph;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Games.TicTacToe
{
    public class GameSession : MonoBehaviour
    {
        [SerializeField] private GameDifficulty m_difficulty = GameDifficulty.Hard;
        [SerializeField] [Range(0.2f, 2.5f)] private float m_thinkSeconds = 1.0f;
        [SerializeField] [Range(0.0f, 0.5f)] private float m_cameraShakeIntensity = 0.05f;
        [SerializeField] [Range(0.0f, 0.5f)] private float m_cameraShakeDuration = 0.05f;

        private GameState m_state = GameState.Empty;
        private Dictionary<GraphCoordinates, GameCell> m_cells;

        public event Action<GameState> StateChangedEvent;
        public event Action<GameCellOwner, List<GraphCoordinates>> GameWonEvent;
        public event Action GameDrawEvent;
        public event Action GameResetEvent;
        public event Action<bool> BlockInputEvent;

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

            var raycaster = FindObjectOfType<PhysicsRaycaster>();
            BlockInputEvent += block => raycaster.enabled = !block;
            
            var cameraShake = FindObjectOfType<GameCameraShake>();
            StateChangedEvent += _ => cameraShake.Shake(m_cameraShakeIntensity, m_cameraShakeDuration);
        }

        private void OnCellClicked(GraphCoordinates coordinates) =>
            StartCoroutine(BlockInputAndExecuteTurn(coordinates));

        private IEnumerator BlockInputAndExecuteTurn(GraphCoordinates coordinates)
        {
            BlockInput();
            yield return ExecuteTurnWithDelay(coordinates);
            UnblockInput();
        }

        private IEnumerator ExecuteTurnWithDelay(GraphCoordinates coordinates)
        {
            SetOwner(coordinates, GameCellOwner.X);
            yield return Think();
            
            if (GameUtils.TryFindOwnerWin(m_state, GameCellOwner.X, out var winningLineX))
            {
                Debug.Log("Player X has won!");
                GameWonEvent?.Invoke(GameCellOwner.X, winningLineX);
                yield break;
            }

            if (GameUtils.IsBoardFull(m_state))
            {
                Debug.Log("Draw Game!");
                GameDrawEvent?.Invoke();
                yield break;
            }

            if (!GameAI.TryPickCell(m_state, GameCellOwner.O, m_difficulty, out var aiCell))
            {
                Debug.Log("Failed to complete AI turn.");
                yield break;
            }
            
            SetOwner(aiCell, GameCellOwner.O);
            
            if (GameUtils.TryFindOwnerWin(m_state, GameCellOwner.O, out var winningLineO))
            {
                Debug.Log("Player O has won!");
                yield return Think();
                GameWonEvent?.Invoke(GameCellOwner.O, winningLineO);
                yield break;
            }

            if (GameUtils.IsBoardFull(m_state))
            {
                Debug.Log("Draw Game!");
                yield return Think();
                GameDrawEvent?.Invoke();
                yield break;
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
            m_state = state;
            StateChangedEvent?.Invoke(state);
        }

        private void BlockInput() => BlockInputEvent?.Invoke(true);

        private void UnblockInput() => BlockInputEvent?.Invoke(false);

        private IEnumerator Think()
        {
            yield return new WaitForSeconds(m_thinkSeconds);
        }
    }
}
