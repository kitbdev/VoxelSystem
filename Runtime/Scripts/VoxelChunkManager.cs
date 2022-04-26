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

        List<ChunkTimeData> chunksToRemove;
        List<ChunkTimeData> chunksToAdd;
        Dictionary<ChunkId, VoxelChunk> loadedChunks = new Dictionary<ChunkId, VoxelChunk>();


        int UpdateChunks(BoundsInt Bounds, List<ChunkId> ChunksToUpdate, System.Action FinishDelegate) {
            // update the mesh

            return -1;
        }
        // void UpdateLODs(long InUpdateIndex, List<FVoxelChunkUpdate> ChunkUpdates) {

        // }

        public void ClearAllChunks() {

        }
        public void ForceLoadChunk() {
            // CreateNewChunk();
        }

        void CreateNewChunk(ChunkId chunkId, byte lod) {
            GameObject chunkGo = new GameObject("Chunk " + chunkId.chunkpos + " lod:" + lod);
            chunkGo.transform.SetParent(transform, false);
            // position
            // todo
            chunkGo.transform.localPosition = Vector3.zero;
            chunkGo.transform.localRotation = Quaternion.identity;


            VoxelChunk chunk = chunkGo.AddComponent<VoxelChunk>();
            chunk.chunkId = chunkId;

            loadedChunks.Add(chunkId, chunk);
        }

    }
}