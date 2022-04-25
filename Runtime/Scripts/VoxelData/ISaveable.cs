using System.IO;
using UnityEngine;

namespace VoxelSystem {
    public interface ISaveable {
        // todo async
        string GetName();
        string GetVersion();
        void Save(Stream writer);
        void Load(Stream reader);
        // note when calling may want to try/catch
    }
    public static class Saveable {
        // utils?
        public static void Write(this Stream writer, int intVal) {
            writer.Write(System.BitConverter.GetBytes(intVal));
        }
        public static int ReadInt32(this Stream reader) {
            byte[] buffer = new byte[sizeof(int)];
            reader.Read(buffer);
            return System.BitConverter.ToInt32(buffer);
        }
        public static void Write(this Stream writer, Vector3Int vector3IntVal) {
            writer.Write(System.BitConverter.GetBytes(vector3IntVal.x));
            writer.Write(System.BitConverter.GetBytes(vector3IntVal.y));
            writer.Write(System.BitConverter.GetBytes(vector3IntVal.z));
        }
        public static Vector3Int ReadV3i(this Stream reader) {
            byte[] buffer = new byte[sizeof(int)];
            Vector3Int v3 = new Vector3Int();
            reader.Read(buffer);
            v3.x = System.BitConverter.ToInt32(buffer);
            reader.Read(buffer);
            v3.y = System.BitConverter.ToInt32(buffer);
            reader.Read(buffer);
            v3.z = System.BitConverter.ToInt32(buffer);
            return v3;
        }
    }
}