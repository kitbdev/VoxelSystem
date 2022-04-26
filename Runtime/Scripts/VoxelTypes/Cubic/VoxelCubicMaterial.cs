using System.Collections.Generic;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    // todo editor to make it easier to edit?
    /// <summary>
    /// Contains all data for Cubic Voxels for Meshers.
    /// layer, material, collision, etc.
    /// </summary>
    [System.Serializable]
    public class VoxelMaterialCubic : IVoxelMaterial {
        [Header("General")]
        public bool hasMesh = true;
        public bool onSeperateLayer = false;
        public Kutil.Layer sperateLayer = 0;
        public bool isTransparent = false;
        public bool showConnectedFacesOfSameType = false;
        public Material mat;
        public Vector2 uvCoord;
        public Vector2 uvScale;
        // color only mode?
        // triplanar option
        // public Color tint;
        [Header("Collision")]
        public bool hasCollision = false;
        [Header("World Offset")]
        public bool useWorldPosUV = false;
        public Vector2 worldUVOffset;
        public Vector2 worldUVScale;
        // todo navmesh
        // [Header("NavMesh")]
        // public int navMeshWalkable = 0;


        public override IEnumerable<Material> GetAllMaterials() {
            return new Material[] { mat };
        }
        public override IVoxelMaterial GetErrorType() => new VoxelMaterialCubic() {
            name = "Error",
            hasMesh = true,
            isTransparent = false,
            onSeperateLayer = false,
            mat = null,// todo error mat
            hasCollision = false,
        };

        public override IVoxelMaterial GetEmptyType() => new VoxelMaterialCubic() {
            name = "Empty",
            hasMesh = false,
            isTransparent = true,
            onSeperateLayer = false,
            mat = null,
            hasCollision = false,
        };

        // todo transparent logic helper func
    }
    [System.Serializable]
    public class VoxelMaterialCuboid : IVoxelMaterial {
        // todo general stuff in base class?

        // todo auto set to and from single mat
        // ? performance when not using split faces?

        public bool splitFaces = false;
        [ConditionalHide(nameof(splitFaces), true)]
        public Material matUp;
        [ConditionalHide(nameof(splitFaces), true)]
        public Material matDown;
        [ConditionalHide(nameof(splitFaces), true)]
        public Material matFront;
        [ConditionalHide(nameof(splitFaces), true)]
        public Material matBack;
        [ConditionalHide(nameof(splitFaces), true)]
        public Material matRight;
        [ConditionalHide(nameof(splitFaces), true)]
        public Material matLeft;
        [ConditionalHide(nameof(splitFaces), true)]
        public Vector2Int texcoordUp;
        [ConditionalHide(nameof(splitFaces), true)]
        public Vector2Int texcoordDown;
        [ConditionalHide(nameof(splitFaces), true)]
        public Vector2Int texcoordFront;
        [ConditionalHide(nameof(splitFaces), true)]
        public Vector2Int texcoordBack;
        [ConditionalHide(nameof(splitFaces), true)]
        public Vector2Int texcoordRight;
        [ConditionalHide(nameof(splitFaces), true)]
        public Vector2Int texcoordLeft;


        public override IEnumerable<Material> GetAllMaterials() {
            // todo 
            return default;
            // return new Material[] { mat };
        }
        public override IVoxelMaterial GetErrorType() => new VoxelMaterialCubic() {
            name = "Error",
            hasMesh = true,
            isTransparent = false,
            sperateLayer = -1,
            mat = null,// todo error mat
            hasCollision = false,
        };

        public override IVoxelMaterial GetEmptyType() => new VoxelMaterialCubic() {
            name = "Empty",
            hasMesh = false,
            isTransparent = true,
            sperateLayer = -1,
            mat = null,
            hasCollision = false,
        };
    }
    [System.Serializable]
    public class VoxelMaterialDensity : IVoxelMaterial {


        public override IEnumerable<Material> GetAllMaterials() {
            // todo 
            return default;
            // return new Material[] { mat };
        }
        public override IVoxelMaterial GetErrorType() => new VoxelMaterialCubic() {
            name = "Error",
            hasMesh = true,
            isTransparent = false,
            sperateLayer = -1,
            mat = null,// todo error mat
            hasCollision = false,
        };

        public override IVoxelMaterial GetEmptyType() => new VoxelMaterialCubic() {
            name = "Empty",
            hasMesh = false,
            isTransparent = true,
            sperateLayer = -1,
            mat = null,
            hasCollision = false,
        };
    }
}