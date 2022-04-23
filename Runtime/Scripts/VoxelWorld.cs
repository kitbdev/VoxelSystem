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
        public bool createAutomatically = true;
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
        public VoxelTypeHolder materialSet;
        // [Header("Navmesh settings")]
        [Header("LOD settings")]
        public int maxLOD;
        [Header("Performance settings")]
        public int octreeDepth = 8;
        [Header("Debug")]
        public bool hideChunks = true;
        public bool debugEnabled = false;

        public VoxelOctree voxelOctree;
        public VoxelTypeId type;


        private void OnValidate() {
            // Debug.Log("onval vw");
            // mesher.OnValidate();
        }

        private void OnEnable() {
            if (createAutomatically) {
                CreateWorld();
            }
        }
        private void OnDisable() {
            DestroyWorld();
        }


        // all chunks should be children of worlddatago
        // global runtime logic components can be added to it (like physics? LOD? renderer? idk) 
        GameObject worldDataGO = null;
        void CreateWorld() {
            if (worldDataGO != null) {
                DestroyWorld();
            }
            // make container GO
            worldDataGO = new GameObject("Voxel World Data");
            worldDataGO.transform.parent = transform;

            // load from save if applicable
            // todo
            // use generator
        }
        void DestroyWorld() {
            if (worldDataGO != null) {
                if (Application.isPlaying) {
                    Destroy(worldDataGO);
                } else {
                    DestroyImmediate(worldDataGO);
                }
                worldDataGO = null;
            }
        }


        public void FinishedGeneration() {
            // event?
        }

        void StartGeneration() {
            if (generator == null) {
                return;
            }
            generator.Generate();
        }
        void StartGenerationImmediate() {
            if (generator == null) {
                return;
            }
            generator.GenerateImmediate();
        }
        void ClearGen() {
            if (generator == null) {
                return;
            }
            generator.Clear();
        }
    }
}