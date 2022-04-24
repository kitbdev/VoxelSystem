using System.Collections;
using System.Collections.Generic;
using Kutil;
using UnityEngine;
using UnityEngine.Events;

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
        // If true, will save the world to SaveFilePath each time it's saved to the save object
        public bool autoSave = false;
        // Automatically loaded on creation
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

        [Space]
        // todo how to determine which voxel type to use here
        public VoxelOctree<VoxelCubic> voxelOctree;
        // public VoxelOctree<IVoxel> voxelOctreei;
        // todo?
        AllVoxelsHolder holder;
        class AllVoxelsHolder {
            // interface?
        }
        class CubicVoxelsHolder : AllVoxelsHolder {
            public VoxelOctree<VoxelCubic> voxelOctree;
        }
        class CuboidVoxelsHolder : AllVoxelsHolder {
            public VoxelOctree<VoxelCuboid> voxelOctree;
        }
        // and then user can extend their own holder
        // and I can set holder to whatever class at runtime
        // ? but is an interface to voxel octree good enough?
        // todo connect mesher on an actual chunk to octree first

        public enum VoxType {
            CUBIC, CUBOID, DENSITY, CUSTOM
        }
        public VoxType voxtype;
        public TypeChoice<IVoxelType> typeToUse;//?

        public VoxelTypeId typetest;


        /// <summary>
        /// Called when generating the world, right after it's created.
        /// Use this to add data items, or to do something right after the world is created
        /// </summary>
        public UnityEvent OnGenerateWorldEvent;
        public UnityEvent OnWorldLoadedEvent;
        /// <summary>
        /// Called right before destroying the world. Use this if you want to save data
        /// </summary>
        public UnityEvent OnWorldDestroyedEvent;


        bool isLoaded = false;
        float creationStartTime = 0;

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


        public static class VoxelDataTools {
            // for saves and stuff

            public struct FVoxelUncompressedWorldSave { }//?
            public static bool LoadFromSave(VoxelWorld world, FVoxelUncompressedWorldSave save) {
                if (world == null) return false;
                // todo
                // world.voxelOctree.Load
                return false;
            }
            // todo save
            // todo compress
        }

        // all chunks should be children of worlddatago
        // global runtime logic components can be added to it (like physics? LOD? renderer? idk) 
        GameObject worldDataGO = null;
        void RecreateWorld() {
            if (isLoaded) {
                // if (worldDataGO != null) {
                DestroyWorld();
            }
            CreateWorld();
        }
        void CreateWorld() {
            if (isLoaded || worldDataGO != null) {
                Debug.LogError("Cannot create world, already created", this);
                return;
            }
            Debug.Log("Loading World");

            isLoaded = false;
            creationStartTime = Time.time;

            // setup world data container GO
            worldDataGO = new GameObject("Voxel World Data");
            worldDataGO.transform.parent = transform;

            bool overrideData = true;
            if (overrideData) {
                // use generator

            } else {
                // load from save if applicable
                // todo checks
                VoxelDataTools.LoadFromSave(this, default);
            }
            OnGenerateWorldEvent?.Invoke();
            // todo
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



        void OnWorldLoaded() {
            isLoaded = true;
            Debug.LogFormat(this, "%s took %fs to generate", name, Time.time - creationStartTime);
            // if (bSimulatePhysicsOnceLoaded) {
            //     WorldRoot->BodyInstance.SetInstanceSimulatePhysics(true);
            // }
            OnWorldLoadedEvent?.Invoke();
        }
        public void FinishedGenerationCallback() {
            OnWorldLoaded();
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