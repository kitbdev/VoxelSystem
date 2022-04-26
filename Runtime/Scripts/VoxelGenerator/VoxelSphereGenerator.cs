using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelSystem {
    [CreateAssetMenu(fileName = "SphereVoxelGenerator", menuName = "VoxelSystem/Generator/Sphere VoxelGenerator", order = 0)]
    public class VoxelSphereGenerator : VoxelGenerator {
        
        public float heightLevel;
        public VoxelMaterialId surfaceType;
        public VoxelMaterialId centerType;

        public override void Clear() {
            throw new System.NotImplementedException();
        }

        public override void Generate() {
            throw new System.NotImplementedException();
        }

        public override void GenerateImmediate() {
            throw new System.NotImplementedException();
        }
    }
}