using System.Collections;
using System.Collections.Generic;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    public class VoxelWorld : MonoBehaviour {
        [Header("Voxel settings")]
        public float voxelSize = 1;
        public int chunkResolution = 16;
        public int octtreeDepth = 16;
        public bool regenerateOnAwake = true;
        public bool saveChunksInScene = false;
        [Header("Save settings")]
        public bool autoSave = false;
        public Object saveObject;//todo SO?
        public Object loadBtnObject;//todo 
        // [Header("Generator settings")]
        [Header("Render settings")]
        public TypeSelector<VoxelMesherBase> mesher = new TypeSelector<VoxelMesherBase>(typeof(VoxelSimpleMesher));
        [Header("Physics settings")]
        public bool enableCollision = false;
        // todo 
        public Layer layer;
        [Header("Material settings")]
        public VoxelMaterialCollectionBaseSO materialSet;
        // [Header("Navmesh settings")]
        [Header("LOD settings")]
        public int maxLOD;
        [Header("Performance settings")]
        public int octreeDepth = 8;
        [Header("Debug")]
        public bool hideChunks = true;
        public bool debugEnabled = false;

        private void OnValidate() {
            // Debug.Log("onval vw");
            mesher.OnValidate();
        }
    }
}