
using System.Collections.Generic;
using Kutil;
using UnityEngine;

namespace VoxelSystem {

    // ? have more settings
    // todo operate on voxel volume?
    /// <summary>
    /// Provides Meshing functionality for voxels. meshes an entire chunk at a time. lives on that chunk.
    /// </summary>
    // [System.Serializable]
    // public abstract class VoxelMesherBase<VoxelTypeT, VoxelT> where VoxelTypeT : IVoxelType, new()
    //                                                       where VoxelT : struct, IVoxel {
    // // todo need a generic one?
    // }
    public abstract class VoxelMesherBase {

        // protected VoxelChunk chunk;
        // Vector3Int chunkPos;
        // protected VoxelWorld world;
        protected float voxelSize;

        protected IEnumerable<VoxelRenderer> renderers;
        // protected VoxelRenderer renderer => renderers[0];
        protected IVoxelVolume<IVoxel> voxelVolume;

        // protected VoxelTypeHolderSOType<VoxelTypeT> typeHolder;// => world?.materialSet;

        int lod;
        int step;
        int size;
        //RuntimeSettings settings
        //bool isTransitionMesh
        // reference to worlds octree
        // VoxelOctree<VoxelT> octree;
        // use cache

        // ? optional timing
        // cached values?
        // accelerator for faster octree operations

        // 

        // todo base->mesher and base->transitionmesher
        // each mesher will have to have and implementation for both


        public virtual void Initialize(IVoxelVolume<IVoxel> voxelVolume, IEnumerable<VoxelRenderer> renderers) {
            this.voxelVolume = voxelVolume;
            this.renderers = renderers;
        }
        // public virtual void Initialize(VoxelChunk chunk, VoxelRenderer renderer, bool renderNullSides=false) {
        //     this.chunk = chunk;
        //     this.world = chunk.world;
        //     this.renderer = renderer;
        //     this.renderNullSides = renderNullSides;
        //     voxelSize = world.voxelSize;
        // }
        public abstract void ClearMesh();
        public abstract void UpdateMesh();
        public abstract void UpdateMeshAt(Vector3Int vpos);
        internal abstract Mesh ApplyMesh();

        protected void FinishedMesh() {
            // renderer.ApplyMesh();
        }
    }
}