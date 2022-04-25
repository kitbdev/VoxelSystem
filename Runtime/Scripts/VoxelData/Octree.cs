using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
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
    /// <summary>
    /// Either a parent or a leaf for an octree
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    [System.Serializable]
    public abstract class OctreeNode<TData> : System.IDisposable
    where TData : struct {

        public int chunksize;

        public OctreeParent<TData> parent;
        public byte childIndex;

        public int height;
        public Vector3Int pos;

        public OctreeNode(int height, Vector3Int pos, OctreeParent<TData> parent, byte childIndex) {
            this.height = height;
            this.pos = pos;
            this.parent = parent;
            this.childIndex = childIndex;
            Debug.Assert(height < 32);
        }
        public abstract bool IsLeaf();
        public bool IsBottomLevel() => height == 0;
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

        public abstract TData GetValue(Vector3Int pos);
        public abstract void SetValue(Vector3Int pos, TData value);
        public abstract void SetAllToValue(TData value);

        /// <summary>
        /// Change into a leaf node if all children have the same value. 
        /// recursively, start with children.
        /// </summary>
        public void Prune() {
            // todo
            if (this is OctreeParent<TData> parent) {
                // recursively prune children
                for (int i = 0; i < 8; i++) {
                    OctreeNode<TData> child = parent.GetChild(i);
                    if (!child.IsLeaf()) {
                        child.Prune();
                    }
                }
                // check that all children are leafs, they were all successfully pruned 
                for (int i = 0; i < 8; i++) {
                    OctreeNode<TData> child = parent.GetChild(i);
                    if (!child.IsLeaf()) {
                        return;
                    }
                }
                // check if all values are the same
                TData val = parent.GetLeafChild(0).GetValue();
                for (int i = 0; i < 8; i++) {
                    OctreeLeaf<TData> child = parent.GetLeafChild(i);
                    if (!child.GetValue().Equals(val)) {
                        return;
                    }
                }
                // merge
                parent.ConvertParentChildToLeaf(childIndex, val);
                // todo anything else?
            }// if leaf do nothing
        }

        public abstract void Dispose();
    }
    // todo new() instead of struct? would that even help with having an interface?
    // todo maybe pass a delegate constructor instead - Func createFunc<TData>
    [System.Serializable]
    public sealed class OctreeLeaf<TData> : OctreeNode<TData> where TData : struct {

        TData data;
        public override bool IsLeaf() => true;

        public OctreeLeaf(OctreeParent<TData> parent, byte childIndex)
            : base(parent.height - 1, GetChildPosition(parent.pos, parent.GetSize(), childIndex),
            parent, childIndex) {
            Debug.Assert(0 <= childIndex && childIndex < 8);
        }

        public override void Dispose() {
            if (data is System.IDisposable ddata) {
                ddata.Dispose();
            }
        }

        public TData GetValue() => data;
        public override TData GetValue(Vector3Int pos) {
            return data;
        }

        public override void SetValue(Vector3Int pos, TData value) {
            if (IsBottomLevel()) {
                data = value;
            } else {
                Split().SetValue(pos, value);
            }
        }
        public override void SetAllToValue(TData value) {
            data = value;
        }
        public OctreeParent<TData> Split() {
            return parent.ConvertLeafChildToParent(childIndex);
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

        public override bool IsLeaf() => false;

        /// <summary>
        /// Create new root parent
        /// </summary>
        /// <param name="height"></param>
        public OctreeParent(byte height)
            : base(height, Vector3Int.zero, null, 0) {
            CreateChildren();
        }

        OctreeParent(OctreeParent<TData> parent, byte childIndex)
        : base(parent.height - 1, GetChildPosition(parent.pos, parent.GetSize(), childIndex), parent, childIndex) {
            Debug.Assert(0 <= childIndex && childIndex < 8);
            CreateChildren();
        }

        public bool HasChildren() => children != null;
        public OctreeNode<TData>[] GetChildren() => children;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeNode<TData> GetChild(int x, int y, int z) => GetChild(GetChildIndex(x, y, z));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeNode<TData> GetChild(Vector3Int pos) => GetChild(pos.x, pos.y, pos.z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeNode<TData> GetChild(int childIndex) {
            // assumes its valid. GetChildIndex only returns 0-7 anyway
            // if (childIndex < 0 || childIndex > 8) {
            //     Debug.LogError($"GetChild child index invalid: {childIndex}");
            //     return null;
            // }
            return children[childIndex];
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeLeaf<TData> GetLeafChild(int x, int y, int z) => GetLeafChild(GetChildIndex(x, y, z));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeLeaf<TData> GetLeafChild(Vector3Int pos) => GetLeafChild(pos.x, pos.y, pos.z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeLeaf<TData> GetLeafChild(int childIndex) {
            return (OctreeLeaf<TData>)GetChild(childIndex);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeParent<TData> GetParentChild(int x, int y, int z) => GetParentChild(GetChildIndex(x, y, z));
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeParent<TData> GetParentChild(Vector3Int pos) => GetParentChild(pos.x, pos.y, pos.z);
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public OctreeParent<TData> GetParentChild(int childIndex) {
            return (OctreeParent<TData>)GetChild(childIndex);
        }

        void CreateChildren() {
            Debug.Assert(!HasChildren() && !IsLeaf());
            children = new OctreeNode<TData>[8];
            for (int i = 0; i < 8; i++) {
                // if (height == 1) {
                //     // todo sparse octree
                // make leaf
                children[i] = new OctreeLeaf<TData>(this, (byte)i);
                // } else {
                //     // make parent
                //     children[i] = new OctreeParent<TData>(this, (byte)i);
                // }
            }
        }
        public OctreeParent<TData> ConvertLeafChildToParent(byte childIndex) {
            if (children[childIndex] is OctreeParent<TData> p) {
                // already done?
                return p;
            }
            TData data = ((OctreeLeaf<TData>)children[childIndex]).GetValue();
            // children[childIndex].Dispose();// todo dispose?
            children[childIndex] = new OctreeParent<TData>(this, childIndex);
            OctreeParent<TData> newChild = (OctreeParent<TData>)children[childIndex];
            newChild.SetAllToValue(data);
            return newChild;
        }
        public OctreeLeaf<TData> ConvertParentChildToLeaf(byte childIndex, TData value) {
            if (children[childIndex] is OctreeLeaf<TData> leaf) {
                // already done?
                return leaf;
            }
            children[childIndex].Dispose();// todo dispose?
            children[childIndex] = new OctreeLeaf<TData>(this, childIndex);
            OctreeLeaf<TData> newChild = (OctreeLeaf<TData>)children[childIndex];
            newChild.SetAllToValue(value);
            return newChild;
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

        /// <summary>
        /// Gets appropriate child index from world pos. In y -> z -> x order.
        /// Does not check bounds
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns>child index from 0-8</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
            return BoolTo01(x >= pos.x) + 4 * BoolTo01(y >= pos.y) + 2 * BoolTo01(z >= pos.z);
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int BoolTo01(bool val) {
            // unsafe {
            //     return *((byte*)(&val)); // 1
            // }
            return val ? 1 : 0;
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

        public override TData GetValue(Vector3Int pos) {
            if (!GetBounds().Contains(pos)) {
                // todo need to check at each level?
                Debug.LogError($"Cannot get value {pos} is oob {GetBounds()} in Octree parent node<{nameof(TData)}>!");
                return default;
            }
            return GetChild(pos).GetValue(pos);
        }
        public override void SetValue(Vector3Int pos, TData value) {
            GetChild(pos).SetValue(pos, value);
        }
        public override void SetAllToValue(TData value) {
            for (int i = 0; i < 8; i++) {
                children[i].SetAllToValue(value);
            }
        }


        public OctreeNode<TData> this[int index] => children[index];
    }
}