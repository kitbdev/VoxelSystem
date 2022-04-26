using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelSystem {
    [CreateAssetMenu(fileName = "SphereVoxelGenerator", menuName = "VoxelSystem/Generator/Sphere VoxelGenerator", order = 0)]
    public class VoxelSphereGenerator : VoxelGenerator {

        public float heightLevel;
        public VoxelMaterialId surfaceType;
        public VoxelMaterialId centerType;


        VoxelVolume<VoxelCubic> voxels;

        public override IVoxelVolume<IVoxel> GetVoxels() => (IVoxelVolume<IVoxel>)voxels;
        public override void Clear() {
            voxels.ClearAllVoxels();
        }

        public override void GenerateChunk(Vector3Int chunkPos, Vector3Int chunkSize) {

            FinishedGeneration();
        }
    }
}