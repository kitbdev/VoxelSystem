using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.IO;
using System.Collections;

namespace VoxelSystem {
    public class RLEVoxelsEncoding<VoxelT> : ISaveable where VoxelT : struct, IVoxel {
        public class RLVoxels {
            public System.UInt16 count;
            public VoxelT value;
        }
        public Vector3Int size;
        public List<RLVoxels> rlVoxels = new List<RLVoxels>();

        public static RLEVoxelsEncoding<VoxelT> FromVolume(VoxelVolume<VoxelT> fromVolume) {
            RLEVoxelsEncoding<VoxelT> rleVox = new RLEVoxelsEncoding<VoxelT>();
            rleVox.size = fromVolume.Size;
            VoxelT[] flatVoxels = fromVolume.ToFlatArray().ToArray();
            RLVoxels curRL = null;
            foreach (var vox in flatVoxels) {
                if (curRL != null && curRL.value.Equals(vox)) {
                    curRL.count++;
                } else {
                    if (curRL != null) {
                        rleVox.rlVoxels.Add(curRL);
                    }
                    curRL = new RLVoxels() {
                        count = 1,
                        value = vox,
                    };
                }
            }
            return rleVox;
        }
        public VoxelVolume<VoxelT> ToVolume() {
            List<VoxelT> voxels = new List<VoxelT>();
            foreach (var rlVoxel in rlVoxels) {
                for (int i = 0; i < rlVoxel.count; i++) {
                    voxels.Add(rlVoxel.value);
                }
            }
            VoxelVolume<VoxelT> voxelVolume = new VoxelVolume<VoxelT>(size);
            voxelVolume.PopulateWithExistingVoxels(voxels.ToArray());
            return voxelVolume;
        }
        public string GetName() => $"RLEVoxelsEncoding<{new VoxelT().GetName()}>";
        public string GetVersion() => "0.1";

        public void Save(Stream writer) {
            // todo an rle identifier?
            writer.Write(size);
            foreach (var rlVoxel in rlVoxels) {
                rlVoxel.value.Save(writer);
                writer.Write(rlVoxel.count);
                // todo
                // ? some seperator?
            }
        }
        public void Load(Stream reader) {
            // todo
            // reader.Read()
        }
    }
    public class BoxVoxelsEncoding<VoxelT> : IEnumerable where VoxelT : struct, IVoxel {
        public struct Box {
            public Vector3Int start;
            public Vector3Int size;
            public BoundsInt area => new BoundsInt(start, size);
            public VoxelT value;
        }
        public Vector3Int size;
        public List<Box> boxes = new List<Box>();

        public static BoxVoxelsEncoding<VoxelT> FromVolume(VoxelVolume<VoxelT> fromVolume) {
            BoxVoxelsEncoding<VoxelT> boxVoxelsEncoding = new BoxVoxelsEncoding<VoxelT>();
            boxVoxelsEncoding.size = fromVolume.Size;
            VoxelT[][][] voxels = fromVolume.voxels;
            Box box;
            for (int y = 0; y < fromVolume.Size.y; y++) {
                for (int z = 0; z < fromVolume.Size.z; z++) {
                    for (int x = 0; x < fromVolume.Size.x; x++) {
                        // new box
                        box = new Box();
                        box.value = voxels[y][z][x];
                        box.start = new Vector3Int(x, y, z);
                        // todo

                        boxVoxelsEncoding.boxes.Add(box);
                    }
                }
            }

            return boxVoxelsEncoding;
        }
        public VoxelVolume<VoxelT> ToVolume() {
            // go through the boxes and 'draw' their value
            VoxelVolume<VoxelT> voxelVolume = new VoxelVolume<VoxelT>(size);
            foreach (var box in boxes) {
                voxelVolume.SetVoxels(box.area, box.value);
            }
            return voxelVolume;
        }
        public string GetName() => $"BoxVoxelsEncoding<{new VoxelT().GetName()}>";
        public string GetVersion() => "0.1";

        public IEnumerator GetEnumerator() {
            throw new System.NotImplementedException();
        }
    }
}