using UnityEngine;

namespace VoxelSystem {
    // public  class VoxelTypeHolderSO : VoxelTypeHolderSOType<IVoxel>  {}
    /// <summary>
    /// Holds all of a VoxelType so they can be easily found, configured, and used by other components
    /// </summary>
    public abstract class VoxelTypeHolderSO : ScriptableObject  {
    }
    // ?
    // todo prob just remove type constraint and use actual in meshers
    public abstract class VoxelTypeHolderSOType<VoxelT> : VoxelTypeHolderSO where VoxelT : IVoxelType {
    // public abstract class VoxelTypeHolderSOType<VoxelT> : ScriptableObject where VoxelT : IVoxelType {

        // general

        protected abstract VoxelTypeId GetIdForVoxelType(VoxelT voxelType);
        public abstract VoxelT GetVoxelType(VoxelTypeId id);

        // for type editors and loaders

        public abstract void ClearVoxelTypes();
        // public abstract void RemoveVoxelMaterial(VoxelT voxMat);
        public abstract VoxelTypeId AddVoxelTypes(VoxelT newVoxMat);

        // for meshers
        public abstract Material[] GetUniqueMaterials(VoxelTypeId[] ids);
    }
}