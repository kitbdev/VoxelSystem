namespace VoxelSystem {
    /// <summary>
    /// Stores instance information for Cuboid voxels
    /// </summary>
    [System.Serializable]
    public struct VoxelCuboid : IVoxel {
        public VoxelTypeId typeId;
        // ?
        // public GameObject conencted;
        //  public VoxelDirFlag facingDir;
        // todo also have rotation direction?
        public enum VoxelDirFlag : byte {
            XMin = 0x01,
            XMax = 0x02,
            YMin = 0x04,
            YMax = 0x08,
            ZMin = 0x10,
            ZMax = 0x20
        }

        public VoxelCuboid(VoxelTypeId typeId) {
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