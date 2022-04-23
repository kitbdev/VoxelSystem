using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kutil;

namespace VoxelSystem {
    // like FVoxelData
    // https://github.dev/Phyronnaz/VoxelPlugin/blob/cd331027eff6bee027101af355408c165d07d1b8/Source/Voxel/Public/VoxelData/VoxelData.h#L369
    /// <summary>
    /// Holds voxels in a sparse Octtree
    /// </summary>
    /// <typeparam name="VoxelT"></typeparam>
    public class VoxelOctree<VoxelT> where VoxelT : struct, IVoxel {
        public Octree<VoxelT> octree;
        // todo
        // ? write lock mutex

        /// ? world space
        public BoundsInt bounds;
        public int depth;
        // ? generator ref
        // public bool enableUndoRedo;// todo


        /// <summary>
        /// Must NOT be locked. Will delete the entire octree & recreate one.
        /// Destroys all items
        /// </summary>
        void ClearAllData() {
            octree.Clear();
        }

        #region getters

        public bool HasVoxelAt(Vector3Int pos) {
            return octree != null && IsInBounds(pos);
        }

        public bool IsInBounds(Vector3Int pos) {
            return pos.y >= bounds.yMin && pos.y <= bounds.yMax
                && pos.z >= bounds.zMin && pos.z <= bounds.zMax
                && pos.x >= bounds.xMin && pos.x <= bounds.xMax;
        }

        public VoxelT GetVoxelAt(Vector3Int pos, int lod = 0) {// does lod make sense here?
            // return voxels[pos.y][pos.z][pos.x];
            return default;
        }
        VoxelVolume<VoxelT> GetAllVoxels(int lod = 0) {
            return GetVoxelsInBounds(bounds, lod);
        }
        VoxelVolume<VoxelT> GetVoxelsInBounds(BoundsInt bounds, int lod) {
            return default;
        }
        // todo get tree node?

        // todo lod stuff

        #endregion
        // todo
        // setters
        // save, load
        // undo?
        // convert to voxel volume
    }
}