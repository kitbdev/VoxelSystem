using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kutil;
using Unity.Mathematics;
using System.Linq;

namespace VoxelSystem {
    [System.Serializable]
    class VoxelVolumeFlat {

        [SerializeField]
        Vector3Int size = Vector3Int.zero;

        // todo sparse array?
        // todo? jagged array is faster
        [SerializeField, ReadOnly] IVoxel[] _voxels;

        public IVoxel[] voxels { get => _voxels; protected set => _voxels = value; }
        public int width => size.x;
        public int length => size.z;
        public int height => size.y;
        public int volume => width * length * height;
        public int[] resolutions => new int[] { width, height, length };

        public VoxelVolumeFlat(Vector3Int size) {
            this.size = size;
            this.voxels = new IVoxel[0];
        }

        // init

        // public void PopulateWithNewVoxels() {
        //     // empty is id 0
        //     PopulateWithNewVoxels((VoxelTypeId)0);
        // }
        // todo
        // public void PopulateWithNewVoxels(VoxelTypeId VoxelTypeId) {
        //     voxels = new IVoxel[volume];
        //     for (int i = 0; i < volume; i++) {
        //         IVoxel IVoxel = new IVoxel(VoxelTypeId);
        //         voxels[i] = IVoxel;
        //     }
        // }
        // public void PopulateWithNewVoxels(VoxelTypeId[] voxelMaterialIds) {
        //     if (voxelMaterialIds.Length != volume) return;
        //     voxels = new IVoxel[volume];
        //     for (int i = 0; i < volume; i++) {
        //         IVoxel IVoxel = new IVoxel(voxelMaterialIds[i]);
        //         voxels[i] = IVoxel;
        //     }
        // }
        public void PopulateWithExistingVoxels(IVoxel[] newVoxels) {
            if (newVoxels.Length != volume) {
                Debug.LogError($"Cannot set voxels to IVoxel volume, wrong size {newVoxels.Length} vs {volume}");
                return;
            }
            voxels = newVoxels;
        }
        public void ClearAllVoxels() {
            this.voxels = new IVoxel[0];
        }

        // get

        public bool HasVoxelAt(Vector3Int pos) {
            return IndexAt(pos) >= 0;
        }
        public IVoxel GetVoxelAt(int index) {
            if (index < 0 || index >= voxels.Length) {
                // invalid index
                return default;
            } else {
                return voxels[index];
            }
        }
        public IVoxel GetVoxelAt(Vector3Int pos) {
            return GetVoxelAt(IndexAt(pos));
        }
        public int IndexAt(Vector3Int pos) {
            return IndexAt(pos, width, height, length);
        }
        public Vector3Int GetLocalPos(int index) {
            return GetLocalPos(index, width, height, length);
        }

        // set

        public void SetVoxels(BoundsInt area, System.Func<Vector3Int, IVoxel, IVoxel> setFunc) {
            area.min = Vector3Int.Max(area.min, Vector3Int.zero);
            area.max = Vector3Int.Min(area.max, size);
            for (int y = area.yMin; y < area.yMax; y++) {
                for (int z = area.zMin; z < area.zMax; z++) {
                    for (int x = area.xMin; x < area.xMax; x++) {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        int i = IndexAt(pos);
                        IVoxel IVoxel = voxels[i];
                        voxels[i] = setFunc(pos, IVoxel);
                    }
                }
            }
        }


        // util

        public static int IndexAt(Vector3Int pos, int resolution) {
            return IndexAt(pos, resolution, resolution, resolution);
        }
        public static int IndexAt(Vector3Int pos, int xResolution, int yResolution, int zResolution) {
            if (pos.x < 0 || pos.x >= xResolution || pos.y < 0 || pos.y >= yResolution || pos.z < 0 || pos.z >= zResolution)
                return -1;
            // in x -> z -> y order
            return pos.x + pos.z * xResolution + pos.y * xResolution * zResolution;
        }
        public static Vector3Int GetLocalPos(int index, int resolution) {
            // assumes all resolutions are the same size
            return GetLocalPos(index, resolution, resolution, resolution);
        }
        public static Vector3Int GetLocalPos(int index, int xResolution, int yResolution, int zResolution) {
            // technically y resolution isnt needed
            Vector3Int pos = Vector3Int.zero;
            // todo test
            pos.y = index / (xResolution * zResolution);
            index -= (pos.y * xResolution * zResolution);
            pos.z = index / xResolution;
            pos.x = index % xResolution;
            return pos;
        }
    }
}