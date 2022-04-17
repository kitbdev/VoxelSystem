using UnityEngine;

namespace VoxelSystem {
    // [CreateAssetMenu(fileName = "VoxelsSaveObject", menuName = "VoxelSystem/VoxelsSaveObject", order = 0)]
    /// <summary>
    /// stored in project. made from saving a world or importing voxel data
    /// </summary>
    public class VoxelsSaveObject : ScriptableObject {
        // ? split into multiple chunks? compressed?
        VoxelVolume voxelVolume;
    }
}