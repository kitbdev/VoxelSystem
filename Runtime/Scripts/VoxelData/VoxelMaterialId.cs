using UnityEngine;

namespace VoxelSystem {
    //todo VoxelTypeId instead?
    /// <summary>
    /// Refers to a voxel type
    /// </summary>
    [System.Serializable]
    public struct VoxelMaterialId {
        // ? System.UInt16
        public int matId;

        public VoxelMaterialId(int materialId) {
            this.matId = materialId;
        }

        public override bool Equals(object obj) {
            return matId.Equals(obj);
        }
        public override int GetHashCode() {
            return matId.GetHashCode();
        }
        public override string ToString() {
            return matId.ToString();
        }

        public static implicit operator VoxelMaterialId(int id) => new VoxelMaterialId(id);
        public static implicit operator int(VoxelMaterialId vMatId) => vMatId.matId;
    }
}