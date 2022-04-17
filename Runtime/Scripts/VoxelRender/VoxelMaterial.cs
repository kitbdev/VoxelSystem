using UnityEngine;

namespace VoxelSystem {
    // todo editor to make it easier to edit
    /// <summary>
    /// Contains all data that Voxel Meshers need to create a mesh for a voxel and render it
    /// </summary>
    [System.Serializable]
    public class VoxelMaterial {
        public bool hasCubeMesh = true;
        public bool hasCollision = false;
        public Color tint;
        public Kutil.Layer layer;
        public bool useWorldPosUV = false;
        public Vector2 worldUVOffset;
        public Vector2 worldUVScale;
        public Material mat;
        public Vector2 texcoord;
        public Vector2 uvScale;

// todo auto set to and from single mat
        public bool splitFaces = false;
        public Material matUp;
        public Material matDown;
        public Material matFront;
        public Material matBack;
        public Material matRight;
        public Material matLeft;
        public Vector2Int texcoordUp;
        public Vector2Int texcoordDown;
        public Vector2Int texcoordFront;
        public Vector2Int texcoordBack;
        public Vector2Int texcoordRight;
        public Vector2Int texcoordLeft;
    }
}