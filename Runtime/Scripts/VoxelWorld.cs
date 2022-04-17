using System.Collections;
using System.Collections.Generic;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    /*
    loads from saveobject or generates from generator on enable
    voxels are discarded on disable?
    */
    /// <summary>
    /// Manages a world of Voxels. manages chunks and LOD. stores octree. starts generator. holds many voxel settings
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    public class VoxelWorld : MonoBehaviour {

        [Header("Voxel settings")]
        public float voxelSize = 1;
        public int chunkResolution = 16;
        public int octtreeDepth = 16;
        public bool regenerateOnAwake = true;
        public bool saveChunksInScene = false;

        public VoxelGenerator generator;

        [Header("Save settings")]
        public bool autoSave = false;
        public VoxelsSaveObject saveObject;//todo
        public Object loadBtnObject;//todo 
        // [Header("Generator settings")]
        [Header("Render settings")]
        // public TypeSelector<VoxelMesherBase> mesher = new TypeSelector<VoxelMesherBase>(typeof(VoxelSimpleMesher));
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

        public VoxelOctree voxelOctree;


        private void OnValidate() {
            // Debug.Log("onval vw");
            // mesher.OnValidate();
        }

        public void FinishedGeneration() {
            // event?
        }

        public void StartGeneration() {
            if (generator == null) {
                return;
            }
            generator.Generate();
        }
        public void StartGenerationImmediate() {
            if (generator == null) {
                return;
            }
            generator.GenerateImmediate();
        }
        public void ClearGen() {
            if (generator == null) {
                return;
            }
            generator.Clear();
        }
    }
}