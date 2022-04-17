using UnityEngine;

namespace VoxelSystem {
    public class VoxelMaterial {
        public bool isInvisible = false;
        public Material mat;
    }
    public class VoxelUVMaterial : VoxelMaterial {
        public float uvScale;
        public Vector2Int texcoord;
    }
    public class VoxelSeperateUVMaterial : VoxelMaterial {
        public float uvScale;
        public Vector2Int texcoordUp;
        public Vector2Int texcoordDown;
        public Vector2Int texcoordFront;
        public Vector2Int texcoordBack;
        public Vector2Int texcoordRight;
        public Vector2Int texcoordLeft;
    }
}