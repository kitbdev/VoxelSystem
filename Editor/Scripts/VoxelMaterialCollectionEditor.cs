using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace VoxelSystem {
    // [CustomEditor(typeof(VoxelMaterialCollectionBaseSO))]
    public class VoxelMaterialCollectionEditor : Editor {

        public override VisualElement CreateInspectorGUI() {
            // return base.CreateInspectorGUI();

            VoxelMaterialCollectionBaseSO voxelMaterialCollection = (VoxelMaterialCollectionBaseSO)target;
            VisualElement root = new VisualElement();
            // root.Add(new Label("hi vmatso"));
            root.Add(new PropertyField(serializedObject.FindProperty("voxelMaterials")));
            // // todo
            // // make list of voxel materials

            // root.Bind(serializedObject);
            return root;
        }
    }
}