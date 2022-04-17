
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Renders a chunk of voxels using a voxel mesher and a meshfilter
    /// </summary>
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class VoxelRenderer : MonoBehaviour {

        [SerializeField, ReadOnly]
        VoxelWorld world;
        [SerializeField, SerializeReference, ReadOnly]
        VoxelMesherBase voxelMesher;
        [SerializeField, ReadOnly]
        MeshFilter meshFilter;
        
        // todo use world octtree

        // public void Initialize(VoxelChunk chunk) {
        //     this.chunk = chunk;
        //     UpdateMesher();
        // }

        [ContextMenu("ClearMesh")]
        public void ClearMesh() {
            voxelMesher.ClearMesh();
        }

        [ContextMenu("UpdateMesher")]
        public void UpdateMesher() {
            UpdateMaterials();
            // voxelMesher = chunk.world.mesher.CreateInstance();
            // voxelMesher.Initialize(chunk, this, chunk.world.renderNullSides);
        }
        [ContextMenu("UpdateMesh")]
        public void UpdateMesh() {

            voxelMesher.UpdateMesh();
        }
        public void UpdateMeshAt(Vector3Int vpos) {
            voxelMesher.UpdateMeshAt(vpos);
        }
        public Mesh GetMesh() {
            return meshFilter?.sharedMesh;
        }
        [ContextMenu("UpdateMats")]
        public void UpdateMaterials() {
            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            // meshRenderer.sharedMaterials = chunk?.world.materialSet?.allUsedMaterials;
        }
        /// <summary>
        /// called by mesher after update is complete
        /// </summary>
        internal void ApplyMesh() {
            Mesh mesh = voxelMesher.ApplyMesh();

            meshFilter ??= GetComponent<MeshFilter>();
            if (meshFilter == null) {
                Debug.LogWarning($"no mesh filter on {name}");
            }
            meshFilter.sharedMesh = mesh;
#if UNITY_EDITOR
            if (!Application.isPlaying) {
                // mark scene not saved
                UnityEditor.EditorUtility.SetDirty(gameObject);
            }
#endif
        }
    }
}