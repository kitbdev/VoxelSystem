using UnityEngine;
using UnityEditor;

namespace VoxelSystem {
    [CustomPropertyDrawer(typeof(VoxelTypeId))]
    public class VoxelTypeIdDrawer : PropertyDrawer {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            // todo 
            // draw a dropdown menu if connected to a voxel material set, either through voxelworld or something
            EditorGUI.PropertyField(position, property.FindPropertyRelative(nameof(VoxelTypeId.id)), label);
        }
    }
}