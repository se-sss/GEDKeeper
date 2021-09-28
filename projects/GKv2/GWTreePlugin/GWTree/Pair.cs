using System.Drawing;

namespace GWTree
{
    public class Pair : Group
    {
        public Node NodeA
        {
            get {
                return (fNodes.Count > 0) ? fNodes[0] : null;
            }
        }

        public Node NodeB
        {
            get {
                return (fNodes.Count > 1) ? fNodes[1] : null;
            }
        }

        public Pair(TreeModel model)
            : base(model)
        {
        }

        public override Node GetLeftEdge()
        {
            var nodeA = NodeA;
            var nodeB = NodeB;

            if (nodeA == null) {
                return nodeB;
            } else if (nodeB == null) {
                return nodeA;
            } else {
                return nodeA.Order < nodeB.Order ? nodeA : nodeB;
            }
        }

        public override Node GetRightEdge()
        {
            var nodeA = NodeA;
            var nodeB = NodeB;

            if (nodeA == null) {
                return nodeB;
            } else if (nodeB == null) {
                return nodeA;
            } else {
                return nodeA.Order < nodeB.Order ? nodeB : nodeA;
            }
        }

        public override void DrawLinks(Graphics gfx)
        {
            if (IsEmpty) {
                return;
            }
            var nodeA = NodeA;
            var nodeB = NodeB;
            if (nodeA == null || nodeB == null) return;
            fModel.DrawLine(gfx, 0, nodeA.x + nodeA.width / 2, nodeA.y + nodeA.height / 2, nodeB.x + nodeB.width / 2, nodeB.y + nodeB.height / 2);
        }

        public override void RemoveNode(Node node)
        {
            base.RemoveNode(node);
            if (fNodes.Count < 2) {
                IsEmpty = true;
            }
        }

        public override void ShiftPivot(int offset)
        {
            GetLeftEdge().x = GetLeftEdge().x + offset;
        }

        public override PointF GetPivot(ref Node left, ref Node right)
        {
            left = GetLeftEdge();
            right = GetRightEdge();

            PointF pivot = new PointF();
            pivot.X = (left.x + right.x + right.width) / 2;
            pivot.Y = (left.y + right.y + right.height) / 2;
            return pivot;
        }

        public void Assign(Node node1, Node node2)
        {
            AddNode(node1);
            AddNode(node2);
        }
    }
}
