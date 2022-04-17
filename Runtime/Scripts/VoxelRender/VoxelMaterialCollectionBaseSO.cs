using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Holds all relevant VoxelMaterials so they can be easily configured and used by other components
    /// </summary>
    [CreateAssetMenu(fileName = "VoxelMaterialCollectionBaseSO", menuName = "VoxelSystem/VoxelMaterialCollectionBaseSO", order = 0)]
    public class VoxelMaterialCollectionBaseSO : ScriptableObject {
        public VoxelMaterial[] voxelMaterials;
    }
}