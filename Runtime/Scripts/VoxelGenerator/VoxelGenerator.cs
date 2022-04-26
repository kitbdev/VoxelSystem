using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Generate Voxels for a world. 
    /// Inherit to provide funcitonality
    /// </summary>
    // [CreateAssetMenu(fileName = "VoxelGenerator", menuName = "VoxelSystem/VoxelGenerator", order = 0)]
    // public abstract class VoxelGenerator<VoxelT> : ScriptableObject where VoxelT : IVoxel, new() {
    public abstract class VoxelGenerator : ScriptableObject {

        // protected VoxelWorld voxelWorld;
        // todo use this instead of world
        public VoxelSettings voxelSettings;

        public event System.Action<Vector3Int> onFinishedGeneratingChunkEvent;


        public abstract void Clear();
        public abstract IVoxelVolume<IVoxel> GetVoxels();

        /// <summary>
        /// Start Generating voxels in specified area.
        /// Call FinishedGeneration when finished.
        /// Generated Voxels should be accessible in GetVoxels().
        /// </summary>
        /// <param name="chunkId">chunk position</param>
        /// <param name="chunkResolution">resolution of chunk in voxels</param>
        //todo async where some parts are ready first?
        public abstract void GenerateChunk(Vector3Int chunkPos, Vector3Int chunkResolution);
        /// <summary>
        /// Start Generating voxels in specified area.
        /// Call FinishedGeneration when finished.
        /// Generated Voxels should be accessible in GetVoxels().
        /// </summary>
        /// <param name="worldBounds"></param>
        // public abstract void GenerateVoxels(BoundsInt worldBounds);
        // todo what about features that overlap? structures larger than a chunk or on the border?


        public void FinishedGeneration() {
            // todo
            onFinishedGeneratingChunkEvent?.Invoke(default);
            //? do we need this
            // voxelWorld.FinishedGenerationCallback();
        }
    }
}