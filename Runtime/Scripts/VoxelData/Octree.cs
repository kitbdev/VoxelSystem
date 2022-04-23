using System.Collections;
using UnityEngine;

// fixed size generic octree

namespace VoxelSystem {
    [System.Serializable]
    public class Octree<TData> where TData : struct {
        public OctreeParent<TData> root;
        public int maxHeight;//?
        // util stuff?

        // todo


        public void Clear() {
            root.Dispose();
            root = new OctreeParent<TData>((byte)maxHeight);
        }
    }
    [System.Serializable]
    public abstract class OctreeNode<TData> : System.IDisposable
    where TData : struct {

        public int chunksize;

        public int height;
        public Vector3Int pos;

        public OctreeNode(int height, Vector3Int pos) {
            this.height = height;
            this.pos = pos;
            Debug.Assert(height < 32);
        }
        public bool IsLeaf() => height == 0;
        public int GetSize() => chunksize << height;
        public int GetHalfSize() => (chunksize / 2) << height;
        public Vector3Int GetMin() => pos - GetHalfSize() * Vector3Int.one;
        public Vector3Int GetMax() => pos + GetHalfSize() * Vector3Int.one;
        // this is probably correctly setting witht the center
        public BoundsInt GetBounds() => new BoundsInt(pos, GetSize() * Vector3Int.one);
        // public BoundsInt GetBounds() => new BoundsInt().SetMinMax(GetMin(), GetMax());
        public bool IsInOctree(Vector3Int pos) => IsInOctree(pos.x, pos.y, pos.z);
        public bool IsInOctree(int x, int y, int z) {
            int HalfSize = GetHalfSize();
            return
                pos.x - HalfSize <= x && x < pos.x + HalfSize &&
                pos.y - HalfSize <= y && y < pos.y + HalfSize &&
                pos.z - HalfSize <= z && z < pos.z + HalfSize;
        }

        protected static Vector3Int GetChildPosition(Vector3Int parentPosition, int parentSize, byte childIndex) {
            // order: z->y->x, back/front->down/up->left/right
            return parentPosition +
                new Vector3Int(
                    parentSize / 4 * ((childIndex & 0x1) > 0 ? 1 : -1),
                    parentSize / 4 * ((childIndex & 0x2) > 0 ? 1 : -1),
                    parentSize / 4 * ((childIndex & 0x4) > 0 ? 1 : -1));
        }

        public abstract void Dispose();
    }
    // todo new() instead of struct? would that even help with having an interface?
    // todo maybe pass a delegate constructor instead - Func createFunc<TData>
    [System.Serializable]
    public sealed class OctreeLeaf<TData> : OctreeNode<TData> where TData : struct {

        public TData data;

        public OctreeLeaf(OctreeNode<TData> parent, byte childIndex)
            : base(parent.height - 1, GetChildPosition(parent.pos, parent.GetSize(), childIndex)) {
            Debug.Assert(0 <= childIndex && childIndex < 8);
        }

        public override void Dispose() {
            if (data is System.IDisposable ddata) {
                ddata.Dispose();
            }
        }
    }
    // public class OctreeParent : OctreeNode, System.Collections.IEnumerable {
    [System.Serializable]
    public sealed class OctreeParent<TData> : OctreeNode<TData>, System.Collections.IEnumerable
        where TData : struct {

        // front/back up/down left/right
        // [SerializeField] OctreeNode childBDL;
        // [SerializeField] OctreeNode childBDR;
        // [SerializeField] OctreeNode childBUL;
        // [SerializeField] OctreeNode childBUR;
        // [SerializeField] OctreeNode childFDL;
        // [SerializeField] OctreeNode childFDR;
        // [SerializeField] OctreeNode childFUL;
        // [SerializeField] OctreeNode childFUR;
        [SerializeField] OctreeNode<TData>[] children = null;

        public OctreeParent(byte height)
            : base(height, Vector3Int.zero) {
        }

        OctreeParent(OctreeParent<TData> parent, byte childIndex)
        : base(parent.height - 1, GetChildPosition(parent.pos, parent.GetSize(), childIndex)) {
            Debug.Assert(0 <= childIndex && childIndex < 8);
        }

        public bool HasChildren() => children != null;
        public OctreeNode<TData>[] GetChildren() => children;
        public OctreeNode<TData> GetChild(int x, int y, int z) => GetChild(GetChildIndex(x, y, z));
        public OctreeNode<TData> GetChild(Vector3Int pos) => GetChild(pos.x, pos.y, pos.z);
        public OctreeNode<TData> GetChild(int childIndex) {
            if (childIndex < 0 || childIndex > 8) {
                Debug.LogError($"GetChild child index invalid: {childIndex}");
                return null;
            }
            return children[childIndex];
        }
        public OctreeLeaf<TData> GetLeafChild(int x, int y, int z) => GetLeafChild(GetChildIndex(x, y, z));
        public OctreeLeaf<TData> GetLeafChild(Vector3Int pos) => GetLeafChild(pos.x, pos.y, pos.z);
        public OctreeLeaf<TData> GetLeafChild(int childIndex) {
            return (OctreeLeaf<TData>)GetChild(childIndex);
        }
        public OctreeParent<TData> GetParentChild(int x, int y, int z) => GetParentChild(GetChildIndex(x, y, z));
        public OctreeParent<TData> GetParentChild(Vector3Int pos) => GetParentChild(pos.x, pos.y, pos.z);
        public OctreeParent<TData> GetParentChild(int childIndex) {
            return (OctreeParent<TData>)GetChild(childIndex);
        }

        void CreateChildren() {
            Debug.Assert(!HasChildren() && !IsLeaf());
            children = new OctreeNode<TData>[8];
            for (int i = 0; i < 8; i++) {
                if (height == 1) {
                    // todo sparse octree
                    // make leaf
                    children[i] = new OctreeLeaf<TData>(this, (byte)i);
                } else {
                    // make parent
                    children[i] = new OctreeParent<TData>(this, (byte)i);
                }
            }
        }
        void DestroyChildren() {
            Debug.Assert(HasChildren());
            foreach (var child in children) {
                child.Dispose();
            }
            children = null;
        }
        public IEnumerator GetEnumerator() {
            return children.GetEnumerator();
        }

        int GetChildIndex(int x, int y, int z) {
            // from absolute pos
            /*
            bottom-top, back-front, left-right
            y-z-x order
            i: xyz
            0: 000
            1: 100
            4: 001
            5: 101
            2: 010
            3: 110
            6: 011
            7: 111
            */
            return (x >= pos.x ? 1 : 0) + 4 * (y >= pos.y ? 1 : 0) + 2 * (z >= pos.z ? 1 : 0);
        }
        Vector3Int GetRelPosFromIndex(byte index) {
            // gets the relative pos
            // just get child at the index, it should know its pos
            // todo test
            int x = (index) & 1;
            int z = (index >> 1) & 1;
            int y = (index >> 2) & 1;
            return new Vector3Int(x, y, z);
        }

        public override void Dispose() {
            if (HasChildren()) {
                DestroyChildren();
            }
        }

        public OctreeNode<TData> this[int index] => children[index];
    }
}