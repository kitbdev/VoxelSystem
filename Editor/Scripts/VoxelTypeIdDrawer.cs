using UnityEngine;
using UnityEditor;

namespace VoxelSystem {
    [CustomPropertyDrawer(typeof(VoxelMaterialId))]
    public class VoxelTypeIdDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // todo 
            // draw a dropdown menu if connected to a voxel material set, either through voxelworld or something
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(VoxelMaterialId.matId)), label);
        }
    }
}