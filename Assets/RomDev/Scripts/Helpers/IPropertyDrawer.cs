using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace RomDev
{
    public interface IPropertyDrawer 
    {
        #if UNITY_EDITOR
        public void DrawProperty(SerializedProperty parentProperty);
        #endif
    }
    public interface IPropertyDrawerL2
    {
        #if UNITY_EDITOR
        public void DrawPropertyL2(SerializedProperty parentProperty, int numIndex = 0);
        #endif
    }
    public interface IPropertyDrawerL3
    {
        #if UNITY_EDITOR
        public void DrawPropertyL3(PropertyDataPack propertyDataPack);
        #endif
    }
    #if UNITY_EDITOR
    [Serializable]
    public class PropertyDataPack
    {
        public SerializedProperty parentProperty;
        public int numIndex;
        public SerializedObject parentSerializedObj;
    }
    #endif
}

