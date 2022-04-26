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
            // int
            writer.Write(System.BitConverter.GetBytes(intVal));
        }
        public static void Write(this Stream writer, string strVal) {
            // str
            for (int i = 0; i < strVal.Length; i++) {
                writer.Write(System.BitConverter.GetBytes(strVal[i]));
            }
        }
        public static void Write(this Stream writer, Vector3Int vector3IntVal) {
            // v3i
            writer.Write(System.BitConverter.GetBytes(vector3IntVal.x));
            writer.Write(System.BitConverter.GetBytes(vector3IntVal.y));
            writer.Write(System.BitConverter.GetBytes(vector3IntVal.z));
        }
        public static int ReadInt32(this Stream reader) {
            byte[] buffer = new byte[sizeof(int)];
            reader.Read(buffer);
            return System.BitConverter.ToInt32(buffer);
        }
        public static string ReadStr(this Stream reader, int strlen) {
            // str
            // todo encoding?
            string str = "";
            byte[] buffer = new byte[sizeof(char)];
            for (int i = 0; i < strlen; i++) {
                reader.Read(buffer);
                str += System.BitConverter.ToChar(buffer);
            }
            return str;
        }
        public static bool CheckReadStr(this Stream reader, string strVal) {
            // str
            long rpos = reader.Position;
            // try {
            if (strVal == ReadStr(reader, strVal.Length)) {
                return true;
            }
            reader.Position = rpos;
            return false;
            // ? dont need to catch exceptions?
            // } catch (System.Exception) {
            //     reader.Position = rpos;
            //     return false;
            // }
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