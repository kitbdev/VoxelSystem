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

        Octree<VoxelT> octree;
        // todo
        // ? write lock mutex

        /// ? world space
        public BoundsInt bounds;
        public int depth;
        // ? generator ref

        // public bool enableUndoRedo;// todo
        bool isDirty = false;


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

        #region Setters
        // setters
        // todo


        #endregion


        #region Save Load
        // save, load
        // todo

        public struct FVoxelUncompressedWorldSaveImpl { }
        /// <summary>
        /// Get a save of this world. No lock required
        /// </summary>
        /// <param name="outSave">out the finished save data</param>
        /// <param name="outObjects">out ?</param>//FVoxelObjectArchiveEntry
        void GetSave(out FVoxelUncompressedWorldSaveImpl outSave, out List<GameObject> outObjects) {
            outSave = default;
            outObjects = default;
        }

        struct LoadInfo { }//FVoxelPlaceableItemLoadInfo
         /// <summary>
         /// Load this world from save. No lock required
         /// </summary>
         /// <param name="saveData">Save to load from</param>
         /// <param name="loadInfo">Used to load placeable items. Can use {}</param>
         /// <param name="outBoundsToUpdate">The modified bounds</param>
         /// <returns>true if loaded successfully, false if the world is corrupted and must not be saved again</returns>
        bool LoadFromSave(FVoxelUncompressedWorldSaveImpl saveData, LoadInfo loadInfo, List<BoundsInt> outBoundsToUpdate = null) {
            return false;
        }

        #endregion

        #region Undo Redo
        // undo?
        // todo

        // Dirty state: can use that to track if the data is dirty
        // MarkAsDirty is called on Undo, Redo, SaveFrame and ClearData
        bool IsDirty() => isDirty;
        void MarkAsDirty() => isDirty = true;
        void ClearDirtyFlag() => isDirty = false;

        #endregion
        // convert to voxel volume
    }
}