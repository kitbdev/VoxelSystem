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
    interface IVoxelVolume<VoxelT> : IEnumerable where VoxelT : struct, IVoxel {


        Vector3Int Size { get; }
        BoundsInt GetBounds() {
            return new BoundsInt(Vector3Int.zero, Size);
        }


        void ClearAllVoxels();

        // get

        bool HasVoxelAt(Vector3Int pos);

        bool IsInBounds(Vector3Int pos) {
            return GetBounds().Contains(pos);
            // return pos.y >= 0 && pos.y < Size.y
            //     && pos.z >= 0 && pos.z < Size.z
            //     && pos.x >= 0 && pos.x < Size.x;
        }

        /// <summary>
        /// Returns the Voxel at the specified position. must be in bounds or will throw exception
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        VoxelT GetVoxelAt(Vector3Int pos);
        /// <summary>
        /// Returns the Voxel at the specified position. must be in bounds or will throw exception
        /// </summary>
        /// <param name="pos"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        VoxelT GetVoxelAt(int posx, int posy, int posz) {
            return GetVoxelAt(new Vector3Int(posx, posy, posz));
        }
        /// <summary>
        /// returns the voxel if valid pos
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="VoxelT"></param>
        /// <returns></returns>
        bool TryGetVoxelAt(Vector3Int pos, out VoxelT VoxelT) {
            if (HasVoxelAt(pos)) {
                VoxelT = GetVoxelAt(pos);
                return true;
            }
            VoxelT = default;
            return false;
        }


        VoxelVolume<VoxelT> GetAllVoxels() {
            return GetVoxelsInBounds(GetBounds());
        }

        /// <summary>
        /// Gets all the voxels within some bounds
        /// </summary>
        /// <param name="newBounds"></param>
        /// <returns></returns>
        VoxelVolume<VoxelT> GetVoxelsInBounds(BoundsInt newBounds) {
            BoundsInt myBounds = GetBounds();
            if (!BoundsIntExt.BoundsIntContains(myBounds, newBounds)) {
                // if (!bounds.ContainsBounds(newBounds)) {
                Debug.LogError("Cannot GetVoxels in bounds, new bounds are too large! bounds:" + myBounds + " new:" + newBounds);
                return default;
            }
            VoxelVolume<VoxelT> newVol = new VoxelVolume<VoxelT>(newBounds.size);
            for (int y = newBounds.yMin; y < newBounds.yMax; y++) {
                for (int z = newBounds.zMin; z < newBounds.zMax; z++) {
                    for (int x = newBounds.xMin; x < newBounds.xMax; x++) {
                        Vector3Int pos = new Vector3Int(x, y, z);
                        // todo may need to offset by myBounds.min ?
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
        /// <param name="newVoxel">value to set voxel to</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        void SetVoxel(Vector3Int pos, VoxelT newVoxel);

        /// <summary>
        /// Set all voxels to a new value
        /// </summary>
        /// <param name="newVoxel">value to set voxels to</param>
        /// <param name="setFunc">input pos and original VoxelT, outputs new VoxelT</param>
        void SetAllVoxels(VoxelT newVoxel) {
            SetVoxels(GetBounds(), newVoxel);
        }
        /// <summary>
        /// Set all voxels using a func
        /// </summary>
        /// <param name="setFunc">input pos and original VoxelT, outputs new VoxelT</param>
        void SetAllVoxels(System.Func<Vector3Int, VoxelT, VoxelT> setFunc) {
            SetVoxels(GetBounds(), setFunc);
        }
        /// <summary>
        /// Set voxels in an area using a func
        /// </summary>
        /// <param name="area">area to set voxels in. will be clamped to size</param>
        /// <param name="setFunc">input pos and original VoxelT, outputs new VoxelT</param>
        void SetVoxels(BoundsInt area, System.Func<Vector3Int, VoxelT, VoxelT> setFunc);
        /// <summary>
        /// Set voxels in an area to a Voxel
        /// </summary>
        /// <param name="area">area to set voxels in. will be clamped to size</param>
        /// <param name="newVoxel">value to set voxels to</param>
        void SetVoxels(BoundsInt area, VoxelT newVoxel) {
            area.min = Vector3Int.Max(area.min, Vector3Int.zero);
            area.max = Vector3Int.Min(area.max, Size);
            for (int y = area.yMin; y < area.yMax; y++) {
                for (int z = area.zMin; z < area.zMax; z++) {
                    for (int x = area.xMin; x < area.xMax; x++) {
                        // dont need to make a copy cause its a struct
                        SetVoxel(new Vector3Int(x, y, z), newVoxel);
                    }
                }
            }
        }
        /// <summary>
        /// Set voxels in all specified positions to a Voxel
        /// </summary>
        /// <param name="positions">all positions to set</param>
        /// <param name="newVoxel">value to set voxels to</param>
        void SetVoxels(IEnumerable<Vector3Int> positions, VoxelT newVoxel) {
            foreach (var pos in positions) {
                SetVoxel(pos, newVoxel);
            }
        }
        /// <summary>
        /// Set voxels in all specified positions using a func
        /// </summary>
        /// <param name="positions">all positions to set</param>
        /// <param name="newVoxel">value to set voxels to</param>
        void SetVoxels(IEnumerable<Vector3Int> positions, System.Func<Vector3Int, VoxelT, VoxelT> setFunc) {
            foreach (var pos in positions) {
                SetVoxel(pos, setFunc(pos, GetVoxelAt(pos)));
            }
        }
        /// <summary>
        /// Set voxels in an area using a voxelVolume
        /// </summary>
        /// <param name="startOffset">area to set voxels in. will be clamped to size</param>
        void SetVoxels(Vector3Int startOffset, VoxelVolume<VoxelT> fromVoxels);

        /// <summary>
        /// Finish setting voxels for now
        /// </summary>
        void FinishUpdating();
        // util
        // conversions?

        // todo saving and compression
    }
}