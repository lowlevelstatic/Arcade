using System.Linq;
using Common.Extensions;
using Common.Graph;

namespace Games.TicTacToe
{
    public static class GameAI
    {
        public static GraphCoordinates PickRandomCell(GameState state) => state.Nodes
            .FindNodes(node => !state.NodeOwners.HasVariety(node))
            .Select(state.Nodes.GetNodeCoordinates)
            .ToList()
            .PickRandom();
    }
}