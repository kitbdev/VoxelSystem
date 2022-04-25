
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Holds more context than a voxel, is a class rather than a struct
    /// </summary>
    public class FullVoxel {
        public IVoxel voxel;
        public Vector3Int pos;
        // ? chunkgo? other info?

    }
    /// <summary>
    /// Interface for single voxel instance data
    /// </summary>
    public interface IVoxel : ISaveable {
        void Init(VoxelTypeIdVoxelData typeId);
    }
}