using Common.Graph;

namespace Games.TicTacToe
{
    public class MutableGameState
    {
        public readonly MutableGraphNodes Nodes;
        public readonly MutableGraphVarieties<NodeID, GameCellOwner> NodeOwners;
        
        public MutableGameState(
            MutableGraphNodes nodes,
            MutableGraphVarieties<NodeID, GameCellOwner> nodeOwners)
        {
            Nodes = nodes;
            NodeOwners = nodeOwners;
        }
        
        public GameState EndEdit() => new(
            Nodes.CloneImmutable(),
            NodeOwners.CloneImmutable());
    }
}