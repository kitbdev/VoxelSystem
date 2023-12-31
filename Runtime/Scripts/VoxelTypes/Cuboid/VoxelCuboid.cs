using System.IO;

namespace VoxelSystem {
    /// <summary>
    /// Stores instance information for Cuboid voxels
    /// </summary>
    /// [System.Serializable]
    public struct VoxelCuboid : IVoxel {
        public VoxelMaterialIdVD typeId;
        // ?
        // public GameObject conencted;// todo also implement idisposable to delete this on destroy?
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

        public VoxelCuboid(VoxelMaterialIdVD typeId) {
            this.typeId = typeId;
        }
        public void Init(VoxelMaterialIdVD typeId) {
            this.typeId = typeId;
        }
        public bool IsEmpty() => typeId.Equals(0);

        public string GetName() => "VoxelCuboid";
        public string GetVersion() => "0.1";

        public void Save(Stream writer) {
            typeId.Save(writer);
            // todo save dir
        }

        public void Load(Stream reader) {
            typeId.Save(reader);
        }


        public bool Equals(IVoxel other) {
            if (other is VoxelCuboid v) {
                // todo
                return typeId.Equals(v.typeId);
            }
            return false;
        }
        public override bool Equals(object obj) {
            if (obj is VoxelCuboid v) {
                return typeId.Equals(v.typeId);
            }
            return false;
        }
        public override int GetHashCode() {
            return typeId.GetHashCode();
        }
        public override string ToString() {
            return typeId.ToString();
        }
    }
}