using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kutil;
using System.Linq;

namespace VoxelSystem {
    [System.Serializable]
    public class VoxelVolume : IEnumerable {

        [SerializeField]
        Vector3Int size = Vector3Int.zero;

        // todo sparse array?
        // jagged array should be faster
        // y, z, x order
        [SerializeField, ReadOnly] Voxel[][][] _voxels;

        public Voxel[][][] voxels { get => _voxels; protected set => _voxels = value; }
        public int width => size.x;
        public int length => size.z;
        public int height => size.y;
        public int volume => width * length * height;
        public int[] resolutions => new int[] { width, height, length };

        public VoxelVolume(Vector3Int size) {
            this.size = size;
            this.voxels = null;
        }

        // init

        public void PopulateWithNewVoxels() {
            // empty is id 0
            PopulateWithNewVoxels((VoxelMaterialId)0);
        }
        public void PopulateWithNewVoxels(VoxelMaterialId voxelMaterialId) {
            voxels = new Voxel[height][][];
            for (int y = 0; y < size.y; y++) {
                voxels[y] = new Voxel[length][];
                for (int z = 0; z < size.z; z++) {
                    voxels[y][z] = new Voxel[width];
                    for (int x = 0; x < size.x; x++) {
                        Voxel voxel = new Voxel(voxelMaterialId);
                        voxels[y][z][x] = voxel;
                    }
                }
            }
        }
        public void PopulateWithExistingVoxels(Voxel[] newVoxels) => PopulateWithExistingVoxels(ToJagged(newVoxels, size));
        public void PopulateWithExistingVoxels(Voxel[][][] newVoxels) {
            if (newVoxels.Length != volume) {
                Debug.LogError($"Cannot set voxels to voxel volume, wrong size {newVoxels.Length} vs {volume}");
                return;
            }
            voxels = newVoxels;
        }
        public void ClearAllVoxels() {
            this.voxels = null;
        }

        // get

        public bool HasVoxelAt(Vector3Int pos) {
            return voxels != null
                && pos.y >= 0 && pos.y < size.y
                && pos.z >= 0 && pos.z < size.z
                && pos.x >= 0 && pos.x < size.x
                ;
        }
        /// <summary>
        /// Returns the voxel at the specified position. must be in bounds or will throw exception
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        public Voxel GetVoxelAt(Vector3Int pos) {
            return voxels[pos.y][pos.z][pos.x];
        }
        public bool TryGetVoxelAt(Vector3Int pos, out Voxel voxel) {
            if (HasVoxelAt(pos)) {
                voxel = GetVoxelAt(pos);
                return true;
            }
            voxel = default;
            return false;
        }

        // set

        /// <summary>
        /// Set all voxels using a func
        /// </summary>
        /// <param name="setFunc">input pos and original voxel, outputs new voxel</param>
        public void SetVoxels(System.Func<Vector3Int, Voxel, Voxel> setFunc) {
            SetVoxels(new BoundsInt(Vector3Int.zero, size), setFunc);
        }
        /// <summary>
        /// Set voxels in an area using a func
        /// </summary>
        /// <param name="area">area to set voxels in. will be clamped to size</param>
        /// <param name="setFunc">input pos and original voxel, outputs new voxel</param>
        public void SetVoxels(BoundsInt area, System.Func<Vector3Int, Voxel, Voxel> setFunc) {
            area.min = Vector3Int.Max(area.min, Vector3Int.zero);
            area.max = Vector3Int.Min(area.max, size);
            for (int y = area.yMin; y < area.yMax; y++) {
                for (int z = area.zMin; z < area.zMax; z++) {
                    for (int x = area.xMin; x < area.xMax; x++) {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        Voxel voxel = voxels[y][z][x];
                        voxels[y][z][x] = setFunc(pos, voxel);
                    }
                }
            }
        }
        /// <summary>
        /// Set voxels in an area using a voxel type
        /// </summary>
        /// <param name="area">area to set voxels in. will be clamped to size</param>
        public void SetVoxels(BoundsInt area, Voxel newVoxel) {
            area.min = Vector3Int.Max(area.min, Vector3Int.zero);
            area.max = Vector3Int.Min(area.max, size);
            for (int y = area.yMin; y < area.yMax; y++) {
                for (int z = area.zMin; z < area.zMax; z++) {
                    for (int x = area.xMin; x < area.xMax; x++) {
                        Voxel voxel = voxels[y][z][x];
                        // ? copy voxel? its a struct so w/e
                        voxels[y][z][x] = newVoxel;
                    }
                }
            }
        }

        // util

        public IEnumerable<Voxel> ToFlatArray() {
            return voxels.SelectMany(v2 => v2.SelectMany(v1 => v1));
        }
        static Voxel[][][] ToJagged(Voxel[] flat, Vector3Int dimensions) {
            Voxel[][][] jagged = new Voxel[dimensions.y][][];
            for (int y = 0; y < dimensions.y; y++) {
                jagged[y] = new Voxel[dimensions.z][];
                for (int z = 0; z < dimensions.z; z++) {
                    jagged[y][z] = new Voxel[dimensions.x];
                    for (int x = 0; x < dimensions.x; x++) {
                        int i = x + z * dimensions.x + y * dimensions.x * dimensions.y;
                        jagged[y][z][x] = flat[i];
                    }
                }
            }
            return jagged;
        }

        public IEnumerator GetEnumerator() {
            return ToFlatArray().GetEnumerator();
        }
        public override bool Equals(object obj) {
            return voxels.Equals(obj);
        }
        public override int GetHashCode() {
            return voxels.GetHashCode();
        }
        public override string ToString() {
            return $"Voxel Volume {size}";
        }

        // todo saving and compression
    }
}