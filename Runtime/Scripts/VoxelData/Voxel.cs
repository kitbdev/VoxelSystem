
namespace VoxelSystem {
    public struct Voxel<TVal, TMat> 
        where TVal: IVoxelValue 
        // where TMat: VoxelMaterialData 
        {
        TVal value;
        TMat mat;
    }
    // public struct Voxel {
    //     IVoxelValue value;
    //     VoxelMaterialData mat;
    // }
    public interface IVoxelValue {
    }
    public struct VoxelNoValue : IVoxelValue {}
    public struct VoxelId32Value : IVoxelValue {
        public int id;
    }
    public struct VoxelId8Value : IVoxelValue {
        public byte id;
    }
    public struct VoxelDensityValue : IVoxelValue {
        public float density;
    }
}