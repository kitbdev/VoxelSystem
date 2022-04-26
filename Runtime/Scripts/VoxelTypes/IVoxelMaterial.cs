
// have meshers and type and voxel here

// cubic - perfect cube, one material, any layer. more efficient, can be smaller
// cuboid - minecraft like, cubes or partial cubes or any boxes or any quads. have presets for stuff like water or flowers
//          can have different materials/textures per face
// density - allows smooth marching cubes, maybe sharp edges? material or color data

// todo rename Type to VoxelMaterial?


// box encoding? better than rle?

// held in array by scriptable object
// replaces voxelmaterial

using System.Collections.Generic;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Contains all data that Voxel Meshers need to create a mesh for a voxel and render it.
    /// </summary>
    [System.Serializable]
    public abstract class IVoxelMaterial {
        public string name = "Unknown";

        public abstract IEnumerable<Material> GetAllMaterials();
        public abstract IVoxelMaterial GetEmptyType();
        public abstract IVoxelMaterial GetErrorType();
    }
}