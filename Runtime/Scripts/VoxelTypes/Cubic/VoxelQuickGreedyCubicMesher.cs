
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// quick greedy cubic mesher. 
    /// uses new unity mesh api.
    /// faster than greedy and still saves some tris.
    /// </summary>
    [System.Serializable]
    public class VoxelQuickGreedyCubicMesher : VoxelMesherBase {
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