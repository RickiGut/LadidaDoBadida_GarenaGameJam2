using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace RomDev
{
    #if UNITY_EDITOR
    public class EditorHelper 
    {
        public static IEnumerable<SerializedProperty> GetChildSerializedProperties(SerializedProperty _serializedProperty)
        {
            IEnumerator propEnumerator = _serializedProperty.GetEnumerator();
            while(propEnumerator.MoveNext())
            {
                SerializedProperty getSerializedProp = propEnumerator.Current as SerializedProperty;
                if(getSerializedProp == null) continue;
                yield return getSerializedProp;
            }
        }
        public static IEnumerable<SerializedPack<T>> GetDictSerializedPack<Tkey,T>(SerializedProperty serializedProperty, Dictionary<Tkey, T> dictionary)
        {
            SerializedPack<T> pack = new SerializedPack<T>();
            IEnumerator propEnumerator = serializedProperty.GetEnumerator();
            Dictionary<Tkey, T>.Enumerator dictEnumerator = dictionary.GetEnumerator();
            while(propEnumerator.MoveNext())
            {
                dictEnumerator.MoveNext();
                KeyValuePair<Tkey, T> keyValuePair = dictEnumerator.Current;
                pack.serializedProperty = propEnumerator.Current as SerializedProperty;
                pack.packValue = keyValuePair.Value;
                if(pack.serializedProperty == null) continue;
                if(pack.packValue == null) continue;  
                yield return pack;  
            }
        }

        public static IEnumerable<SerializedPack<T>> GetPairSerializedPack<T,T2>(SerializedProperty serializedProperty, T2 pairedCollection) where T2 : IEnumerable<T>
        {
            SerializedPack<T> pack = new SerializedPack<T>();
            IEnumerator propEnumerator = serializedProperty.GetEnumerator();
            IEnumerator collectionEnumerator = pairedCollection.GetEnumerator();
            while(propEnumerator.MoveNext())
            {
                collectionEnumerator.MoveNext();
                if(collectionEnumerator.Current is T getValue)
                {
                    pack.serializedProperty = propEnumerator.Current as SerializedProperty;
                    pack.packValue = getValue;
                    if(pack.serializedProperty == null) continue;
                    if(pack.packValue == null) continue;
                    yield return pack;  
                }
            }
        }
        public static void ShowPropertyOnLine(string labelText, int maxWidth, SerializedProperty targetProp)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(labelText, GUILayout.MaxWidth (maxWidth));
            EditorGUILayout.PropertyField(targetProp, new GUIContent());
            EditorGUILayout.EndHorizontal();
        }
    }
    public class SerializedPack<T>
    {
        public SerializedProperty serializedProperty;
        public T packValue;
    }
    #endif
}

