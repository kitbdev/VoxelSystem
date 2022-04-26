using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    public struct ChunkId {
        public Vector3Int chunkpos;
    }
    /// <summary>
    /// stores data for a chunk of voxels.
    /// manages voxel renderers.
    /// ? want this
    /// </summary>
    public class VoxelChunk : MonoBehaviour {
        public ChunkId chunkId;
        // public Vector3Int chunkpos;

        // per layer renderers
        [SerializeField, HideInInspector]
        List<VoxelRenderer> renderers = new List<VoxelRenderer>();

        private void Awake() {
        }

        void ClearRenderers() {
            for (int i = renderers.Count - 1; i >= 0; i--) {
                VoxelRenderer renderer = renderers[i];
                if (Application.isPlaying) {
                    Destroy(renderer.gameObject);
                } else {
                    DestroyImmediate(renderer.gameObject);
                }
            }
            renderers.Clear();
        }
        void AddRendererLayer(int layer) {
            GameObject renGo = new GameObject($"ChunkRenderer {renderers.Count}-{layer}");
            renGo.transform.SetParent(transform, false);
            renGo.transform.localPosition = Vector3.zero;
            renGo.transform.localRotation = Quaternion.identity;
            renGo.layer = layer;
            VoxelRenderer voxelRenderer = renGo.AddComponent<VoxelRenderer>();
            renderers.Add(voxelRenderer);
        }
        VoxelRenderer GetRendererOnLayer(int layer) {
            return renderers.FirstOrDefault(vr => vr.gameObject.layer == layer);
        }
        void ForEachRenderer(System.Action<VoxelRenderer> action) {
            foreach (var renderer in renderers) {
                action?.Invoke(renderer);
            }
        }
    }
}