using UnityEngine;
using Kutil;
using System.Linq;
using System.Collections.Generic;

namespace VoxelSystem {
    // public  class VoxelTypeHolderSO : VoxelTypeHolderSOType<IVoxel>  {}
    /// <summary>
    /// Holds all of a VoxelType so they can be easily found, configured, and used by other components
    /// </summary>
    public abstract class VoxelTypeHolder : ScriptableObject {

        public abstract IEnumerable<KeyValuePair<VoxelTypeId, IVoxelType>> GetAllTypes();
        public abstract bool HasVoxelTypeId(VoxelTypeIdVoxelData id);
        public abstract bool HasVoxelTypeId(VoxelTypeId id);
        public abstract bool HasVoxelType(IVoxelType voxelType);
        public abstract VoxelTypeId GetIdForVoxelType(IVoxelType voxelType);
        public abstract IVoxelType GetVoxelType(VoxelTypeId id);
        public abstract IVoxelType GetVoxelType(VoxelTypeIdVoxelData id);
    }
    // ?
    // todo prob just remove type constraint and use actual in meshers
    // or remove specific ones and just use this
    // actually specific ones are just to add the menu
    public abstract class VoxelTypeHolderSOType<VoxelT> : VoxelTypeHolder where VoxelT : IVoxelType, new() {
        // public abstract class VoxelTypeHolderSOType<VoxelT> : ScriptableObject where VoxelT : IVoxelType {

        // using a dictionary instead of array because ids may not be contiguous and allows easier updating

        // for inspector
        // [SerializeField]
        // VoxelT[] voxelTypesArray = new VoxelT[0];

        [System.Serializable] class VoxDict : SerializableDictionary<VoxelTypeId, VoxelT> { }
        [SerializeField]
        VoxDict voxelTypeDict = new VoxDict();
        IEnumerable<VoxelT> voxelTypes => voxelTypeDict.Values;


        private void Awake() {
            EnsureEmptyTypeIsAdded();
        }

        private void OnValidate() {
            UpdateDictFromInspector();
        }

        // general

        public override IEnumerable<KeyValuePair<VoxelTypeId, IVoxelType>> GetAllTypes() {
            return voxelTypeDict.Select(kvp => new KeyValuePair<VoxelTypeId, IVoxelType>(kvp.Key, kvp.Value)).ToArray();
        }

        public override bool HasVoxelTypeId(VoxelTypeId id) {
            return voxelTypeDict.ContainsKey(id);
        }
        public override bool HasVoxelType(IVoxelType voxelType) => HasVoxelType(voxelType);
        public bool HasVoxelType(VoxelT voxelType) {
            return voxelTypeDict.Any(kp => kp.Value == voxelType);
        }
        public override VoxelTypeId GetIdForVoxelType(IVoxelType voxelType) => GetIdForVoxelType(voxelType);
        public VoxelTypeId GetIdForVoxelType(VoxelT voxelType) {
            if (!HasVoxelType(voxelType)) {
                Debug.LogWarning($"VoxelTypeId {voxelType} not found!");
                return VoxelTypeId.INVALID;
            }
            return voxelTypeDict.First(kp => kp.Value == voxelType).Key;
        }
        public override IVoxelType GetVoxelType(VoxelTypeId id) {
            return (IVoxelType)GetVoxelTypeT(id);
        }
        public VoxelT GetVoxelTypeT(VoxelTypeId id) {
            if (!voxelTypeDict.ContainsKey(id)) {
                Debug.LogWarning($"VoxelTypeId {id} not found!");
                return null;
            }
            return voxelTypeDict[id];
        }

        public override bool HasVoxelTypeId(VoxelTypeIdVoxelData id) {
            // todo by index? or store with normal typeid
            return false;
        }
        public override IVoxelType GetVoxelType(VoxelTypeIdVoxelData voxelType) {
            if (!HasVoxelTypeId(voxelType)) {
                Debug.LogWarning($"VoxelTypeIdVD {voxelType} not found!");
                return null;
            }
            // todo
            // return voxelTypeDict.First(kp => kp.Value == voxelType).Key;
            return null;
        }

        VoxelTypeIdVoxelData UpdateVoxelTypeIdVoxelData(VoxelTypeIdVoxelData id){
            // todo something like this, when changing representation
            // but apply to voxel volumes
            return id;
        }

        // for type editors and loaders

        public virtual void ClearVoxelTypes() {
            voxelTypeDict.Clear();
            UpdateInspectorRepresentation();
        }
        public virtual void RemoveVoxelType(VoxelT voxType) {
            VoxelTypeId voxelTypeId = GetIdForVoxelType(voxType);
            if (voxelTypeId.IsValid()) {
                RemoveVoxelType(voxelTypeId);
            }
        }
        public virtual void RemoveVoxelType(VoxelTypeId id) {
            voxelTypeDict.Remove(id);
            UpdateInspectorRepresentation();
        }
        public virtual VoxelTypeId AddVoxelType(VoxelT newVoxType) {
            VoxelTypeId newId = new VoxelTypeId(ToIdName(newVoxType.name));
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
            VoxelTypeId emptyId = VoxelTypeId.EMPTY;
            // note empty type can be changed from default, but it must be at id 0
            if (!voxelTypeDict.ContainsKey(emptyId)) {
                voxelTypeDict.Add(emptyId, (VoxelT)(new VoxelT()).GetEmptyType());
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

        public virtual Material[] GetUniqueMaterials(VoxelTypeId[] ids) {
            return voxelTypes.SelectMany(vct => vct.GetAllMats()).Where(m => m != null).Distinct().ToArray();
            // return default;
        }
    }
}