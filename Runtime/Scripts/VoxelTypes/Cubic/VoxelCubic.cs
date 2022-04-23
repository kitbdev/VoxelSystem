using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Stores instance information for Cubic voxels
    /// </summary>
    [System.Serializable]
    public struct VoxelCubic : IVoxel {
        public VoxelTypeId typeId;

        public VoxelCubic(VoxelTypeId typeId) {
            this.typeId = typeId;
        }
        public void Init(VoxelTypeId typeId) {
            this.typeId = typeId;
        }

        public override bool Equals(object obj) {
            return typeId.Equals(obj);
        }
        public override int GetHashCode() {
            return typeId.GetHashCode();
        }

        public override string ToString() {
            return typeId.ToString();
        }
    }
}