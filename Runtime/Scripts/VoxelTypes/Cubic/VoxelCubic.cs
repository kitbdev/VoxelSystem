using System.IO;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Stores instance information for Cubic voxels
    /// </summary>
    [System.Serializable]
    public struct VoxelCubic : IVoxel {
        public VoxelMaterialIdVD typeId;

        public VoxelCubic(VoxelMaterialIdVD typeId) {
            this.typeId = typeId;
        }
        public void Init(VoxelMaterialIdVD typeId) {
            this.typeId = typeId;
        }

        public bool IsEmpty() => typeId.Equals(0);

        public string GetName() => "VoxelCubic";
        public string GetVersion() => "0.1";

        public void Save(Stream writer) {
            typeId.Save(writer);
        }

        public void Load(Stream reader) {
            typeId.Save(reader);
        }

        public bool Equals(IVoxel other) {
            if (other is VoxelCubic v) {
                return typeId.Equals(v.typeId);
            }
            return false;
        }
        public override bool Equals(object obj) {
            if (obj is VoxelCubic v) {
                return typeId.Equals(v.typeId);
            }
            return false;
        }
        public override int GetHashCode() {
            // shouldnt ever be put in same container as another type
            return typeId.GetHashCode();
        }

        public override string ToString() {
            return typeId.ToString();
        }
    }
}