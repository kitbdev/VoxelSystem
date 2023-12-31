using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kutil;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;

namespace VoxelSystem {
    /// <summary>
    /// Holds voxels in a 3 dimensional volume
    /// </summary>
    [System.Serializable]
    public class VoxelVolume<VoxelT> : IVoxelVolume<VoxelT>, ISaveable where VoxelT : struct, IVoxel {

        [SerializeField]
        Vector3Int size;// = Vector3Int.zero;

        // todo sparse array?
        // jagged array should be faster
        // y, z, x order
        [SerializeField, ReadOnly] VoxelT[][][] _voxels;

        public VoxelT[][][] voxels { get => _voxels; private set => _voxels = value; }
        public int width => size.x;
        public int length => size.z;
        public int height => size.y;
        public int volume => width * length * height;
        public int[] resolutions => new int[] { width, height, length };
        public Vector3Int Size => size;




        public VoxelVolume(Vector3Int size) {
            this.size = size;
            _voxels = null;
        }

        public VoxelVolume() { }

        // init

        public void Init(Vector3Int newSize) {
            this.size = newSize;
            PopulateWithNewVoxels();
        }

        public void PopulateWithNewVoxels() {
            // empty is id 0
            PopulateWithNewVoxels(new VoxelMaterialIdVD(0));
        }
        public void PopulateWithNewVoxels(VoxelMaterialIdVD typeId) {
            voxels = new VoxelT[height][][];
            for (int y = 0; y < size.y; y++) {
                voxels[y] = new VoxelT[length][];
                for (int z = 0; z < size.z; z++) {
                    voxels[y][z] = new VoxelT[width];
                    for (int x = 0; x < size.x; x++) {
                        VoxelT vox = new VoxelT();
                        vox.Init(typeId);
                        voxels[y][z][x] = vox;
                    }
                }
            }
        }
        public void PopulateWithExistingVoxels(VoxelT[] newVoxels) => PopulateWithExistingVoxels(ToJagged(newVoxels, size));
        public void PopulateWithExistingVoxels(VoxelT[][][] newVoxels) {
            if (newVoxels.Length != volume) {
                Debug.LogError($"Cannot set voxels to VoxelT volume, wrong size {newVoxels.Length} vs {volume}");
                return;
            }
            voxels = newVoxels;
        }

        public void ClearAllVoxels() {
            this.voxels = null;
        }

        // get

        public bool HasVoxelAt(Vector3Int pos) {
            return voxels != null && IsInBounds(pos);
        }

        public bool IsInBounds(Vector3Int pos) {
            return pos.y >= 0 && pos.y < size.y
                && pos.z >= 0 && pos.z < size.z
                && pos.x >= 0 && pos.x < size.x;
        }

        /// <summary>
        /// Returns the VoxelT at the specified position. must be in bounds or will throw exception
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VoxelT GetVoxelAt(Vector3Int pos) {
            return voxels[pos.y][pos.z][pos.x];
        }
        /// <summary>
        /// Returns the VoxelT at the specified position. must be in bounds or will throw exception
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public VoxelT GetVoxelAt(int posx, int posy, int posz) {
            return voxels[posy][posz][posx];
        }
        public bool TryGetVoxelAt(Vector3Int pos, out VoxelT VoxelT) {
            if (HasVoxelAt(pos)) {
                VoxelT = GetVoxelAt(pos);
                return true;
            }
            VoxelT = default;
            return false;
        }

        public BoundsInt GetBounds() {
            return new BoundsInt(Vector3Int.zero, size);
        }
        public VoxelVolume<VoxelT> GetVoxelsInBounds(BoundsInt newBounds) {
            BoundsInt bounds = GetBounds();
            if (!BoundsIntExt.BoundsIntContains(bounds, newBounds)) {
                // if (!bounds.ContainsBounds(newBounds)) {
                Debug.LogError("Cannot GetVoxels in bounds, new bounds are too large! bounds:" + bounds + " new:" + newBounds);
                return default;
            }
            VoxelVolume<VoxelT> newVol = new VoxelVolume<VoxelT>(newBounds.size);
            for (int y = newBounds.yMin; y < newBounds.yMax; y++) {
                for (int z = newBounds.zMin; z < newBounds.zMax; z++) {
                    for (int x = newBounds.xMin; x < newBounds.xMax; x++) {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        Vector3Int newPos = pos - newBounds.min;
                        newVol.SetVoxel(newPos, GetVoxelAt(pos));
                    }
                }
            }
            return newVol;
        }

        // set

        /// <summary>
        /// Set a voxel to a new value
        /// </summary>
        /// <param name="setFunc">input pos and original VoxelT, outputs new VoxelT</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVoxel(Vector3Int pos, VoxelT newVoxel) {
            voxels[pos.y][pos.z][pos.x] = newVoxel;
        }
        /// <summary>
        /// Set a voxel to a new value
        /// </summary>
        /// <param name="setFunc">input pos and original VoxelT, outputs new VoxelT</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVoxel(int posx, int posy, int posz, VoxelT newVoxel) {
            voxels[posy][posz][posx] = newVoxel;
        }
        /// <summary>
        /// Set voxels in an area using a func
        /// </summary>
        /// <param name="area">area to set voxels in. will be clamped to size</param>
        /// <param name="setFunc">input pos and original VoxelT, outputs new VoxelT</param>
        public void SetVoxels(BoundsInt area, System.Func<Vector3Int, VoxelT, VoxelT> setFunc) {
            area.min = Vector3Int.Max(area.min, Vector3Int.zero);
            area.max = Vector3Int.Min(area.max, size);
            for (int y = area.yMin; y < area.yMax; y++) {
                for (int z = area.zMin; z < area.zMax; z++) {
                    for (int x = area.xMin; x < area.xMax; x++) {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        VoxelT VoxelT = voxels[y][z][x];
                        voxels[y][z][x] = setFunc(pos, VoxelT);
                    }
                }
            }
        }
        /// <summary>
        /// Set voxels in an area using a VoxelT type
        /// </summary>
        /// <param name="area">area to set voxels in. will be clamped to size</param>
        public void SetVoxels(BoundsInt area, VoxelT newVoxel) {
            area.min = Vector3Int.Max(area.min, Vector3Int.zero);
            area.max = Vector3Int.Min(area.max, size);
            for (int y = area.yMin; y < area.yMax; y++) {
                for (int z = area.zMin; z < area.zMax; z++) {
                    for (int x = area.xMin; x < area.xMax; x++) {
                        // VoxelT VoxelT = voxels[y][z][x];
                        // dont need to make a copy cause its a struct
                        voxels[y][z][x] = newVoxel;
                    }
                }
            }
        }

        /// <summary>
        /// Set voxels in an area using another voxelVolume
        /// </summary>
        /// <param name="startOffset">area to set voxels in. will be clamped to size</param>
        public void SetVoxels(Vector3Int startOffset, IVoxelVolume<VoxelT> fromVoxels) {
            startOffset = Vector3Int.Max(startOffset, Vector3Int.zero);
            Vector3Int maxBound = startOffset + fromVoxels.Size;
            maxBound = Vector3Int.Min(maxBound, size);
            for (int y = startOffset.y; y < maxBound.y; y++) {
                for (int z = startOffset.z; z < maxBound.z; z++) {
                    for (int x = startOffset.x; x < maxBound.x; x++) {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        Vector3Int fromPos = pos - startOffset;
                        SetVoxel(pos, fromVoxels.GetVoxelAt(fromPos));
                    }
                }
            }
        }

        public void FinishUpdating() {
            // nothing
            // maybe something with dirty flag?
        }

        // util

        public IEnumerable<VoxelT> ToFlatArray() {
            return voxels.SelectMany(v2 => v2.SelectMany(v1 => v1));
        }
        static VoxelT[][][] ToJagged(VoxelT[] flat, Vector3Int dimensions) {
            VoxelT[][][] jagged = new VoxelT[dimensions.y][][];
            for (int y = 0; y < dimensions.y; y++) {
                jagged[y] = new VoxelT[dimensions.z][];
                for (int z = 0; z < dimensions.z; z++) {
                    jagged[y][z] = new VoxelT[dimensions.x];
                    for (int x = 0; x < dimensions.x; x++) {
                        int i = x + z * dimensions.x + y * dimensions.x * dimensions.y;
                        jagged[y][z][x] = flat[i];
                    }
                }
            }
            return jagged;
        }
        // todo saving and compression

        // save

        public string GetName() => $"VoxelVolumeJagged<{new VoxelT().GetName()}>";
        public string GetVersion() => "0.1";
        public void Save(Stream writer) {// todo return false if failed? or throw
            writer.Write(size);
            // writer.BeginWrite();//todo async 
            // writer.EndWrite();
            for (int y = 0; y < size.y; y++) {
                for (int z = 0; z < size.z; z++) {
                    for (int x = 0; x < size.x; x++) {
                        GetVoxelAt(x, y, z).Save(writer);
                    }
                }
            }
        }
        public void Load(Stream reader) {
            // todo test
            ClearAllVoxels();
            size = reader.ReadV3i();
            for (int y = 0; y < size.y; y++) {
                for (int z = 0; z < size.z; z++) {
                    for (int x = 0; x < size.x; x++) {
                        VoxelT vox = new VoxelT();
                        vox.Load(reader);
                        SetVoxel(x, y, z, vox);
                    }
                }
            }
        }

        // misc

        public IEnumerator GetEnumerator() {
            return ToFlatArray().GetEnumerator();
        }
        public IEnumerable<FullVoxel> GetFullVoxelEnumerable() {
            var sz = Size;
            return ToFlatArray().Select((v, i) => new FullVoxel(v, VoxelVolumeFlat.GetLocalPos(i, sz)));
        }
        public override bool Equals(object obj) {
            return voxels.Equals(obj);
        }
        public override int GetHashCode() {
            return voxels.GetHashCode();
        }
        public override string ToString() {
            return $"VoxelT Volume {size}";
        }
    }
}