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
    public class VoxelSettings {

        public float voxelSize = 1;
    }
    /// <summary>
    /// Manages a world of Voxels. manages chunks and LOD. stores octree. starts generator. holds many voxel settings
    /// </summary>
    [SelectionBase]
    [DisallowMultipleComponent]
    public class VoxelWorld : MonoBehaviour {

        // settings

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
        public VoxelMaterialHolder materialSet;
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
        public VoxelTypeInterface holder;
        public class VoxelTypeInterface {
            // interface?
            IVoxelVolume<IVoxel> volume;
        }
        public class VoxelTypeInterface<VoxelMaterialT, VoxelT> : VoxelTypeInterface
                                                    where VoxelMaterialT : IVoxelMaterial
                                                    where VoxelT : struct, IVoxel {
            public VoxelOctree<VoxelT> voxelOctree;
        }
        public class CubicVoxelsHolder : VoxelTypeInterface<VoxelMaterialCubic, VoxelCubic> { }
        public class CuboidVoxelsHolder : VoxelTypeInterface<VoxelMaterialCuboid, VoxelCuboid> { }
        // and then user can extend their own holder
        // and I can set holder to whatever class at runtime
        // ? but is an interface to voxel octree good enough?
        // todo connect mesher on an actual chunk to octree first

        public enum VoxType {
            CUBIC, CUBOID, DENSITY, CUSTOM
        }
        public VoxType voxtype;
        public TypeChoice<IVoxelMaterial> typeToUse;//?

        public VoxelMaterialId typetest;


        [Header("Events")]
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

        // all chunks should be children of worlddatago
        // global runtime logic components can be added to it (like physics? LOD? renderer? idk) 
        GameObject worldDataGO = null;

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

        [ContextMenu("Recreate")]
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
            if (debugEnabled) Debug.Log("Creating Voxel World", this);

            isLoaded = false;
            creationStartTime = Time.time;


            // setup world data container GO
            worldDataGO = new GameObject("Voxel World Data");
            worldDataGO.transform.parent = transform;
            VoxelChunkManager voxelChunkManager = worldDataGO.AddComponent<VoxelChunkManager>();
            voxelChunkManager.Init(this);
            if (generator == null) {
                Debug.LogWarning("No generator set!", this);
            } else {
                generator.onFinishedGeneratingChunkEvent += (_) => FinishedGenerationCallback();
                voxelChunkManager.ForceLoadChunk();
            }

            bool loadFromFile = false;
            if (loadFromFile) {
                // load from save if applicable
                // todo checks
                VoxelDataTools.LoadFromSave(this, default);
            } else {
                // use generator?
                // StartGeneration();
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
            isLoaded = false;
            if (debugEnabled) Debug.Log("Destroyed Voxel World", this);
        }

        void OnWorldLoaded() {
            isLoaded = true;
            Debug.LogFormat(this, "{0} took {1:N6}s to load", name, Time.time - creationStartTime);
            // if (bSimulatePhysicsOnceLoaded) {
            //     WorldRoot->BodyInstance.SetInstanceSimulatePhysics(true);
            // }
            OnWorldLoadedEvent?.Invoke();
            Debug.Log("Loaded Voxel World Successfully", this);
        }
        public void FinishedGenerationCallback() {
            OnWorldLoaded();
        }

        // void StartGeneration() {
        //     if (generator == null) {
        //         Debug.LogError("Cannot Start, No generator set!");
        //         return;
        //     }
        //     generator.Generate();
        // }
        // void ClearGen() {
        //     if (generator == null) {
        //         return;
        //     }
        //     generator.Clear();
        // }
    }
}