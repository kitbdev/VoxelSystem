using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// default cuboid mesher that uses new unity mesh api but does not greedily mesh.
    /// creates cube mesh, and boxes for non perfect cubes
    /// </summary>
    [System.Serializable]
    public class VoxelCuboidMesher : VoxelMesherBase {
        public override void ClearMesh() {
            throw new System.NotImplementedException();
        }

        public override void UpdateMesh() {
            throw new System.NotImplementedException();
        }

        public override void UpdateMeshAt(Vector3Int vpos) {
            throw new System.NotImplementedException();
        }

        internal override Mesh ApplyMesh() {
            throw new System.NotImplementedException();
        }
    }
}