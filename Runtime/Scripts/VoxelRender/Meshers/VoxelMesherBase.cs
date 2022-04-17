
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    // todo make into SO? would need to not store things here, and support creating multiple meshes at a time
    // ? have more settings
    // todo operate on voxel volume?
    /// <summary>
    /// Provides Meshing functionality for voxels
    /// </summary>
    [System.Serializable]
    public abstract class VoxelMesherBase {

        // protected VoxelChunk chunk;
        // Vector3Int chunkPos;
        // protected VoxelWorld world;
        protected VoxelRenderer renderer;
        protected float voxelSize;
        public bool renderNullSides;//? move to mat col?

        public virtual TypeChoice<VoxelMaterial> neededMaterial => null;
        // public virtual TypeChoice<VoxelData>[] neededDatas => new TypeChoice<VoxelData>[0];

        protected VoxelMaterialCollectionBaseSO materialSet;// => world?.materialSet;

int lod;
int step;
int size;
//RuntimeSettings settings
//bool isTransitionMesh
// reference to worlds octree
VoxelOctree octree;

// ? optional timing
// cached values?
// accelerator for faster octree operations

// 

// todo base->mesher and base->transitionmesher
// each mesher will have to have and implementation for both


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