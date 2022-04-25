using System.IO;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Stores instance information for Cubic voxels
    /// </summary>
    [System.Serializable]
    public struct VoxelCubic : IVoxel {
        public VoxelTypeIdVoxelData typeId;

        public VoxelCubic(VoxelTypeIdVoxelData typeId) {
            this.typeId = typeId;
        }
        public void Init(VoxelTypeIdVoxelData typeId) {
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

        public string GetName() => "VoxelCubic";
        public string GetVersion() => "0.1";

        public void Save(Stream writer) {
            typeId.Save(writer);
        }

        public void Load(Stream reader) {
            typeId.Save(reader);
        }
    }
}