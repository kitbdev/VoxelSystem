
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// default smooth mesher for density voxels.
    /// uses new unity mesh api.
    /// </summary>
    // todo transvoxel algorithm?
    [System.Serializable]
    public class VoxelMarchingCubesMesher : VoxelMesherBase {
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