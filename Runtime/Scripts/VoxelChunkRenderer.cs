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
    public class VoxelChunkRenderer : MonoBehaviour {
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
        struct FChunkT {
            long Id;
            double Time; // Time at which to remove or stop dithering the chunk
        };
        struct ChunkId {
            public Vector3Int chunkpos;
        }

        List<FChunkT> chunksToRemove;
        List<FChunkT> chunksToAdd;


        int UpdateChunks(BoundsInt Bounds, List<ChunkId> ChunksToUpdate, System.Action FinishDelegate) {
            return -1;
        }
        // void UpdateLODs(long InUpdateIndex, List<FVoxelChunkUpdate> ChunkUpdates) {

        // }

        void ClearAllChunks() {

        }

        void CreateNewChunk(ChunkId chunkId, byte lod) {
            GameObject chunkGo = new GameObject("Chunk " + chunkId.chunkpos + " lod:" + lod);
            chunkGo.transform.parent = transform;
            // position

        }

    }
}