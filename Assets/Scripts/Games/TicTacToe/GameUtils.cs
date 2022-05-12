using System;
using System.Collections.Generic;
using System.Linq;
using Common.Graph;
using UnityEngineInternal;

namespace Games.TicTacToe
{
    public static class GameUtils
    {
        private static readonly Random _random = new();
        
        public static void AddCells(
            GameState state,
            IEnumerable<GraphCoordinates> cellCoordinates,
            Action<GameState> changed)
        {
            var edit = state.BeginEdit();

            foreach (var coordinates in cellCoordinates)
            {
                edit.Nodes.AddNode(coordinates, GraphIDFactory.NodeID());
            }

            changed?.Invoke(edit.EndEdit());
        }

        public static bool TryAddOwner(
            GameState state,
            GraphCoordinates coordinates,
            GameCellOwner owner,
            out GameState newState)
        {
            if (!state.Nodes.TryGetNodeAtCoordinates(coordinates, out var node))
            {
                newState = default;
                return false;
            }

            if (state.NodeOwners.HasVariety(node))
            {
                newState = default;
                return false;
            }

            var edit = state.BeginEdit();
            edit.NodeOwners.SetVariety(node, owner);
            newState = edit.EndEdit();
            return true;
        }

        public static bool TryFindOwnerWin(
            GameState state,
            GameCellOwner owner,
            out List<GraphCoordinates> winningLine)
        {
            var ownedCells = state.Nodes
                .FindNodes(node => state.NodeOwners.IsVariety(node, owner))
                .Select(state.Nodes.GetNodeCoordinates)
                .ToList();

            // Check each row
            foreach (int row in Enumerable.Range(0, GameConstants.Height))
            {
                var line = ownedCells
                    .Where(coordinates => coordinates.y == row)
                    .ToList();
                
                if (line.Count >= GameConstants.Width)
                {
                    winningLine = line;
                    return true;
                }
            }

            // Check each column
            foreach (int column in Enumerable.Range(0, GameConstants.Width))
            {
                var line = ownedCells
                    .Where(coordinates => coordinates.x == column)
                    .ToList();
                
                if (line.Count >= GameConstants.Height)
                {
                    winningLine = line;
                    return true;
                }
            }

            // Check diagonals
            var diagonal1 = new List<GraphCoordinates> { new(0, 0), new(1, 1), new(2, 2) };
             
            if (diagonal1.All(ownedCells.Contains))
            {
                winningLine = diagonal1;
                return true;
            }

            var diagonal2 = new List<GraphCoordinates> { new(0, 2), new(1, 1), new(2, 0) };

            if (diagonal2.All(ownedCells.Contains))
            {
                winningLine = diagonal2;
                return true;
            }

            // Nope, no victory yet
            winningLine = null;
            return false;
        }

        public static bool IsBoardFull(GameState state) => !state.Nodes
            .FindNodes(node => !state.NodeOwners.HasVariety(node))
            .Any();
        
        public static void ClearAllOwners(
            GameState state,
            Action<GameState> changed)
        {
            if (!state.Nodes.FindNodes(state.NodeOwners.HasVariety).Any())
            {
                return;
            }
            
            var edit = state.BeginEdit();
            edit.NodeOwners.Clear();
            changed?.Invoke(edit.EndEdit());
        }

        public static GameCellOwner Other(this GameCellOwner source) => source switch
        {
            GameCellOwner.O => GameCellOwner.X,
            GameCellOwner.X => GameCellOwner.O,
            _ => throw new ArgumentOutOfRangeException(nameof(source), source, "Unknown GameCellOwner")
        };
    }
}
