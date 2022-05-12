using Common.Graph;

namespace Games.TicTacToe
{
    public readonly struct GameState
    {
        public readonly GraphNodes Nodes;
        public readonly GraphVarieties<NodeID, GameCellOwner> NodeOwners;

        public static GameState Empty => new(
            GraphNodes.Empty,
            GraphVarieties<NodeID, GameCellOwner>.Empty);
        
        public GameState(
            GraphNodes nodes,
            GraphVarieties<NodeID, GameCellOwner> nodeOwners)
        {
            Nodes = nodes;
            NodeOwners = nodeOwners;
        }

        public MutableGameState BeginEdit() => new(
            Nodes.CloneMutable(),
            NodeOwners.CloneMutable());
    }
}