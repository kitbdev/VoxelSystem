using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Holds all relevant VoxelMaterials so they can be easily configured and used by other components
    /// </summary>
    [CreateAssetMenu(fileName = "VoxelMaterialCollectionBase", menuName = "VoxelSystem/VoxelMaterialCollectionBase", order = 0)]
    public class VoxelMaterialCollectionBaseSO : ScriptableObject {

        // todo auto add air?

        [SerializeField] VoxelMaterial[] voxelMaterials;

        // [SerializeField]
        // [HideInInspector]
        // Kutil.SerializableDictionary<VoxelMaterialId, VoxelMaterial> voxelMats;
        public VoxelMaterialId voxelTypeIdTest;
        // public VoxelMaterialId[] voxelTypeIds;
        // public Dictionary<VoxelMaterialId, VoxelMaterial> voxelMaterialDict => voxelMats;


        VoxelMaterialId GetIdForVoxelMaterial(VoxelMaterial voxelMaterial) {
            // todo use hash for ids?
            int id = System.Array.IndexOf(voxelMaterials, voxelMaterial);
            // Debug.LogWarning($"could not find id for vmat {voxelMaterial}");
            return id;
        }
         public VoxelMaterial GetVoxelMaterial(VoxelMaterialId id) {
            if (id < 0 || id >= voxelMaterials.Length) {
                Debug.LogWarning($"VoxelMaterial {id} not found!");
                return null;
            }
            return voxelMaterials[id];
        }

        public void ClearVoxelMaterials(){
            voxelMaterials = new VoxelMaterial[0];
        }
        public void RemoveVoxelMaterial(VoxelMaterial voxMat) {
            // todo 
            // uh removing one will mess up entire array
        }
        public VoxelMaterialId AddVoxelMaterial(VoxelMaterial newVoxMat) {
            // should only really be used in editor, or at least not frequently
            voxelMaterials = voxelMaterials.Append(newVoxMat).ToArray();
            // ? any preprocessing on voxel materials?
            // if (newVoxMat.material != null) {
            //     if (!allUsedMaterials.Contains(newVoxMat.material)) {
            //         List<Material> materials = allUsedMaterials.ToList();
            //         materials.Append(newVoxMat.material);
            //         allUsedMaterials = materials.ToArray();
            //     }
            //     newVoxMat.materialIndex = System.Array.IndexOf(allUsedMaterials, newVoxMat.material);
            // }
            // newVoxMat.Initialize(this);
            return GetIdForVoxelMaterial(newVoxMat);
        }
        public Material[] GetUniqueMaterials(VoxelMaterialId[] ids){
            // for meshers
            // todo
            return default;
        }
    }
}