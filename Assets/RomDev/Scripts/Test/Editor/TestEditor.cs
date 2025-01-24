#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
namespace RomDev
{
    [CustomEditor(typeof(Test))]
    public class TestEditor : Editor
    {
        private Test test;
        private void OnEnable() {
            test = (Test)target;
        }
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI(); 
            if(GUILayout.Button("Create Object"))
            {
                test.InstantObject();
            }
            if(GUILayout.Button("Destroy Object"))
            {
                test.DestroyObject();
            }
            if(GUILayout.Button("Add Component"))
            {
                test.AddObjectComponent();
            }
            if(GUILayout.Button("Remove Component"))
            {
                test.RemoveObjectComponent();
            }
        }
    }
}
#endif

