using System.Collections.Generic;
using System.Linq;
using Common.Extensions;
using Common.Graph;
using UnityEngine;

namespace Games.TicTacToe
{
    public static class GameAI
    {
        public static bool TryPickCell(
            GameState state,
            GameCellOwner owner,
            GameDifficulty difficulty,
            out GraphCoordinates cell)
        {
            var openCells = state.Nodes
                .FindNodes(node => !state.NodeOwners.HasVariety(node))
                .Select(state.Nodes.GetNodeCoordinates)
                .ToList();

            if (!openCells.Any())
            {
                cell = default;
                return false;
            }

            // If there is a winning move now, take it.
            if (difficulty >= GameDifficulty.Medium && TryPickWinningCell(state, owner, openCells, out cell))
            {
                return true;
            }
            
            // If the enemy can win next turn, block it.
            if (difficulty >= GameDifficulty.Hard && TryPickWinningCell(state, owner.Other(), openCells, out cell))
            {
                return true;
            }

            return TryPickRandomCell(openCells, out cell);
        }
        
        private static bool TryPickWinningCell(
            GameState state,
            GameCellOwner owner,
            List<GraphCoordinates> openCells,
            out GraphCoordinates cell)
        {
            foreach (var openCell in openCells)
            {
                if (!GameUtils.TryAddOwner(state, openCell, owner, out var testState))
                {
                    Debug.LogWarning("Failed to assign cell.");
                    continue;
                }

                if (!GameUtils.TryFindOwnerWin(testState, owner, out _))
                {
                    continue;
                }

                cell = openCell;
                return true;
            }

            cell = default;
            return false;
        }
        
        private static bool TryPickRandomCell(List<GraphCoordinates> openCells, out GraphCoordinates cell)
        {
            cell = openCells.PickRandom();
            return true;
        }
    }
}