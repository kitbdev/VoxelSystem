using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VoxelSystem {
    [CreateAssetMenu(fileName = "FlatVoxelGenerator", menuName = "VoxelSystem/Generator/Flat VoxelGenerator", order = 0)]
    public class VoxelFlatGenerator : VoxelGenerator {
        
        public float heightLevel;
        public VoxelTypeId surfaceType;
        public VoxelTypeId underGroundType;

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