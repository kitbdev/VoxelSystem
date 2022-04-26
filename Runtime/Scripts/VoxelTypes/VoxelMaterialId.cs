
using System.IO;

namespace VoxelSystem {
    // todo split editor data class from regular class?
    //  as editor needs to keep track of typeholderSO, but voxels dont need that individually
    // ! also this way I could use string (for names) to better keep track of types between changes
    /// <summary>
    /// Refers to a voxel type. better for general uses
    /// </summary>
    [System.Serializable]
    public class VoxelMaterialId {
        public string idName;
        // public int id;
        // VoxelTypeIdVoxelData vtvId;// todo
        // [SerializeField]
        public VoxelTypeHolder typeHolder;

        public VoxelMaterialId(string idName) {
            this.idName = idName;
        }

        public bool IsValid() {
            return idName != null && idName != "";
        }

        bool Equals(VoxelMaterialId other) {
            if (!other.IsValid() && !IsValid()) return true;
            if (!other.IsValid() || !IsValid()) return false;
            return idName == other.idName;
        }
        public override bool Equals(object obj) {
            if (obj == null) return false;
            if (obj is VoxelMaterialId other) {
                return Equals(other);
            }
            return idName?.Equals(obj) ?? false;
        }
        public override int GetHashCode() {
            return idName?.GetHashCode() ?? -1;
        }
        public override string ToString() {
            return "VTID:" + (idName?.ToString() ?? "?");
        }

        public static bool operator ==(VoxelMaterialId a, VoxelMaterialId b) => a.Equals(b);
        public static bool operator !=(VoxelMaterialId a, VoxelMaterialId b) => !(a == b);

        // public static implicit operator VoxelTypeId(int id) => new VoxelTypeId(id);
        // public static implicit operator int(VoxelTypeId vMatId) => vMatId.id;

        public static VoxelMaterialId INVALID = new VoxelMaterialId(null);
        public static VoxelMaterialId EMPTY = new VoxelMaterialId("empty");
    }
    /// <summary>
    /// Refers to a voxel type. stored in voxels.
    /// </summary>
    [System.Serializable]
    public struct VoxelMaterialIdVD : ISaveable {
        // ? System.UInt16 ? byte ? string??
        //? maybe multiple values?
        public ushort id;//=System.UInt16

        public VoxelMaterialIdVD(int id) {
            this.id = (ushort)id;
        }

        public bool IsValid() {
            return id >= 0;
        }

        bool Equals(VoxelMaterialIdVD other) {
            if (!other.IsValid() && !IsValid()) return true;
            if (!other.IsValid() || !IsValid()) return false;
            return id == other.id;
        }
        public override bool Equals(object obj) {
            if (obj is VoxelMaterialIdVD other) {
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

        public string GetName() => "VoxelTypeIdVD";

        public string GetVersion() => "0.1";

        public void Save(Stream writer) {
            writer.Write(System.BitConverter.GetBytes(id));
        }
        public void Load(Stream reader) {
            byte[] buffer = new byte[sizeof(ushort)];
            reader.Read(buffer);
            id = System.BitConverter.ToUInt16(buffer);
        }

        public static bool operator ==(VoxelMaterialIdVD a, VoxelMaterialIdVD b) => a.Equals(b);
        public static bool operator !=(VoxelMaterialIdVD a, VoxelMaterialIdVD b) => !(a == b);

        // public static implicit operator VoxelTypeIdVoxelData(int id) => new VoxelTypeIdVoxelData(id);
        // public static implicit operator int(VoxelTypeIdVoxelData vMatId) => vMatId.id;

        public static VoxelMaterialIdVD INVALID = new VoxelMaterialIdVD(-1);
    }
}