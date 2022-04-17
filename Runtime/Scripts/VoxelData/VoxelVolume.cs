using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Holds all data necessary for a volume of voxels.
    /// Save, load, and import to this format
    /// </summary>
    public class VoxelVolume {
        // jagged array should be faster
        // y,z,x
        Voxel<int>[][][] voxels;

        public void Set(){

        }
    }
}