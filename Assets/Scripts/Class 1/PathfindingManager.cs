using System.Collections.Generic;
using UnityEngine;

namespace Class1
{
    public class PathfindingManager : MonoBehaviourSingleton<PathfindingManager>
    {
        public enum PathfindingStrategy
        {
            BreathFirst,
            DepthFirst
        }

        [SerializeField] private PathGenerator pathGenerator = new PathGenerator();
        [SerializeField] private PathfindingStrategy pathfindingStrategy = PathfindingStrategy.BreathFirst;

        private List<PathNode> pathNodes;
        private List<PathNode> openNodes;
        private List<PathNode> closedNodes;


        void Start ()
        {
            if (pathNodes == null)
                GeneratePath();
            
            openNodes = new List<PathNode>();
            closedNodes = new List<PathNode>();
        }

        void OnDrawGizmos ()
        {
            if (pathNodes == null)
                return;

            Gizmos.color = Color.blue;

            foreach (PathNode pathNode in pathNodes)
            {
                foreach (PathNode adjacentNode in pathNode.AdjacentNodes)
                    Gizmos.DrawLine(pathNode.Position, adjacentNode.Position);
            }           
        }

        
        [ContextMenu("Generate Path")]
        private void GeneratePath ()
        {
            pathNodes = pathGenerator.GenerateNodes();
        }

        private PathNode FindClosestNode (Vector3 position)
        {
            PathNode closestNode = null;

            float closestSqrDistance = float.MaxValue;

            foreach (PathNode pathNode in pathNodes)
            {
                float sqrDistance = (pathNode.Position - position).sqrMagnitude;

                if (sqrDistance < closestSqrDistance)
                {
                    closestSqrDistance = sqrDistance;
                    closestNode = pathNode;
                }
            }

            return closestNode;
        }

        private PathNode GetNextOpenNode ()
        {
            if (openNodes.Count == 0)
                return null;

            PathNode openNode = null;

            switch (pathfindingStrategy)
            {
                case PathfindingStrategy.BreathFirst:
                    openNode = openNodes[0];
                    break;
                
                case PathfindingStrategy.DepthFirst:
                    openNode = openNodes[^1];
                    break;
            }

            return openNode;
        }

        private void OpenNode (PathNode node)
        {
            if (openNodes.Contains(node))
                return;

            node.CurrentState = PathNode.State.Open;
            openNodes.Add(node);
        }

        private void CloseNode (PathNode node)
        {
            if (!openNodes.Contains(node))
                return;

            node.CurrentState = PathNode.State.Closed;
            openNodes.Remove(node);
            closedNodes.Add(node);
        }

        private void OpenAdjacentNodes (PathNode node)
        {
            foreach (PathNode pathNode in node.AdjacentNodes)
            {
                if (pathNode.CurrentState != PathNode.State.Unreviewed)
                    continue;

                pathNode.Parent = node;

                OpenNode(pathNode);
            }
        }

        private void ResetNodes ()
        {
            foreach (PathNode pathNode in pathNodes)
            {
                if (pathNode.CurrentState == PathNode.State.Unreviewed)
                    continue;

                pathNode.CurrentState = PathNode.State.Unreviewed;
                pathNode.Parent = null;
            }

            openNodes.Clear();
            closedNodes.Clear();
        }

        private Stack<PathNode> GeneratePath (PathNode originNode, PathNode destinationNode)
        {
            Stack<PathNode> path = new Stack<PathNode>();

            PathNode currentNode = destinationNode;

            while (currentNode != null)
            {
                path.Push(currentNode);
                currentNode = currentNode.Parent;
            }

            return path;
        }

        public Stack<PathNode> CreatePath (Vector3 origin, Vector3 destination)
        {
            Stack<PathNode> path = null;

            PathNode originNode = FindClosestNode(origin);
            PathNode destinationNode = FindClosestNode(destination);

            OpenNode(originNode);

            while (openNodes.Count > 0 && path == null)
            {
                PathNode openNode = GetNextOpenNode();

                if (openNode == destinationNode)
                    path = GeneratePath(originNode, destinationNode);
                else
                    OpenAdjacentNodes(openNode);

                CloseNode(openNode);
            }

            ResetNodes();

            return path;
        }
    }
}