using System.IO;

namespace VoxelSystem {
    /// <summary>
    /// Stores instance information for Density voxels (smooth)
    /// </summary>
    /// [System.Serializable]
    public struct VoxelDensity : IVoxel {
        public VoxelMaterialIdVD typeId;
        public float density;

        public VoxelDensity(VoxelMaterialIdVD typeId) {
            this.typeId = typeId;
            this.density = 0f;
        }
        public void Init(VoxelMaterialIdVD typeId) {
            this.typeId = typeId;
        }

        public bool IsEmpty() => typeId.Equals(0);


        public string GetName() => "VoxelDensity";
        public string GetVersion() => "0.1";

        public void Save(Stream writer) {
            typeId.Save(writer);
            writer.Write(System.BitConverter.GetBytes(density));
        }

        public void Load(Stream reader) {
            typeId.Save(reader);
            byte[] buffer = new byte[sizeof(float)];
            reader.Read(buffer);
            density = System.BitConverter.ToSingle(buffer);
        }


        public bool Equals(IVoxel other) {
            if (other is VoxelDensity v) {
                return typeId.Equals(v.typeId) && density.Equals(v.density);
            }
            return false;
        }
        public override bool Equals(object obj) {
            if (obj is VoxelDensity v) {
                return typeId.Equals(v.typeId) && density.Equals(v.density);;
            }
            return false;
        }
        public override int GetHashCode() {
            // todo better hash
            return typeId.GetHashCode() + 4231 * density.GetHashCode();
        }
        public override string ToString() {
            return typeId.ToString();
        }
    }
}