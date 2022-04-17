using UnityEngine;

namespace VoxelSystem {
    // [System.Serializable]
    // public struct Voxel<TVal>
    //     where TVal : struct {
    //     public TVal value;
    // }
    /// <summary>
    /// Holds more context than a voxel, is a class rather than a struct
    /// </summary>
    public class FullVoxel {
        public Voxel voxel;
        public Vector3Int pos;
        // ? chunkgo? other info?

    }
    /// <summary>
    /// Voxel data struct
    /// </summary>
    [System.Serializable]
    public struct Voxel {
        public VoxelMaterialId id;
        // todo also have rotation direction?
        public enum VoxelDirFlag : byte {
            XMin = 0x01,
            XMax = 0x02,
            YMin = 0x04,
            YMax = 0x08,
            ZMin = 0x10,
            ZMax = 0x20
        }
        // todo value for marching cubes?

        public Voxel(VoxelMaterialId id) {
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
    }
}