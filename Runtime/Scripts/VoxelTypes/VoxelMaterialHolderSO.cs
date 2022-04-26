using UnityEngine;
using Kutil;
using System.Linq;
using System.Collections.Generic;

namespace VoxelSystem {
    // public  class VoxelTypeHolderSO : VoxelTypeHolderSOType<IVoxel>  {}
    /// <summary>
    /// Holds all of a VoxelType so they can be easily found, configured, and used by other components
    /// </summary>
    public abstract class VoxelMaterialHolder : ScriptableObject {

        public abstract IEnumerable<KeyValuePair<VoxelMaterialId, IVoxelMaterial>> GetAllTypes();
        public abstract bool HasVoxelTypeId(VoxelMaterialIdVD id);
        public abstract bool HasVoxelTypeId(VoxelMaterialId id);
        public abstract bool HasVoxelType(IVoxelMaterial voxelType);
        public abstract VoxelMaterialId GetIdForVoxelType(IVoxelMaterial voxelType);
        public abstract IVoxelMaterial GetVoxelType(VoxelMaterialId id);
        public abstract IVoxelMaterial GetVoxelType(VoxelMaterialIdVD id);
        public abstract VoxelMaterialIdVD GetVoxMatIdVD(VoxelMaterialId id);
    }
    // ?
    // todo? prob just remove type constraint and use actual in meshers
    // or remove specific ones and just use this
    // actually specific ones are just to add the menu
    public abstract class VoxelMaterialHolderSOType<VoxelMaterialT> : VoxelMaterialHolder where VoxelMaterialT : IVoxelMaterial, new() {
        // public abstract class VoxelTypeHolderSOType<VoxelT> : ScriptableObject where VoxelT : IVoxelType {

        // using a dictionary instead of array because ids may not be contiguous and allows easier updating

        // for inspector
        // [SerializeField]
        // VoxelT[] voxelTypesArray = new VoxelT[0];

        [System.Serializable] class VoxDict : SerializableDictionary<VoxelMaterialId, VoxelMaterialT> { }
        [SerializeField]
        VoxDict voxelTypeDict = new VoxDict();
        IEnumerable<VoxelMaterialT> voxelTypes => voxelTypeDict.Values;


        private void Awake() {
            EnsureEmptyTypeIsAdded();
        }

        private void OnValidate() {
            UpdateDictFromInspector();
        }

        // general

        public override IEnumerable<KeyValuePair<VoxelMaterialId, IVoxelMaterial>> GetAllTypes() {
            return voxelTypeDict.Select(kvp => new KeyValuePair<VoxelMaterialId, IVoxelMaterial>(kvp.Key, kvp.Value)).ToArray();
        }

        public override bool HasVoxelTypeId(VoxelMaterialId id) {
            return voxelTypeDict.ContainsKey(id);
        }
        public override bool HasVoxelType(IVoxelMaterial voxelType) => HasVoxelType(voxelType);
        public bool HasVoxelType(VoxelMaterialT voxelType) {
            return voxelTypeDict.Any(kp => kp.Value == voxelType);
        }
        public override VoxelMaterialId GetIdForVoxelType(IVoxelMaterial voxelType) => GetIdForVoxelType(voxelType);
        public VoxelMaterialId GetIdForVoxelType(VoxelMaterialT voxelType) {
            if (!HasVoxelType(voxelType)) {
                Debug.LogWarning($"VoxelTypeId {voxelType} not found!");
                return VoxelMaterialId.INVALID;
            }
            return voxelTypeDict.First(kp => kp.Value == voxelType).Key;
        }
        public override IVoxelMaterial GetVoxelType(VoxelMaterialId id) {
            return (IVoxelMaterial)GetVoxelTypeT(id);
        }
        public VoxelMaterialT GetVoxelTypeT(VoxelMaterialId id) {
            if (!voxelTypeDict.ContainsKey(id)) {
                Debug.LogWarning($"VoxelTypeId {id} not found!");
                return null;
            }
            return voxelTypeDict[id];
        }

        public override bool HasVoxelTypeId(VoxelMaterialIdVD id) {
            // todo by index? or store with normal typeid
            return false;
        }
        public override IVoxelMaterial GetVoxelType(VoxelMaterialIdVD voxelType) {
            if (!HasVoxelTypeId(voxelType)) {
                Debug.LogWarning($"VoxelTypeIdVD {voxelType} not found!");
                return null;
            }
            // todo
            // return voxelTypeDict.First(kp => kp.Value == voxelType).Key;
            return null;
        }

        public override VoxelMaterialIdVD GetVoxMatIdVD(VoxelMaterialId matId) {
            // dont use field on property
            // todo update dict matid then use that one
            int id = voxelTypeDict.Keys.ToList().FindIndex(key => key.Equals(matId));
            return new VoxelMaterialIdVD(id);
        }
        VoxelMaterialIdVD UpdateVoxelTypeIdVoxelData(VoxelMaterialIdVD id) {
            // todo something like this, when changing representation
            // but apply to voxel volumes
            return id;
        }

        // for type editors and loaders

        public virtual void ClearVoxelTypes() {
            voxelTypeDict.Clear();
            UpdateInspectorRepresentation();
        }
        public virtual void RemoveVoxelType(VoxelMaterialT voxType) {
            VoxelMaterialId voxelTypeId = GetIdForVoxelType(voxType);
            if (voxelTypeId.IsValid()) {
                RemoveVoxelType(voxelTypeId);
            }
        }
        public virtual void RemoveVoxelType(VoxelMaterialId id) {
            voxelTypeDict.Remove(id);
            UpdateInspectorRepresentation();
        }
        public virtual VoxelMaterialId AddVoxelType(VoxelMaterialT newVoxType) {
            VoxelMaterialId newId = new VoxelMaterialId(ToIdName(newVoxType.name));
            voxelTypeDict.Add(newId, newVoxType);
            UpdateInspectorRepresentation();
            return newId;
        }
        string ToIdName(string displayName) {
            return displayName.Replace(" ", "_").ToLower().Trim();
        }
        // public struct KeyUpdate{
        //     public VoxelTypeId oldKey;
        //     public VoxelTypeId newKey;
        // }
        // public virtual void UpdateKeys(KeyUpdate[] keyUpdates){

        // }

        // protected VoxelTypeId NewKey() {
        //     // get first value not in dict
        //     int keyId = 0;
        //     while (voxelTypeDict.ContainsKey(new VoxelTypeId(keyId))) {
        //         keyId++;
        //     }
        //     return new VoxelTypeId(keyId);
        // }

        private void EnsureEmptyTypeIsAdded() {
            VoxelMaterialId emptyId = VoxelMaterialId.EMPTY;
            // note empty type can be changed from default, but it must be at id 0
            if (!voxelTypeDict.ContainsKey(emptyId)) {
                voxelTypeDict.Add(emptyId, (VoxelMaterialT)(new VoxelMaterialT()).GetEmptyType());
                // todo put at start of dict, somehow
                //?KeyedCollection ?serializeddict util?
            }
        }
        protected void UpdateInspectorRepresentation() {
            // voxelTypesArray = voxelTypes.ToArray();
        }
        protected void UpdateDictFromInspector() {
            // // todo were losing typeid!
            // Dictionary<VoxelTypeId, VoxelT> dictionary = voxelTypesArray.Zip(
            //                 Enumerable.Range(0, voxelTypesArray.Length),
            //                 (vt, i) => new KeyValuePair<VoxelTypeId, VoxelT>(i, vt)).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            // // validate it
            // // make sure keys are unique
            // if (dictionary.Keys.Count() != dictionary.Keys.Distinct().Count()) {
            //     Debug.LogWarning("type keys not unique! " + dictionary.Keys.Except(dictionary.Keys.Distinct()).ToStringFull());
            //     return;
            // }

            // // set dict from inspector array
            // voxelTypeDict.Clear();
            // foreach (var kvp in dictionary) {
            //     voxelTypeDict.Add(kvp.Key, kvp.Value);
            // }
            // todo auto number 

            EnsureEmptyTypeIsAdded();
            // UpdateInspectorRepresentation();
        }


        // for meshers

        public virtual Material[] GetUniqueMaterials(VoxelMaterialId[] ids) {
            return voxelTypes.SelectMany(vct => vct.GetAllMaterials()).Where(m => m != null).Distinct().ToArray();
            // return default;
        }
    }
}