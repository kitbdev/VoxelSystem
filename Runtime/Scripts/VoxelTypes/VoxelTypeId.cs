
namespace VoxelSystem {
    // todo split editor data class from regular class?
    // todo as editor needs to keep track of typeholderSO, but voxels dont need that individually
    // ! also this way I could use string (for names) to better keep track of types between changes
    /// <summary>
    /// Refers to a voxel type. better for general uses
    /// </summary>
    [System.Serializable]
    public class VoxelTypeId {
        // public string id;
        public int id;
        // [SerializeField]
        public VoxelTypeHolder typeHolder;
        
        public VoxelTypeId(int id) {
            this.id = id;
        }

        public bool IsValid() {
            return id >= 0;
        }

        bool Equals(VoxelTypeId other) {
            if (!other.IsValid() && !IsValid()) return true;
            if (!other.IsValid() || !IsValid()) return false;
            return id == other.id;
        }
        public override bool Equals(object obj) {
            if (obj is VoxelTypeId other) {
                return Equals(other);
            }
            return id.Equals(obj);
        }
        public override int GetHashCode() {
            return id.GetHashCode();
        }
        public override string ToString() {
            return "VTID:" + id.ToString();
        }

        public static bool operator ==(VoxelTypeId a, VoxelTypeId b) => a.Equals(b);
        public static bool operator !=(VoxelTypeId a, VoxelTypeId b) => !(a == b);

        // public static implicit operator VoxelTypeId(int id) => new VoxelTypeId(id);
        // public static implicit operator int(VoxelTypeId vMatId) => vMatId.id;

        public static VoxelTypeId INVALID = new VoxelTypeId(-1);
    }
    // /// <summary>
    // /// Refers to a voxel type. stored in voxels.
    // /// </summary>
    // [System.Serializable]
    // public struct VoxelTypeIdVoxelData {
    //     // ? System.UInt16 ? byte ? string??
    //     //? maybe multiple values?
    //     public int id;

    //     public VoxelTypeIdVoxelData(int id) {
    //         this.id = id;
    //     }

    //     public bool IsValid() {
    //         return id >= 0;
    //     }

    //     bool Equals(VoxelTypeIdVoxelData other) {
    //         if (!other.IsValid() && !IsValid()) return true;
    //         if (!other.IsValid() || !IsValid()) return false;
    //         return id == other.id;
    //     }
    //     public override bool Equals(object obj) {
    //         if (obj is VoxelTypeIdVoxelData other) {
    //             return Equals(other);
    //         }
    //         return id.Equals(obj);
    //     }
    //     public override int GetHashCode() {
    //         return id.GetHashCode();
    //     }
    //     public override string ToString() {
    //         return "VTID:" + id.ToString();
    //     }

    //     public static bool operator ==(VoxelTypeIdVoxelData a, VoxelTypeIdVoxelData b) => a.Equals(b);
    //     public static bool operator !=(VoxelTypeIdVoxelData a, VoxelTypeIdVoxelData b) => !(a == b);

    //     public static implicit operator VoxelTypeIdVoxelData(int id) => new VoxelTypeIdVoxelData(id);
    //     public static implicit operator int(VoxelTypeIdVoxelData vMatId) => vMatId.id;

    //     public static VoxelTypeIdVoxelData INVALID = new VoxelTypeIdVoxelData(-1);
    // }
}