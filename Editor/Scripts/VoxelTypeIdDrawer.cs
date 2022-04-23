using UnityEngine;
using UnityEditor;
using System;
using Kutil;
using System.Collections.Generic;

namespace VoxelSystem {
    [CustomPropertyDrawer(typeof(VoxelTypeId))]
    public class VoxelTypeIdDrawer : PropertyDrawer {

        [SerializeField]
        public bool loadAttempted = false;
        // todo need to store this in editor, persistently
        [SerializeField]
        public VoxelTypeHolder typeHolder = null;

        private void OnEnable() {
            // GetDefTypeHolder();
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            SerializedProperty idProp = property.FindPropertyRelative(nameof(VoxelTypeId.id));
            if (property.serializedObject.targetObject is VoxelTypeHolder) {
                // draw default
                EditorGUI.PropertyField(position, idProp, label);
                return;
            }
            if (typeHolder == null && !loadAttempted) {
                // todo draw a dropdown menu if connected to a voxel material set, either through voxelworld or something
                GetDefTypeHolder();
                loadAttempted = true;
            }
            using (var propScope = new EditorGUI.PropertyScope(position, label, property)) {

                Rect rect = EditorGUI.PrefixLabel(position, propScope.content);
                float typeHolderSelectorWidth = 85;
                float spacing = 2;

                Rect voxelTypeDropdownRect = rect;
                voxelTypeDropdownRect.width -= typeHolderSelectorWidth + spacing;

                Rect typeHolderRect = rect;
                typeHolderRect.width = typeHolderSelectorWidth;
                typeHolderRect.x = voxelTypeDropdownRect.xMax + spacing;

                if (typeHolder == null) {
                    // just show id in normal prop field
                    EditorGUI.PropertyField(voxelTypeDropdownRect, idProp, GUIContent.none);
                    // return;
                } else {
                    // draw dropdown menu for voxel type

                    // EditorGUI.LabelField(voxelTypeDropdownRect, "test");
                    VoxelTypeId selectedId = new VoxelTypeId(idProp.intValue);
                    IVoxelType selectedType;
                    if (typeHolder.HasVoxelTypeId(selectedId)) {
                        selectedType = typeHolder.GetVoxelType(selectedId);
                    } else {
                        selectedType = null;
                    }
                    string selectedIdStr = selectedType?.name ?? (!selectedId.IsValid() ? "none" : "unkown");
                    GUIContent buttonContent = new GUIContent(selectedIdStr);
                    if (EditorGUI.DropdownButton(voxelTypeDropdownRect, buttonContent, FocusType.Passive)) {
                        GenericMenu dmenu = new GenericMenu();
                        // add invalid 'none'
                        {
                            bool isSet = !selectedId.IsValid();
                            string content = "none";
                            dmenu.AddItem(new GUIContent(content), isSet, SetMenuItemEvent, new SetMenuItemEventData() {
                                value = -1,
                                property = idProp,
                            });
                        }
                        // add all types
                        IEnumerable<KeyValuePair<VoxelTypeId, IVoxelType>> alltypes = typeHolder.GetAllTypes();
                        foreach (var type in alltypes) {
                            VoxelTypeId optionId = type.Key;
                            IVoxelType optionType = type.Value;
                            bool isSet = selectedId.Equals(optionId);
                            string content = optionType.name;
                            dmenu.AddItem(new GUIContent(content), isSet, SetMenuItemEvent, new SetMenuItemEventData() {
                                value = optionId.id,
                                property = idProp,
                            });
                        }

                        dmenu.DropDown(voxelTypeDropdownRect);
                    }
                }
                // draw object selector for typeholder
                // EditorGUI.LabelField(typeHolderRect, "test2");
                // todo for some reason when this is null, it doesnt show any options

                EditorGUI.BeginChangeCheck();
                var tHolder = (VoxelTypeHolder)EditorGUI.ObjectField(typeHolderRect, label: GUIContent.none, typeHolder, typeof(VoxelTypeHolder), allowSceneObjects: false);
                if (EditorGUI.EndChangeCheck()) {
                    typeHolder = tHolder;
                }
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
            return base.GetPropertyHeight(property, label);
        }
        public void GetDefTypeHolder() {
            // todo not cubic?
            const string defHolderPath = "Default CubicVoxelTypeHolder";
            VoxelTypeHolder voxelTypeHolder = Resources.Load<VoxelTypeHolder>(defHolderPath);
            typeHolder = voxelTypeHolder;
            // Debug.Log("Loading def type holder " + (typeHolder == null ? "null" : "found") + " at " + defHolderPath);
        }


        [Serializable]
        class SetMenuItemEventData {
            public SerializedProperty property;
            public int value;
        }
        void SetMenuItemEvent(object data) {
            var edata = (SetMenuItemEventData)data;
            edata.property.intValue = edata.value;
            edata.property.serializedObject.ApplyModifiedProperties();
        }
    }
}