using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Generate Voxels for a world. 
    /// Inherit to provide funcitonality
    /// </summary>
    // [CreateAssetMenu(fileName = "VoxelGenerator", menuName = "VoxelSystem/VoxelGenerator", order = 0)]
    public abstract class VoxelGenerator : ScriptableObject {

        protected VoxelWorld voxelWorld;

        public abstract void Clear();
        // todo generate by chunk? bounds? any instance data allowed?
        public abstract void Generate();
        public abstract void GenerateImmediate();
        // ? how to set voxels

        public void FinishedGeneration() {
            //? do we need this
            voxelWorld.FinishedGenerationCallback();
        }
    }
}