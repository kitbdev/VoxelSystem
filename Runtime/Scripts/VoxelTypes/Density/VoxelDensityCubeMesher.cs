using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// cube mesher for density voxels.
    /// uses new unity mesh api but does not greedily mesh.
    /// </summary>
    [System.Serializable]
    public class VoxelDensityCubeMesher : VoxelMesherBase {
        
        public float groundThreshold;

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