using UnityEngine;
using Kutil;
using System.Collections.Generic;
using System.Linq;

namespace VoxelSystem {
    // todo editor to make it easier to edit
    /// <summary>
    /// Contains all data that Voxel Meshers need to create a mesh for a voxel and render it.
    /// </summary>
    [System.Serializable]
    public class VoxelMaterial {
        // any custom data or logic should be implemented on top of this, like blocks with large sizes
        public bool hasCubeMesh = true;
        public bool hasCollision = false;
        // todo transparent logic
        

        // ? navmesh data?

        // color only mode?
        // triplanar option
        public Color tint;
        public Kutil.Layer layer;

        public bool useWorldPosUV = false;
        public Vector2 worldUVOffset;
        public Vector2 worldUVScale;


        public Material mat;
        public Vector2 texcoord;
        public Vector2 uvScale;

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
    }
}