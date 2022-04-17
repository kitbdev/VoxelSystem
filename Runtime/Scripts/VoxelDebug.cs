using System.Collections;
using System.Collections.Generic;
using Kutil;
using UnityEngine;

namespace VoxelSystem {
    public static class VoxelDebug {
        /// <summary>
        /// Checks a condition if debug is enabled
        /// </summary>
        /// <param name="condition"></param>
        public static void Check(System.Func<bool> condition, string msg = null, Object context = null) {
#if VOXELS_DEBUG
            if (msg == null) {
                msg = "Voxel Check Failed: {" + condition.ToString() + "}";
            }
            Debug.Assert(condition.Invoke(), msg, context);
#endif
        }
        public static void DebugBreak() {
#if VOXELS_DEBUG
            Debug.Break();
#endif
        }
    }
    public static class VoxelTiming {
        // todo 
#if VOXELS_TIMING
#endif
    }
}