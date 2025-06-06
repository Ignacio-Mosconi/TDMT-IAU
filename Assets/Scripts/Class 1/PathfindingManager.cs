using System.Collections.Generic;
using UnityEngine;

namespace Class1
{
    public class PathfindingManager : MonoBehaviourSingleton<PathfindingManager>
    {
        public enum PathfindingStrategy
        {
            BreadthFirst,
            DepthFirst,
            Dijkstra,
            AStar
        }

        [SerializeField] private PathGenerator pathGenerator = new PathGenerator();
        [SerializeField] private PathfindingStrategy pathfindingStrategy = PathfindingStrategy.BreadthFirst;

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

        private PathNode GetNextOpenNode (PathNode destinationNode)
        {
            if (openNodes.Count == 0)
                return null;

            PathNode openNode = null;

            switch (pathfindingStrategy)
            {
                case PathfindingStrategy.BreadthFirst:
                    openNode = GetNextOpenNodeBreadthFirst();
                    break;
                
                case PathfindingStrategy.DepthFirst:
                    openNode = GetNextOpenNodeDepthFirst();
                    break;

                case PathfindingStrategy.Dijkstra:
                    openNode = GetNextOpenNodeDijkstra();
                    break;
                
                case PathfindingStrategy.AStar:
                    openNode = GetNextOpenNodeAStar(destinationNode);
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

        private void OpenAdjacentNodes (PathNode parentNode)
        {
            foreach (PathNode pathNode in parentNode.AdjacentNodes)
            {
                if (pathNode.CurrentState != PathNode.State.Unreviewed)
                    continue;

                pathNode.Parent = parentNode;

                switch (pathfindingStrategy)
                {
                    case PathfindingStrategy.Dijkstra:
                    case PathfindingStrategy.AStar:
                        float sqrDistance = (parentNode.Position - pathNode.Position).sqrMagnitude;
                        pathNode.AccumulatedCost = parentNode.AccumulatedCost + sqrDistance * pathNode.CostMultiplier;
                        break;
                }

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
                pathNode.AccumulatedCost = 0f;
            }

            openNodes.Clear();
            closedNodes.Clear();
        }

        private PathNode GetNextOpenNodeBreadthFirst () => openNodes[0];
        
        private PathNode GetNextOpenNodeDepthFirst () => openNodes[^1];

        private PathNode GetNextOpenNodeDijkstra ()
        {
            PathNode openNode = openNodes[0];

            foreach (PathNode pathNode in openNodes)
            {
                if (pathNode.AccumulatedCost < openNode.AccumulatedCost)
                    openNode = pathNode;
            }
            
            return openNode;
        }
        
        private PathNode GetNextOpenNodeAStar (PathNode destinationNode)
        {
            PathNode openNode = openNodes[0];

            float closestSqrDistance = (destinationNode.Position - openNode.Position).sqrMagnitude;

            foreach (PathNode pathNode in openNodes)
            {
                float sqrDistance = (destinationNode.Position - pathNode.Position).sqrMagnitude;

                if (pathNode.AccumulatedCost <= openNode.AccumulatedCost && sqrDistance < closestSqrDistance)
                {
                    openNode = pathNode;
                    closestSqrDistance = sqrDistance;
                }
            }
            
            return openNode;
        }

        private Stack<PathNode> GeneratePath (PathNode destinationNode)
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
                PathNode openNode = GetNextOpenNode(destinationNode);

                if (openNode == destinationNode)
                    path = GeneratePath(destinationNode);
                else
                    OpenAdjacentNodes(openNode);

                CloseNode(openNode);
            }

            ResetNodes();

            return path;
        }
    }
}