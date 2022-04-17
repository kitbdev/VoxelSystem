using UnityEngine;

namespace VoxelSystem {
    [System.Serializable]
    public struct Voxel<TVal>
        where TVal : struct {
        public TVal value;
    }
    [System.Serializable]
    public struct Voxel {
        public System.UInt16 id;
    }
}