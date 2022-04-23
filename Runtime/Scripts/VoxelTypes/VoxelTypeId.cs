
namespace VoxelSystem {
    /// <summary>
    /// Refers to a voxel type
    /// </summary>
    [System.Serializable]
    public struct VoxelTypeId {
        // todo editor
        // ? System.UInt16 ? byte ? string??
        //? maybe multiple values?
        public int id;

        public VoxelTypeId(int id) {
            this.id = id;
        }

        public override bool Equals(object obj) {
            return id.Equals(obj);
        }
        public override int GetHashCode() {
            return id.GetHashCode();
        }
        public override string ToString() {
            return id.ToString();
        }

        public static implicit operator VoxelTypeId(int id) => new VoxelTypeId(id);
        public static implicit operator int(VoxelTypeId vMatId) => vMatId.id;
    }
}