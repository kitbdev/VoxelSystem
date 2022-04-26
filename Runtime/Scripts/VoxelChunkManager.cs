using System.Collections;
using System.Collections.Generic;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    /// <summary>
    /// Manages adding, removing, and updating chunks.
    /// Chunks are LOD meshes.
    /// </summary>
    // todo use lod groups? maybe
    // https://docs.unity3d.com/Manual/class-LODGroup.html
    // configured through code the various gos. 
    // chunks have a fixed size in voxels regardless of lod? maybe
    public class VoxelChunkManager : MonoBehaviour {
        struct Chunk {
            enum EChunkState : byte {
                NewChunk,
                Hidden,
                DitheringIn,
                Showed,
                WaitingForNewChunks,
                DitheringOut
            };
            int Id;
            byte LOD;
            BoundsInt Bounds;
            EChunkState State;// = EChunkState::NewChunk;
        }
        struct ChunkTimeData {
            public ChunkId chunkId;
            // Time at which to remove or stop dithering the chunk
            public float time;
        };

        VoxelWorld world;

        List<ChunkTimeData> chunksToRemove;
        List<ChunkTimeData> chunksToAdd;
        Dictionary<ChunkId, VoxelChunk> loadedChunks = new Dictionary<ChunkId, VoxelChunk>();

        private void Awake() {
            world ??= GetComponentInParent<VoxelWorld>();
        }

        public void Init(VoxelWorld world) {
            this.world = world;
        }

        int UpdateChunks(BoundsInt Bounds, List<ChunkId> ChunksToUpdate, System.Action FinishDelegate) {
            // update the mesh

            return -1;
        }
        // void UpdateLODs(long InUpdateIndex, List<FVoxelChunkUpdate> ChunkUpdates) {

        // }

        public void ClearAllChunks() {

        }
        public void ForceLoadChunk() {
            CreateNewChunk(new ChunkId(Vector3Int.zero), 0);
        }

        void CreateNewChunk(ChunkId chunkId, byte lod) {
            string chunkName = $"Chunk {chunkId.chunkpos} lod:{lod}";
            GameObject chunkGo = new GameObject(chunkName);
            if (world.debugEnabled) Debug.Log($"Creating chunk {chunkName}", chunkGo);
            chunkGo.transform.SetParent(transform, false);
            // position
            // todo
            chunkGo.transform.localPosition = Vector3.zero;
            chunkGo.transform.localRotation = Quaternion.identity;

            VoxelChunk chunk = chunkGo.AddComponent<VoxelChunk>();
            chunk.chunkId = chunkId;

            loadedChunks.Add(chunkId, chunk);

            // todo load from file option?
            // todo do in world?
            // world.generator.onFinishedGeneratingChunkEvent+=
            FillChunkGen(chunkId);
        }
        void FillChunkGen(ChunkId chunkId) {
            if (world.debugEnabled) Debug.Log($"filling chunk {chunkId}");
            world.generator.GenerateChunk(chunkId.chunkpos, world.chunkResolution * Vector3Int.one);
        }

    }
}