
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// greedy cubic mesher. 
    /// uses new unity mesh api.
    /// slower than quick greedy, but saves more tris
    /// </summary>
    [System.Serializable]
    public class VoxelGreedyCubicMesher : VoxelMesherBase {
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