using UnityEngine;

namespace VoxelSystem {
    public static class BoundsIntExt {
        public static bool BoundsIntIntersects(this BoundsInt bounds, BoundsInt other) {
            return AsBounds(bounds).Intersects(AsBounds(other));
        }
        public static Bounds AsBounds(this BoundsInt bounds) {
            return new Bounds(bounds.center, bounds.size);
        }
        public static bool BoundsIntContains(this BoundsInt bounds, BoundsInt smaller) {
            return bounds.Contains(smaller.min) && bounds.Contains(smaller.max);
        }
    }
}