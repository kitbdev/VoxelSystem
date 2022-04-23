namespace VoxelSystem {
    /// <summary>
    /// Stores instance information for Density voxels (smooth)
    /// </summary>
    [System.Serializable]
    public struct VoxelDensity : IVoxel {
        public VoxelTypeIdVoxelData typeId;
        public float density;

        public VoxelDensity(VoxelTypeIdVoxelData typeId) {
            this.typeId = typeId;
            this.density = 0f;
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
    }
}