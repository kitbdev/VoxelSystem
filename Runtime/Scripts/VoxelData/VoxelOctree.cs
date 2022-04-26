using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Kutil;
using System;

namespace VoxelSystem {
    // like FVoxelData
    // https://github.dev/Phyronnaz/VoxelPlugin/blob/cd331027eff6bee027101af355408c165d07d1b8/Source/Voxel/Public/VoxelData/VoxelData.h#L369
    /// <summary>
    /// Holds voxels in a sparse Octtree
    /// </summary>
    /// <typeparam name="VoxelT"></typeparam>
    public class VoxelOctree<VoxelT> : IVoxelVolume<VoxelT> where VoxelT : struct, IVoxel {

        Octree<VoxelT> octree;
        // todo
        // ? write lock mutex

        /// ? world space
        BoundsInt bounds;
        public int depth;
        // ? generator ref

        // public bool enableUndoRedo;// todo
        bool isDirty = false;

        public Vector3Int Size => bounds.size;
        public BoundsInt GetBounds() => bounds;


        public void Init(Vector3Int newSize) {
            // todo
        }

        /// <summary>
        /// Must NOT be locked. Will delete the entire octree & recreate one.
        /// Destroys all items
        /// </summary>
        public void ClearAllVoxels() {
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

        public VoxelT GetVoxelAt(Vector3Int pos) {
            return GetVoxelAt(pos, 0);
        }
        public VoxelT GetVoxelAt(Vector3Int pos, int lod) {// does lod make sense here?
            // todo
            return default;
        }
        public VoxelVolume<VoxelT> GetAllVoxels(int lod = 0) {
            return GetVoxelsInBounds(bounds, lod);
        }
        public VoxelVolume<VoxelT> GetVoxelsInBounds(BoundsInt bounds) => GetVoxelsInBounds(bounds, 0);
        public VoxelVolume<VoxelT> GetVoxelsInBounds(BoundsInt bounds, int lod) {
            return default;
        }

        public IEnumerator GetEnumerator() {
            throw new NotImplementedException();
        }
        public IEnumerable<FullVoxel> GetFullVoxelEnumerable() {
            throw new NotImplementedException();
            // return voxelDict.Select(kvp => new FullVoxel(kvp.Value, kvp.Key));
        }
        // todo get tree node?

        // todo lod stuff

        #endregion

        #region Setters
        // setters
        // todo

        public void SetVoxel(Vector3Int pos, VoxelT newVoxel) {
            // todo make sure pos is in right space?
            // todo move to octree base?
            octree.root.SetValue(pos, newVoxel);
        }

        public void SetVoxels(BoundsInt area, Func<Vector3Int, VoxelT, VoxelT> setFunc) {
            octree.SetValues(area, (pos) => setFunc(pos, default));
        }
        public void SetVoxels(Vector3Int startOffset, IVoxelVolume<VoxelT> fromVoxels) {
            throw new NotImplementedException();
        }
        public void SetVoxels(IEnumerable<Vector3Int> positions, VoxelT newVoxel) {
            octree.SetValues(positions, newVoxel);
        }
        public void FinishUpdating() => FinishUpdating(null);
        public void FinishUpdating(BoundsInt? area) {
            octree.FinishUpdating(area);
        }

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