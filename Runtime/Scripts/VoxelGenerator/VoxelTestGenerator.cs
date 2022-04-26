using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelSystem {
    [CreateAssetMenu(fileName = "TestVoxelGenerator", menuName = "VoxelSystem/Generator/Test VoxelGenerator", order = 0)]
    public class VoxelTestGenerator : VoxelGenerator {
        
        // public float heightLevel;
        public VoxelMaterialId fillType;

        VoxelVolume<VoxelCubic> voxels;

        public override IVoxelVolume<IVoxel> GetVoxels() => (IVoxelVolume<IVoxel>)voxels;
        public override void Clear() {
            voxels.ClearAllVoxels();
        }

        // todo voxelsettings and dont pass chunkres
        public override void GenerateChunk(Vector3Int chunkPos, Vector3Int chunkRes) {
            BoundsInt bounds = new BoundsInt(chunkPos * chunkRes, chunkRes);
            Debug.Log($"Generating {chunkPos} {bounds}...");
            voxels = new VoxelVolume<VoxelCubic>();
            voxels.Init(bounds.size);
            ((IVoxelVolume<VoxelCubic>)voxels).SetAllVoxels(new VoxelCubic(fillType));
            FinishedGeneration();
            Debug.Log($"Finished Generating {chunkPos} {bounds}");
        }
    }
}