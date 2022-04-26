using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Kutil;
using System.Linq;
using System.Runtime.CompilerServices;
using System.IO;
using System;

namespace VoxelSystem {
    /// <summary>
    /// Holds voxels in a 3 dimensional volume. using a sparse dictionary with position as key
    /// </summary>
    [System.Serializable]
    public class VoxelDictVolume<VoxelT> : IVoxelVolume<VoxelT>, ISaveable where VoxelT : struct, IVoxel {

        [SerializeField]
        SerializableDictionary<Vector3Int, VoxelT> voxelDict = new SerializableDictionary<Vector3Int, VoxelT>();
        Vector3Int maxDimensions;

        /// <summary>
        /// if the dict doesnt have a value, what should be assumed. Typically, empty or air
        /// </summary>
        public VoxelT defaultVoxel;

        public Vector3Int Size => maxDimensions;

        // todo store entire bounds
        // BoundsInt bounds;
        public BoundsInt GetBounds() {
            return new BoundsInt(Vector3Int.zero, Size);
        }

        public void Init(Vector3Int newSize) {
            maxDimensions = newSize;
            defaultVoxel = new VoxelT();
            defaultVoxel.Init((VoxelMaterialIdVD)0);
        }

        public void ClearAllVoxels() {
            voxelDict.Clear();
        }

        public void FinishUpdating() {
            // nothing
        }

        public VoxelT GetVoxelAt(Vector3Int pos) {
            if (voxelDict.ContainsKey(pos)) {
                return voxelDict[pos];
            }
            return defaultVoxel;
        }

        public bool HasVoxelAt(Vector3Int pos) {
            return GetBounds().Contains(pos);
        }

        public void SetVoxel(Vector3Int pos, VoxelT newVoxel) {
            bool isDefaultVoxel = newVoxel.Equals(defaultVoxel);
            if (!voxelDict.ContainsKey(pos)) {
                if (!isDefaultVoxel) {
                    voxelDict.Add(pos, newVoxel);
                }
            } else {
                if (isDefaultVoxel) {
                    voxelDict.Remove(pos);
                } else {
                    voxelDict[pos] = newVoxel;
                }
            }
        }

        public void SetVoxels(BoundsInt area, Func<Vector3Int, VoxelT, VoxelT> setFunc) {
            foreach (Vector3Int pos in area.allPositionsWithin) {
                ((IVoxelVolume<VoxelT>)this).TryGetVoxelAt(pos, out var v);
                SetVoxel(pos, setFunc(pos, v));
            }
        }

        public void SetVoxels(Vector3Int startOffset, IVoxelVolume<VoxelT> fromVoxels) {
            foreach (FullVoxel vox in fromVoxels.GetFullVoxelEnumerable()) {
                if (!vox.voxel.Equals(defaultVoxel)) {
                    SetVoxel(vox.pos, (VoxelT)vox.voxel);
                }
            }
        }
        public IEnumerable<FullVoxel> GetFullVoxelEnumerable() {
            return voxelDict.Select(kvp => new FullVoxel(kvp.Value, kvp.Key));
        }


        public IEnumerator GetEnumerator() {
            return voxelDict.GetEnumerator();
        }

        // save load

        public string GetName() => $"VoxelDict{nameof(VoxelT)}";
        public string GetVersion() => "0.1";
        string endKey => GetName() + "END";

        public void Load(Stream reader) {
            // this seems... eh
            // todo test
            while (!reader.CheckReadStr(endKey)) {
                var k = reader.ReadV3i();
                VoxelT voxelT = new VoxelT();
                voxelT.Load(reader);
                voxelDict.Add(k, voxelT);
            }
        }

        public void Save(Stream writer) {
            foreach (var kvp in voxelDict) {
                writer.Write(kvp.Key);
                kvp.Value.Save(writer);
            }
            writer.Write(endKey);
        }
    }
}