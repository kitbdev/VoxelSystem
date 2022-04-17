using UnityEngine;

namespace VoxelSystem {
    [CreateAssetMenu(fileName = "VoxelMaterialCollectionBaseSO", menuName = "VoxelSystem/VoxelMaterialCollectionBaseSO", order = 0)]
    public class VoxelMaterialCollectionBaseSO : ScriptableObject {
        public VoxelMaterial[] voxelMaterials;
    }
}