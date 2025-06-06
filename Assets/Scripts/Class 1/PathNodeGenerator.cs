using System;
using System.Collections.Generic;
using UnityEngine;

namespace Class1
{
    [Serializable]
    public class PathGenerator
    {
        [Serializable]
        private struct TerrainLayer
        {
            public string tag;
            public float costMultiplier;
        }

        [SerializeField] private Transform lowerLeftLimit;
        [SerializeField] private Transform upperRightLimit;
        [SerializeField, Range(0.1f, 10f)] private float nodesSeparation;
        [SerializeField] private TerrainLayer[] terrainLayers;
        [SerializeField] private LayerMask ignoreLayers;

        
        private bool AreNodesAdjacent (PathNode first, PathNode second, float maxSqrDistance)
        {
            Vector3 diff = first.Position - second.Position;

            return first != second && diff.sqrMagnitude <= maxSqrDistance;
        }

        public List<PathNode> GenerateNodes ()
        {
            List<PathNode> nodes = new List<PathNode>();

            float separationSqr = nodesSeparation * nodesSeparation;
            float maxSqrDistance = separationSqr + separationSqr;

            const float raycastHeight = 100f;

            float startX = lowerLeftLimit.position.x;
            float endX = upperRightLimit.position.x;
            float startZ = lowerLeftLimit.position.z;
            float endZ = upperRightLimit.position.z;

            for (float x = startX; x <= endX; x += nodesSeparation)
            {
                for (float z = startZ; z <= endZ; z += nodesSeparation)
                {
                    RaycastHit hitInfo;
                    Vector3 rayOrigin = new Vector3(x, raycastHeight, z);

                    if (Physics.Raycast(rayOrigin, Vector3.down, out hitInfo, raycastHeight, ~ignoreLayers))
                    {
                        foreach (TerrainLayer terrainLayer in terrainLayers)
                        {
                            if (hitInfo.collider.CompareTag(terrainLayer.tag))
                            {
                                PathNode pathNode = new PathNode()
                                {
                                    Position = hitInfo.point,
                                    CostMultiplier = terrainLayer.costMultiplier,
                                    AccumulatedCost = 0f
                                };

                                nodes.Add(pathNode);
                                break;
                            }
                        }
                    }
                }
            }

            foreach (PathNode node in nodes)
                node.AdjacentNodes = nodes.FindAll(n => AreNodesAdjacent(node, n, maxSqrDistance));

            return nodes;
        }
    }
}