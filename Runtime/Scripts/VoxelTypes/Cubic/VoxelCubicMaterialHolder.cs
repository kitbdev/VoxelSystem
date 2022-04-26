using System.Linq;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Holds all VoxelCubicType so they can be easily found, configured, and used by other components
    /// </summary>
    [CreateAssetMenu(fileName = "VoxelCubicTypeHolder", menuName = "VoxelSystem/VoxelCubicTypeHolder", order = 0)]
    public class VoxelCubicMaterialHolder : VoxelMaterialHolderSOType<VoxelMaterialCubic> {
        // VoxelCubicType[] voxelCubicTypes;

        // protected override VoxelTypeId GetIdForVoxelType(VoxelCubicType voxelType) {
        //     // todo use hash for ids?
        //     int id = System.Array.IndexOf(voxelCubicTypes, voxelType);
        //     // Debug.LogWarning($"could not find id for vmat {voxelMaterial}");
        //     return id;
        // }
        // public override VoxelCubicType GetVoxelType(VoxelTypeId id) {
        //     if (id < 0 || id >= voxelCubicTypes.Length) {
        //         Debug.LogWarning($"VoxelCubicType {id} not found!");
        //         return null;
        //     }
        //     return voxelCubicTypes[id];
        // }

        // // todo some way of keeping typeids accurate between updating the types, not just index

        // public override void ClearVoxelTypes() {
        //     voxelCubicTypes = new VoxelCubicType[0];
        // }
        // // public override void RemoveVoxelMaterial(VoxelCubicType voxMat) {
        // //     // todo 
        // //     // uh removing one will mess up entire array indices
        // //     // disallow?
        // // }
        // public override VoxelTypeId AddVoxelTypes(VoxelCubicType newVoxMat) {
        //     // should only really be used in editor, or at least not frequently
        //     voxelCubicTypes = voxelCubicTypes.Append(newVoxMat).ToArray();
        //     // ? any preprocessing on voxel materials?
        //     // if (newVoxMat.material != null) {
        //     //     if (!allUsedMaterials.Contains(newVoxMat.material)) {
        //     //         List<Material> materials = allUsedMaterials.ToList();
        //     //         materials.Append(newVoxMat.material);
        //     //         allUsedMaterials = materials.ToArray();
        //     //     }
        //     //     newVoxMat.materialIndex = System.Array.IndexOf(allUsedMaterials, newVoxMat.material);
        //     // }
        //     // newVoxMat.Initialize(this);
        //     return GetIdForVoxelType(newVoxMat);
        // }
        // public override Material[] GetUniqueMaterials(VoxelTypeId[] ids) {
        //     return voxelCubicTypes.Select(vct => vct.mat).Where(m => m != null).Distinct().ToArray();
        // }
    }
}